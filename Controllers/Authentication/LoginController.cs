using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Data;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;


namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "Student,HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class LoginController : ControllerBase
    {
        //private readonly IMapper _mapper;
        private readonly AuthenticatioHelper _authenticatioHelper;
        private readonly ILoginRepository _loginRepository;
        private readonly ITokenBlacklistRepository _tokenBlacklistRepository;

        public LoginController(IConfiguration config,ILoginRepository loginRepository, ITokenBlacklistRepository tokenBlacklistRepository)
        {
            _authenticatioHelper = new AuthenticatioHelper(config);
            _loginRepository = loginRepository;
            _tokenBlacklistRepository = tokenBlacklistRepository;
            _tokenBlacklistRepository = tokenBlacklistRepository;
            //_mapper = new MapperConfiguration(cfg =>
            //{
            //}).CreateMapper();
        }

        //[Authorize("StudentPolicy")]
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginTry([FromBody] LoginDto user)
        {
            if(user == null)
            {
                return BadRequest(new { message = "Invalid Request" });
            }

            if (user.Role == "Student")
            {
                StudentAuthentication studentAuthentication = _loginRepository.GetSingleStudentAuthentication(user.Email);

                if (studentAuthentication == null)
                {
                    return BadRequest(new { message = "User doesn't exist" });
                }

                byte[] passwordHash = _authenticatioHelper.GetPasswordHash(user.Password, studentAuthentication.PasswordSalt);

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


                var roles = new List<string> { "Student" };

                string token = _authenticatioHelper.CreateToken(user.Email, roles);

                Console.WriteLine(token);


                return Ok(new { message = "Login Success", token });
            }
            else if (user.Role == "HallAdmin")
            {
                HallAdminAuthentication hallAdminAuthentication = _loginRepository.GetSingleHallAdminAuthentication(user.Email);
                if (hallAdminAuthentication == null)
                {
                    return BadRequest(new { message = "User doesn't exist" });
                }
                byte[] passwordHash = _authenticatioHelper.GetPasswordHash(user.Password, hallAdminAuthentication.PasswordSalt);
                if (hallAdminAuthentication.PasswordHash.Length != passwordHash.Length)
                {
                    return BadRequest(new { message = "Invalid Password" });
                }
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != hallAdminAuthentication.PasswordHash[i])
                    {
                        return BadRequest(new { message = "Invalid Password" });
                    }
                }

                var roles = new List<string> { "HallAdmin" };
                string token = _authenticatioHelper.CreateToken(user.Email, roles);
                Console.WriteLine(token);
                return Ok(new { message = "Login Success", token });
            }
            return BadRequest(new { message = "Functionality isn't Complete" });
        }




        //[Authorize("StudentPolicy")]
        [HttpGet("RefreshStudentToken")]
        public string RefreshToken()
        {
            string? email=User.FindFirst("userEmail")?.Value;
            if(email==null)
            {
                return "Invalid Request";
            }
            StudentAuthentication studentAuthentication = _loginRepository.GetSingleStudentAuthentication(email);
            return _authenticatioHelper.CreateToken(email, new List<string> { "Student" });
        }

        
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                _tokenBlacklistRepository.AddTokenToBlacklist(token);
            }
            return Ok(new { message = "Logout Success" });
        }
    }
}
