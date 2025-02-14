using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
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
        public async Task<ActionResult<StudentManagementPageDto>> GetStudentManagementPage()
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

            StudentManagementPageDto studentManagementPageDto = new StudentManagementPageDto();
            studentManagementPageDto = await _adminStudentManagementHelper.GetStudentManagementPage(hallId.Value);

            return Ok(studentManagementPageDto);

        }




        [HttpDelete("DeleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            bool IsDeleted=await _adminStudentManagementHelper.DeleteStudent(studentId);

            if (IsDeleted)
            {
                string email = User.FindFirst("userEmail")?.Value;
                int? hallId = await _studentManagementRepository.GetHallId(email);
                StudentManagementPageDto adminStudentManagementPageDto =await _adminStudentManagementHelper.GetStudentManagementPage(hallId.Value);
                return Ok(adminStudentManagementPageDto);
            }
            else
            {
                return BadRequest("Student not deleted");
            }

        }

        [HttpPost("AssignRoom/{studentId}/{roomNo}")]
        public async Task<IActionResult> AssignRoom(int studentId, string roomNo)
        {
            Console.WriteLine(studentId.ToString()+roomNo);

            int? hallId = await _studentManagementRepository.GetHallId(User.FindFirst("userEmail")?.Value);

            Room room = await _studentManagementRepository.GetRoomByRoomNoAndHallId(roomNo, hallId.Value);

            Student student = await _studentManagementRepository.GetStudentById(studentId);

            if (room.OccupiedSeats < room.HasSeats)
            {
                student.RoomNo = roomNo;
                room.OccupiedSeats += 1;
                _studentManagementRepository.UpdateEntity<Room>(room);
                _studentManagementRepository.UpdateEntity<Student>(student);
                if(await _studentManagementRepository.SaveChangesAsync())
                {
                    return Ok(await _adminStudentManagementHelper.GetStudentManagementPage(hallId.Value));
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

        [HttpGet("GetAvailableRooms")]
        public async Task<ActionResult<IEnumerable<AvailableRoomsDto>>> GetAvailableRooms()
        {
            int? hallId = await _studentManagementRepository.GetHallId(User.FindFirst("userEmail")?.Value);
            IEnumerable<Room> rooms = await _studentManagementRepository.GetAvailableRooms(hallId.Value);
            List<AvailableRoomsDto> availableRoomsDto = new List<AvailableRoomsDto>();
            foreach (var room in rooms)
            {
                availableRoomsDto.Add(new AvailableRoomsDto
                {
                    RoomNo = room.RoomNo,
                    TotalSeats = room.HasSeats,
                    AvailableSeats = room.HasSeats- room.OccupiedSeats
                });
            }
            availableRoomsDto.OrderBy(r => r.AvailableSeats);
            return Ok(availableRoomsDto);
        }


        

      
    }
}
