using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos.HallAdmin.Student;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using System.Security.Claims;
using Student_Hall_Management.Dtos.DSW;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "DSW")]
    [ApiController]
    [Route("/[controller]")]
    public class DSWStudentController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IStudentManagementRepository _studentManagementRepository;
        private readonly IPresentDateTime _presentDateTime;
        private readonly AdminStudentManagementHelper _adminStudentManagementHelper;
        private readonly DSWStudentRepository _dSWStudentRepository;

        private readonly ILogger<DSWStudentController> _logger;

        public DSWStudentController(IStudentManagementRepository studentManagementRepository, IPresentDateTime presentDateTime,IDSWStudentRepository dSWStudentRepository, ILogger<DSWStudentController> logger)
        {
            _studentManagementRepository = studentManagementRepository;
            _adminStudentManagementHelper = new AdminStudentManagementHelper(_studentManagementRepository);
            _dSWStudentRepository = (DSWStudentRepository)dSWStudentRepository;
            _presentDateTime = presentDateTime;
            _logger = logger;
            _mapper = new MapperConfiguration(cfg =>
            {

            }).CreateMapper();
            _logger = logger;
        }


        [HttpGet("GetStudentManagementPage/{hallId}")]
        public async Task<IActionResult> GetStudentManagementPage(int hallId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

           
            //int? hallId = await _studentManagementRepository.GetHallId(email);
            _logger.LogInformation("Fetching student management page for hallId: {HallId}", hallId);
            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }

            StudentManagementPageDto studentManagementPageDto = new StudentManagementPageDto();
            studentManagementPageDto = await _adminStudentManagementHelper.GetStudentManagementPage(hallId);
            //show studentMAnagementPageDto in log
            _logger.LogInformation("Student Management Page Data: {@StudentManagementPageDto}", studentManagementPageDto);
            //if(studentManagementPageDto == null) {
            //    return BadRequest("No student found");
            //}
            return Ok(studentManagementPageDto);

        }




        [HttpDelete("DeleteStudent/{studentId}/{hallId}")]
        public async Task<IActionResult> DeleteStudent(int studentId, int hallId)
        {
            bool IsDeleted = await _adminStudentManagementHelper.DeleteStudent(studentId);

            if (IsDeleted)
            {
                string email = User.FindFirst("userEmail")?.Value;
                //int? hallId = await _studentManagementRepository.GetHallId(email);
                StudentManagementPageDto adminStudentManagementPageDto = await _adminStudentManagementHelper.GetStudentManagementPage(hallId);
                return Ok(adminStudentManagementPageDto);
            }
            else
            {
                return BadRequest("Student not deleted");
            }

        }

        [HttpPost("AssignRoom/{studentId}/{roomNo}/{hallId}")]
        public async Task<IActionResult> AssignRoom(int studentId, string roomNo, int hallId)
        {
            Console.WriteLine(studentId.ToString() + roomNo);

            //int? hallId = await _studentManagementRepository.GetHallId(User.FindFirst("userEmail")?.Value);

            Room room = await _studentManagementRepository.GetRoomByRoomNoAndHallId(roomNo, hallId);

            Student student = await _studentManagementRepository.GetStudentById(studentId);

            if (room.OccupiedSeats < room.HasSeats)
            {
                student.RoomNo = roomNo;
                room.OccupiedSeats += 1;
                _studentManagementRepository.UpdateEntity<Room>(room);
                _studentManagementRepository.UpdateEntity<Student>(student);
                if (await _studentManagementRepository.SaveChangesAsync())
                {
                    return Ok(await _adminStudentManagementHelper.GetStudentManagementPage(hallId));
                }
                else
                {
                    return BadRequest("Room not assigned");
                }
            }
            else
            {
                return BadRequest("Room not assigned");
            }

        }

        [HttpGet("GetAvailableRooms/{hallId}")]
        public async Task<ActionResult<IEnumerable<AvailableRoomsDto>>> GetAvailableRooms(int hallId)
        {
            //int? hallId = await _studentManagementRepository.GetHallId(User.FindFirst("userEmail")?.Value);
            IEnumerable<Room> rooms = await _studentManagementRepository.GetAvailableRooms(hallId);
            List<AvailableRoomsDto> availableRoomsDto = new List<AvailableRoomsDto>();
            foreach (var room in rooms)
            {
                availableRoomsDto.Add(new AvailableRoomsDto
                {
                    RoomNo = room.RoomNo,
                    TotalSeats = room.HasSeats,
                    AvailableSeats = room.HasSeats - room.OccupiedSeats
                });
            }
            availableRoomsDto.OrderBy(r => r.AvailableSeats);
            return Ok(availableRoomsDto);
        }


        [HttpGet("Halls")]
        public async Task<IActionResult> GetHalls()
        {
            List<HallListDto> hallListDtos = new List<HallListDto>();
            List<HallDetails> halls = await _studentManagementRepository.GetHalls();

            foreach (var hall in halls)
            {
                HallListDto hallListDto = new HallListDto();
                hallListDto.HallId = hall.HallId;
                hallListDto.HallName = hall.HallName;
                hallListDtos.Add(hallListDto);

            }
            AllHallsLiat allHallsLiat = new AllHallsLiat();
            allHallsLiat.Halls = hallListDtos;
            Console.WriteLine(hallListDtos.Count());
            return Ok(allHallsLiat);
        }


        [HttpPut("StudentToHall/{studentId}/{hallId}")]
        public async Task<IActionResult> StudentToHall(int studentId, int hallId)
        {
            Student student = await _studentManagementRepository.GetStudentById(studentId);
            student.HallId = hallId;
            _studentManagementRepository.UpdateEntity<Student>(student);
            if (await _studentManagementRepository.SaveChangesAsync())
            {
                return Ok(await _adminStudentManagementHelper.GetStudentManagementPage(hallId));
            }
            else
            {
                return BadRequest("Student not assigned to hall");
            }

        }



        [HttpGet("StudentsToAddSuggestion")]
        public async Task<IActionResult> StudentsToAddSuggestion()
        {
            List<Student> students = await _dSWStudentRepository.GetNotAllotedStudents();
            SuggestionsDto  suggestions1 = new SuggestionsDto();
            foreach (Student student in students)
            {
                StudentSuggestionDto studentToAddSuggestionDto = new StudentSuggestionDto();
                studentToAddSuggestionDto.StudentId = student.Id;
                studentToAddSuggestionDto.Name = student.Name;

                suggestions1.Suggestions.Add(studentToAddSuggestionDto);
            }
            return Ok(suggestions1);
        }

    }
}
