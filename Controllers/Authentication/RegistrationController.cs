using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Student_Hall_Management.Repositories;
using System.Security.Cryptography;
using Student_Hall_Management.Dto;

namespace Student_Hall_Management.Controllers
{
    [Authorize()]
    [ApiController]
    [Route("/[controller]")]
    public class RegistrationController : ControllerBase
    {

        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMapper _mapper;
        private readonly PresentDateTime _presentDateTime;
        private readonly EmailService _emailService;
        private readonly AuthenticatioHelper _authenticatioHelper;

        public RegistrationController(IConfiguration config, IRegistrationRepository registrationRepository, EmailService emailService)
        {
            _registrationRepository = registrationRepository;
            _presentDateTime = new PresentDateTime(config);
            _emailService = emailService;
            _authenticatioHelper = new AuthenticatioHelper(config);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StudentToAddDto, Student>();
                cfg.CreateMap<StudentVerificationDto, Student>();
                cfg.CreateMap<Student, StudentVerificationDto>();
            }).CreateMapper();
        }


        [AllowAnonymous]
        [HttpPost("Registration")]
        public IActionResult RegistrationTry([FromBody] StudentToAddDto student)
        {
            Student exist = _registrationRepository.GetSingleStudent(student.Email);

            if (exist != null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            if (student.Password != student.ConfirmPassword)
            {
                return BadRequest(new { message = "Password and Confirm Password do not match" });
            }

            string email = "u" + student.Id.ToString() + "@student.cuet.ac.bd";

            if (student.Email != email)
            {
                return BadRequest(new { message = "Id and Email doesn't match" });
            }

            if (student.Password.Length < 8)
            {
                return BadRequest(new { message = "Password must be atleast 8 characters long" });
            }


            StudentPendingRequest request = _registrationRepository.PendingRequest(student.Email);
            //Console.WriteLine(_presentDateTime.GetPresentDateTime());
            if (request != null)
            {
                DateTime currentTime = _presentDateTime.GetPresentDateTime();
                TimeSpan timeDifference = currentTime - request.Sent;
                double hoursDifference = timeDifference.TotalHours;

                if (hoursDifference < 1)
                {
                    return Ok(new { message = "Please use the verification code already sent before" });
                }
                else
                {
                    _registrationRepository.RemoveEntity<StudentPendingRequest>(request);
                    if (!_registrationRepository.SaveChanges())
                    {
                        return BadRequest(new { message = "Failed to send verification code. Please try again later." });
                    }
                }
            }

            Random random = new Random();
            int verificationCode = random.Next(100000, 999999);

            StudentPendingRequest newRequest = new StudentPendingRequest();
            newRequest.Email = student.Email;
            newRequest.Code = verificationCode.ToString();
            newRequest.Sent = _presentDateTime.GetPresentDateTime();


            _registrationRepository.AddEntity<StudentPendingRequest>(newRequest);

            if (!_registrationRepository.SaveChanges())
            {
                return BadRequest(new { message = "Failed to send verification code. Please try again later." });
            }

            try
            {
                Console.WriteLine("Your 6 digit verification code is: " + verificationCode);
                //_emailService.SendEmail(student.Email, "Verification Code", "Your 6 digit verification code is: " + verificationCode);
            }
            catch (Exception ex)
            {
                _registrationRepository.RemoveEntity<StudentPendingRequest>(newRequest);
                return BadRequest(new { message = "Failed to send verification code. Please try again later." });
            }

            return Ok(new { message = "6 DigitVerification Code in your email:" });
        }







        [AllowAnonymous]
        [HttpPost("Verify")]
        public IActionResult Verify([FromBody] StudentVerificationDto student)
        {

            StudentPendingRequest request = _registrationRepository.PendingRequest(student.Email);
            if (request == null)
            {
                return BadRequest(new { message = "Follow Appropiate Procedure" });
            }

            if (student.VerificationCode != request.Code)
            {
                return BadRequest(new { message = "Verification Code does not match" });
            }


            Student studentToAdd = new Student();


            //Console.WriteLine(studentToAdd.ImageData);


            string base64Data = student.ImageData;

            if (base64Data.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                int commaIndex = base64Data.IndexOf(',');
                if (commaIndex > 0)
                {
                    base64Data = base64Data.Substring(commaIndex + 1);
                }
            }
            byte[] imageBytes = Convert.FromBase64String(base64Data);



            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", student.Email.Substring(1, 2));

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = student.Id.ToString() + ".jpg";

            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                System.IO.File.WriteAllBytes(filePath, imageBytes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something Went Wrong" });
            }

            _mapper.Map(student, studentToAdd);
            studentToAdd.ImageData = filePath;
            studentToAdd.Role = "Student";
            studentToAdd.RoomNo = null;
            studentToAdd.HallId = null;
            studentToAdd.IsActive = false;
            studentToAdd.Batch = Convert.ToInt32(student.Email.Substring(1, 2));


            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }

            byte[] passwordHash = _authenticatioHelper.GetPasswordHash(student.Password, passwordSalt);

            StudentAuthentication studentAuthentication = new StudentAuthentication();
            studentAuthentication.Email = studentToAdd.Email;
            studentAuthentication.PasswordHash = passwordHash;
            studentAuthentication.PasswordSalt = passwordSalt;

            //Console.WriteLine(studentToAdd.Name);
            //Console.WriteLine(studentToAdd.Email);
            //Console.WriteLine(studentToAdd.Department);
            //Console.WriteLine(studentToAdd.ImageData);
            //Console.WriteLine(studentToAdd.Role);



            _registrationRepository.AddStudentAuthentication(studentAuthentication);
            _registrationRepository.AddEntity<Student>(studentToAdd);
            _registrationRepository.RemoveEntity<StudentPendingRequest>(request);

            if (_registrationRepository.SaveChanges())
            {
                return Ok(new { message = "User added successfully" });
            }

            return BadRequest(new { message = "Failed to add user" });
        }






        [AllowAnonymous]
        [HttpPost("AddAdmin")]
        public IActionResult AddAdmin([FromBody] HallAdminToAdd hallAdmin)
        {
            string email = _registrationRepository.GetHallAdmin(hallAdmin.Email);

            if (email != null)
            {
                return BadRequest("Admin Already Exists");
            }

            if (hallAdmin.Password != hallAdmin.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match");
            }

            if (hallAdmin.Password.Length < 8)
            {
                return BadRequest("Password must be atleast 8 characters long");
            }

            HallAdmin hallAdminDb = new HallAdmin();
            HallAdminAuthentication hallAdminAuthentication = new HallAdminAuthentication();

            hallAdminAuthentication.Email = hallAdmin.Email;


            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }

            byte[] passwordHash = _authenticatioHelper.GetPasswordHash(hallAdmin.Password, passwordSalt);

            hallAdminAuthentication.PasswordHash = passwordHash;
            hallAdminAuthentication.PasswordSalt = passwordSalt;

            hallAdminDb.HallId = hallAdmin.HallId;
            hallAdminDb.Email = hallAdmin.Email;
            _registrationRepository.AddEntity<HallAdminAuthentication>(hallAdminAuthentication);
            _registrationRepository.AddEntity<HallAdmin>(hallAdminDb);

            if (_registrationRepository.SaveChanges())
            {
                return Ok(new { message = "Admin added successfully" });
            }
            return BadRequest("Something Went Wrong");

        }


        [AllowAnonymous]
        [HttpPost("AddDSW")]
        public IActionResult AddDSW([FromBody] DSWToAddDto dsw)
        {
            DSW dswDb = new DSW();
            dswDb.Email = dsw.Email;


            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(passwordSalt);
            }

            byte[] passwordHash = _authenticatioHelper.GetPasswordHash(dsw.Password, passwordSalt);
            dswDb.PasswordHash = passwordHash; dswDb.PasswordSalt = passwordSalt;

            _registrationRepository.AddEntity<DSW>(dswDb);
            _registrationRepository.SaveChanges();
            return Ok(
                new
                {
                    message = "DSW added successfully"
                }
            );

        }
    }
}
