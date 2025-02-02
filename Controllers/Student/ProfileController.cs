using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using Student_Hall_Management.Dtos;
using System.Security.Claims;
using System.Data;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "Student,HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                //    cfg.CreateMap<ProfileDto, Profile>();
                //    cfg.CreateMap<Profile, ProfileDto>();
                cfg.CreateMap<Student, ProfileToShowDto>();
                cfg.CreateMap<ProfileToShowDto, Student>();
                cfg.CreateMap<HallDetailsToShowDto, HallDetails>();
                cfg.CreateMap<HallDetails, HallDetailsToShowDto>();
            }).CreateMapper();
        }


        //[AllowAnonymous]
        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            string? email = User.FindFirst("userEmail")?.Value.ToString();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            //Console.WriteLine(email);

            if(roles.Contains("Student"))
            {
                Student student = new Student();
                if (email != null)
                {
                    student = _profileRepository.GetSingleStudentProfile(email);
                }
                //Console.WriteLine(student.Email);

                //studen.imagedata=string64 of the saved pathh in it

                student.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(student.ImageData));
                ProfileToShowDto studentToShow = _mapper.Map<ProfileToShowDto>(student);

                if (student.HallId == 0)
                {
                    studentToShow.HallName = "No Hall Alloted";
                }
                else
                {
                    studentToShow.HallName = _profileRepository.GetHallName(student.HallId);
                }
                if (studentToShow.RoomNo == null)
                {
                    studentToShow.RoomNo = "No Room Alloted";
                }

                return Ok(studentToShow);
            }
            else if(roles.Contains("HallAdmin"))
            {
                HallDetails hallDetails = new HallDetails();
                int? HallId=await Task.Run(() => _profileRepository.GetHallId(email));
                if (email != null)
                {
                    hallDetails = await Task.Run(() => _profileRepository.GetHallDetails(HallId));
                }
                HallDetailsToShowDto hallDetailsToShow = _mapper.Map<HallDetailsToShowDto>(hallDetails);

                hallDetailsToShow.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(hallDetails.ImageData));
                //Console.WriteLine(hallDetails.Established);
                hallDetailsToShow.Established = DateOnly.ParseExact(hallDetails.Established.ToString("dd/MM/yyyy"), "dd/MM/yyyy");
                return Ok(hallDetailsToShow);
                
            }
            else
            {
                return BadRequest(new { message = "Invalid Request" });
            }


        }
    }
}
