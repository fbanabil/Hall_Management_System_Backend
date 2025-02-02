using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos.HallAdmin.Student;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using System.Security.Claims;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class StudentManagementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentManagementRepository _studentManagementRepository;
        private readonly IPresentDateTime _presentDateTime;
        private readonly AdminStudentManagementHelper _adminStudentManagementHelper;


        public StudentManagementController(IStudentManagementRepository studentManagementRepository, IPresentDateTime presentDateTime)
        {
            _studentManagementRepository = studentManagementRepository;
            _adminStudentManagementHelper = new AdminStudentManagementHelper(_studentManagementRepository);
            _presentDateTime = presentDateTime;
            _mapper = new MapperConfiguration(cfg =>
            {
               
            }).CreateMapper();
        }


        [HttpGet("GetStudentManagementPage")]
        public async Task<IActionResult> GetStudentManagementPage()
        {
            string email = User.FindFirst("userEmail")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains("HallAdmin"))
            {
                return Unauthorized("Login as Admin");
            }

            int? hallId = await _studentManagementRepository.GetHallId(email);

            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }


            //Tuple<int,int> TotalStudentAndIsActive = await Task.Run(() => _studentManagementRepository.GetTotalStudentAndIsActive(hallId.Value));

            StudentManagementPageDto studentManagementPageDto = new StudentManagementPageDto();
            studentManagementPageDto = await _adminStudentManagementHelper.GetStudentManagementPage(hallId.Value);

            return Ok(studentManagementPageDto);

        }




        //[HttpDelete("DeleteStudent/{studentId}")]
        //public async Task<IActionResult> DeleteStudent(int studentId)
        //{
        //    Student student = await _studentManagementRepository.GetStudentById(studentId);
            
        //}



    }
}
