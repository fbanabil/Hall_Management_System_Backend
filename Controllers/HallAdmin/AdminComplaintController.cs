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
    public class AdminComplaintController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IComplaintManagementRepository _complaintManagementRepository;
        private readonly AdminComplaintHelper _adminComplaintHelper;

        public AdminComplaintController(IComplaintManagementRepository complaintManagementRepository)
        {
            _complaintManagementRepository = complaintManagementRepository;
            _adminComplaintHelper = new AdminComplaintHelper(_complaintManagementRepository);
            _mapper = new MapperConfiguration(cfg =>
            {

            }
            ).CreateMapper();
        }


        [HttpGet("AdminComplaintOverview")]
        public async Task<IActionResult> GetComplaintPage()
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await _complaintManagementRepository.GetHallId(email);
            AdminComplaintPage adminComplaintPage = await _adminComplaintHelper.GetComplaintPage(hallId.Value);
            Console.WriteLine(adminComplaintPage.TotalComplaints);
            return Ok(adminComplaintPage);
        }

        [HttpGet("ComplaintsToShow")]
        public async Task<IActionResult> ComplaintToShow()
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await _complaintManagementRepository.GetHallId(email);
            IEnumerable<ComplaintToShowDto> complaints = await _adminComplaintHelper.ComplaintToShow(hallId.Value);
            return Ok(complaints);
        }

        [HttpPut("UpdateComplaintStatus/{complaintId}/{newStatus}")]
        public async Task<IActionResult> UpdateComplaintStatus(int complaintId,string newStatus)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await _complaintManagementRepository.GetHallId(email);
            Complaint complaint = await _complaintManagementRepository.GetComplaintById(complaintId);

            complaint.Status = newStatus;
            _complaintManagementRepository.UpdateEntity(complaint);
            //await _complaintManagementRepository.SaveChangesAsync();
            if(await _complaintManagementRepository.SaveChangesAsync())
            {
                return Ok(await _adminComplaintHelper.GetComplaintPage(hallId.Value));
            }
            else
            {
                return BadRequest(new { message = "Something Wrong" });
            }

        }
    }
}
