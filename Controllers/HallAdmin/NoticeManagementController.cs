using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class NoticeManagementController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INoticeManagementRepository _noticeRepository;
        private readonly AdminNoticeManagementHelper _adminNoticeManagementHelper;
        
        private readonly IPresentDateTime _presentDateTime;
        public NoticeManagementController(INoticeManagementRepository noticeRepository,IPresentDateTime presentDateTime)
        {
            _noticeRepository = noticeRepository;
            _presentDateTime = presentDateTime;
            _adminNoticeManagementHelper = new AdminNoticeManagementHelper(noticeRepository);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Notice,NoticeToAddDto>();  
            }).CreateMapper();
        }


        [HttpGet("GetNoticesOfHall")]
        public async Task<ActionResult<AdminNoticePageDto>> GetNoticePage()
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await _noticeRepository.GetHallId(email);
            if (hallId == null)
            {
                return BadRequest("Hall not found");
            }

            AdminNoticePageDto adminNoticePageDto = await _adminNoticeManagementHelper.GetNoticePage(hallId.Value);

            return Ok(adminNoticePageDto);
        }


        [HttpPost("AddNotice")]
        public async Task<ActionResult<AdminNoticePageDto>> AddNotice(NoticeToAddDto noticeToAddDto)
        {
            Console.WriteLine(noticeToAddDto.Title);
            Console.WriteLine(noticeToAddDto.Description);
            Console.WriteLine(noticeToAddDto.NoticeType);
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = _noticeRepository.GetHallId(email).Result;
            Notice notice = new Notice();
            notice.Title = noticeToAddDto.Title;
            notice.Description = noticeToAddDto.Description;
            notice.NoticeType = noticeToAddDto.NoticeType;
            notice.HallId = hallId.Value;
            notice.Priority = false;
            notice.IsRead = false;
            notice.Date = _presentDateTime.GetPresentDateTime();
            _noticeRepository.AddEntity<Notice>(notice);

            if (_noticeRepository.SaveChanges())
            {
                return Ok(await _adminNoticeManagementHelper.GetNoticePage(hallId.Value));
            }
            else
            {
                return BadRequest("Notice Adding Failed" );
            }
        }

        [HttpDelete("DeleteNotice/{noticeId}")]
        public async Task<ActionResult<AdminNoticePageDto>> DeleteNotice(int noticeId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await _noticeRepository.GetHallId(email);
            if (hallId == null)
            {
                return BadRequest("Hall not found");
            }
            Notice notice = await _noticeRepository.GetNotice(noticeId);
            if (notice == null)
            {
                return BadRequest("Notice not found");
            }
            _noticeRepository.RemoveEntity<Notice>(notice);
            if (_noticeRepository.SaveChanges())
            {
                return Ok(await _adminNoticeManagementHelper.GetNoticePage(hallId.Value));
            }
            else
            {
                return BadRequest("Notice Deletion Failed");
            }
        }
    }
}
