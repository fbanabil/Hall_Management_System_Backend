using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize("StudentPolicy")]
    [ApiController]
    [Route("/[controller]")]
    public class NoticeController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INoticeRepository _noticeRepository;

        public NoticeController(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NoticeToShowDto, Notice>();
                cfg.CreateMap<Notice, NoticeToShowDto>();
                cfg.CreateMap<NoticeToAddDto, Notice>();
                cfg.CreateMap<Notice, NoticeToAddDto>();
            }).CreateMapper();
        }

        [HttpGet("GetNotices")]
        public ActionResult<IEnumerable<NoticeToShowDto>> GetNotice(int pageNumber = 1, int pageSize = 10, string filter = "all", string searchTerm = "")
        {
            string email = User.FindFirst("userEmail")?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Authorization Failed");
            }

            Student? student = _noticeRepository.GetSingleStudent(email);
            if (student == null)
            {
                return BadRequest("Authorization Failed");
            }

            List<NoticeToShowDto> noticeToShowDtos = new List<NoticeToShowDto>();

            IEnumerable<Notice> notices = _noticeRepository.GetNoticesOfHall(student.HallId);

            if (filter != "all")
            {
                notices = notices.Where(n => n.NoticeType.Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                notices = notices.Where(n => n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            notices=notices.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            foreach (Notice notice in notices)
            {
                NoticeToShowDto noticeTo = _mapper.Map<NoticeToShowDto>(notice);
                noticeTo.Date = notice.Date.ToString("dd/MM/yyyy");
                noticeTo.Priority  = _noticeRepository.PriorityOrFavourite(notice.NoticeId, student.Id);
                noticeTo.IsRead = _noticeRepository.IsRead(notice.NoticeId, student.Id);
                noticeToShowDtos.Add(noticeTo);
            }

            if (noticeToShowDtos.Count > 0)
            {
                int totalNotices = _noticeRepository.TotalNoticesOfHall(student.HallId);
                int totalPages = (int)Math.Ceiling(totalNotices / (double)pageSize);
                totalPages++;
                Response.Headers.Add("X-Total-Pages", totalPages.ToString());
                return Ok(noticeToShowDtos);
            }
            else if (noticeToShowDtos.Count == 0)
            {
                return Ok("No Notice Found");
            }
            else
            {
                return BadRequest("No Notice Found");
            }
        }


        [HttpPut("PriorityOrFavourite/{noticeId}")] 
        public IActionResult PriorityOrFavourite(int noticeId)
        {
            string email = User.FindFirst("userEmail")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Authorization Failed");
            }

            Student? student = _noticeRepository.GetSingleStudent(email);

            bool exists=_noticeRepository.PriorityOrFavourite(noticeId, student.Id);

            if (exists)
            {
                _noticeRepository.RemoveEntity<NoticePriority>(_noticeRepository.NoticePriority(noticeId, student.Id));
            }
            else
            {
                NoticePriority noticePriority = new NoticePriority
                {
                    NoticeId = noticeId,
                    StudentId = student.Id
                };
                _noticeRepository.AddEntity<NoticePriority>(noticePriority);
            }

            if (_noticeRepository.SaveChanges())
            {
                //Console.WriteLine(notice.Priority);
                return Ok();
            }
            return BadRequest("Failed to update favourites");
        }


        [HttpPut("MarkAsRead/{noticeId}")]
        public IActionResult MarkAsRead(int noticeId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Authorization Failed");
            }

            Student? student = _noticeRepository.GetSingleStudent(email);

            bool exists = _noticeRepository.IsRead(noticeId, student.Id);
            if (exists) {
                _noticeRepository.RemoveEntity<IsRead>(_noticeRepository.IsReadEntity(noticeId, student.Id));
            }
            else
            {
                IsRead noticePriority = new IsRead
                {
                    NoticeId = noticeId,
                    StudentId = student.Id
                };
                _noticeRepository.AddEntity<IsRead>(noticePriority);
            }

            if (_noticeRepository.SaveChanges())
            {
                //Console.WriteLine(notice.Priority);
                return Ok();
            }
            return BadRequest("Failed to update read");
        }


    }
}
