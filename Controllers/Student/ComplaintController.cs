using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Models;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Repositories;
using Student_Hall_Management.Helpers;
using System;

namespace Student_Hall_Management.Controllers
{
    [Authorize("StudentPolicy")]
    [ApiController]
    [Route("/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IComplaintRepository _complaintRepository;
        private readonly ComplaintHelper _complaintHelper;
        public ComplaintController(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
            _complaintHelper = new ComplaintHelper();

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ComplaintToShowDto, Complaint>();
                cfg.CreateMap<Complaint, ComplaintToShowDto>();
                cfg.CreateMap<ComplaintToAddDto, Complaint>();
                cfg.CreateMap<Complaint, ComplaintToAddDto>();
                cfg.CreateMap<Complaint, Complaint>();
                cfg.CreateMap<CommentToShowDto, Comment>();
                cfg.CreateMap<Comment, CommentToShowDto>();
                cfg.CreateMap<Comment, CommentToAddDto>();
                cfg.CreateMap<CommentToAddDto, Comment>();

            }).CreateMapper();
        }


        [HttpPost("AddComplaint")]
        public IActionResult AddComplaint(ComplaintToAddDto complaintToAddDto)
        {
            string email = User.FindFirst("userEmail")?.Value;

            Student? student = _complaintRepository.GetSingleStudent(email);
            if (student == null)
            {
                return BadRequest("Authorization Failed");
            }

            Complaint complaint = _mapper.Map<Complaint>(complaintToAddDto);

            complaint.StudentId = student.Id;
            complaint.ComplaintDate = _complaintRepository.PresentDateTime();
            complaint.Status = "Pending";
           // Console.WriteLine(complaint.Catagory);
            //Console.WriteLine(complaintToAddDto.Catagory);
            if (student != null) complaint.HallId = student.HallId;

            if(complaint.ImageData != null)
            {
                string uniqueFileName = $"{student.Id}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";

                string filepath =_complaintHelper.StoreImage(complaint.ImageData,uniqueFileName);
                if (filepath != null)
                {
                    complaint.ImageData = filepath;
                }
                else
                {
                    return BadRequest("Failed to store image");
                }
            }



            if (complaint.FileData != null)
            {
                string uniqueFileName = $"{student.Id}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";

                string filepath = _complaintHelper.storeFile(complaint.FileData, uniqueFileName);
                if(filepath != null) {
                    complaint.FileData = filepath;
                }
                else
                {
                    return BadRequest("Failed to store file");

                }
            }

            _complaintRepository.AddEntity<Complaint>(complaint);
            if (_complaintRepository.SaveChanges())
            {
                return Ok(complaint);
            }
            return BadRequest("Failed to add complaint");
        }


        [HttpPost("AddComment/{ComplaintId}")]
        public IActionResult AddComment(CommentToAddDto commentToAddDto,int ComplaintId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            Student? student = _complaintRepository.GetSingleStudent(email);
            if (student == null)
            {
                return BadRequest("Student not found");
            }
            Comment comment = _mapper.Map<Comment>(commentToAddDto);
            comment.StudentId = student.Id;
            comment.CommentedAt = _complaintRepository.PresentDateTime();
            comment.CommentedBy = "Student";
            comment.HallId = student.HallId;
            comment.ComplaintId = ComplaintId;

            _complaintRepository.AddEntity<Comment>(comment);
            if (_complaintRepository.SaveChanges())
            {
                CommentToShowDto commentToShow = _mapper.Map<CommentToShowDto>(comment);
                return Ok(commentToShow);
            }
            return BadRequest("Failed to add comment");
        }

        [HttpGet("GetComplaints")]
        public async Task<ActionResult<IEnumerable<ComplaintToShowDto>>> GetComplaints(int pageNumber = 1, int pageSize = 10)
        {
            string email = User.FindFirst("userEmail")?.Value;
            Student student = _complaintRepository.GetSingleStudent(email);
            if (student == null)
            {
                return BadRequest("You are not authorized to view complaints");
            }

            if (student.HallId == null)
            {
                return BadRequest("No Hall Assigned");
            }

            IEnumerable<Complaint> complaints = _complaintRepository.GetComplaintsOfHall(student.HallId, pageNumber, pageSize);
            List<ComplaintToShowDto> complaintToShowDtos = new List<ComplaintToShowDto>();

            foreach (Complaint c in complaints)
            {
                ComplaintToShowDto complaintToShow = _mapper.Map<ComplaintToShowDto>(c);

                IEnumerable<Comment> comments = await Task.Run(() => _complaintRepository.GetCommentsByComplaitId(c.ComplaintId));
                List<CommentToShowDto> commentToShowDtos = comments.Select(comment => _mapper.Map<CommentToShowDto>(comment)).ToList();

                complaintToShow.Comments = commentToShowDtos;

                if (!string.IsNullOrEmpty(c.ImageData))
                {
                    complaintToShow.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(c.ImageData));
                }
                if (!string.IsNullOrEmpty(c.FileData))
                {
                    complaintToShow.FileData = Convert.ToBase64String(System.IO.File.ReadAllBytes(c.FileData));
                }

                complaintToShowDtos.Add(complaintToShow);
            }
            //remove redundancy by using automapper

            //complaintToShowDtos = complaintToShowDtos.Select(complaint => _complaintHelper.RemoveRedundancy(complaint)).ToList();
            //complaintToShowDtos = _complaintHelper.RemoveRedundancy(complaintToShowDtos);
            //Console.WriteLine(complaintToShowDtos.Count);
            return Ok(complaintToShowDtos);
        }
    }
}
