using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using Student_Hall_Management.Dtos;
using System.Security.Claims;
using System.Data;
using Student_Hall_Management.Dtos.HallAdmin;
using Microsoft.Identity.Client;
using Student_Hall_Management.Helpers;
using System.Security.Cryptography;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "Student,HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly AuthenticatioHelper _authenticationHelper;

        public ProfileController(IProfileRepository profileRepository,IConfiguration config)
        {
            _profileRepository = profileRepository;
            _authenticationHelper = new AuthenticatioHelper(config);
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

                if (student.HallId == null)
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
                hallDetailsToShow.TotalSeats = _profileRepository.TotalSeats(HallId);
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

        [HttpPut("ChengePassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto == null)
            {
                return BadRequest(new { message = "Invalid Request" });
            }

            string email = User.FindFirst("userEmail")?.Value;

            if (email == null)
            {
                return BadRequest(new { message = "Invalid Request" });
            }

            StudentAuthentication studentAuthentication = _profileRepository.GetSingleStudentAuthentication(email);


            byte[] passwordHash = _authenticationHelper.GetPasswordHash(changePasswordDto.Password, studentAuthentication.PasswordSalt);

            if (studentAuthentication.PasswordHash.Length != passwordHash.Length)
            {
                return BadRequest(new { message = "Invalid Password" });
            }

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != studentAuthentication.PasswordHash[i])
                {
                    return BadRequest(new { message = "Invalid Password" });
                }
            }
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }
            Console.Write(changePasswordDto.Password);
            byte[] passwordHash1 = _authenticationHelper.GetPasswordHash(changePasswordDto.NewPassword, passwordSalt);

            studentAuthentication.PasswordHash = passwordHash1;
            studentAuthentication.PasswordSalt = passwordSalt;

            _profileRepository.UpdateEntity(studentAuthentication);
            if (_profileRepository.SaveChanges())
            {
                return Ok(new { message = "Password Changed Successfully" });
            }
            else
            {
                return BadRequest(new { message = "Password Changing Failed" });

            }

        }


    }
}
