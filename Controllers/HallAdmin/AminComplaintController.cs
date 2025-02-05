using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class AminComplaintController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IComplaintManagementRepository _complaintManagementRepository;
        private readonly AdminComplaintHelper _adminComplaintHelper;

        public AminComplaintController(IComplaintManagementRepository complaintManagementRepository)
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
    }
}
