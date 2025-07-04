using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Dtos.HallAdmin.Room;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using System.Security.Claims;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = ("HallAdmin"))]
    [ApiController]
    [Route("/[controller]")]
    public class AdminRoomController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminRoomRepository _adminRoomRepository;
        private readonly AdminRoomHelper _adminRoomHelper;
        private readonly ILogger<AdminRoomController> _logger;

        public AdminRoomController(IAdminRoomRepository adminRoomRepository, ILogger<AdminRoomController> logger)
        {
            _logger = logger;
            _adminRoomRepository = adminRoomRepository;
            _adminRoomHelper = new AdminRoomHelper(adminRoomRepository);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminRoomToShowDto, Room>();
                cfg.CreateMap<Room, AdminRoomToShowDto>();
                //cfg.CreateMap<IEnumerable<Room>, List<AdminRoomToShowDto>>();
                cfg.CreateMap<AdminRoomToShowDto, Room>();
                cfg.CreateMap<Room, AdminRoomToShowDto>();
                cfg.CreateMap<Student, StudentInRoomDto>(
                    ).ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

                cfg.CreateMap<Student, StudentDetailsDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageData)
                );

            }
            ).CreateMapper();
            _logger = logger;
        }


        [HttpGet("Room")]
        public async Task<IActionResult> GetRooms()
        {
            string email = User.FindFirst("userEmail")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains("HallAdmin"))
            {
                return Unauthorized("Login as Admin");
            }

            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));

            AdminRoomPageDto adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);

            return Ok(adminRoomPageDto);

        }



        [HttpPost("GetStudents/{roomNo}")]
        public IActionResult GetStudents(string roomNo)
        {
            IEnumerable<Student> students = new List<Student>();
            students = _adminRoomRepository.GetStudentByRoomNo(roomNo);

            List<StudentInRoomDto> studentInRoomDtos = new List<StudentInRoomDto>();

            foreach (var student in students)
            {
                StudentInRoomDto studentInRoomDto = new StudentInRoomDto();
                _mapper.Map(student, studentInRoomDto);
                studentInRoomDtos.Add(studentInRoomDto);
            }
            return Ok(studentInRoomDtos);
        }

        [HttpPost("AddRoom")]
        public async Task<IActionResult> AddRoom(RoomToAddDto roomToAddDto)
        {
            if (roomToAddDto == null)
            {
                return BadRequest("Null request made");
            }
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));

            Room? exist = await Task.Run(() => _adminRoomRepository.GetRoomByRoomNoAndHallId(roomToAddDto.RoomNo, hallId.Value));

            if(exist != null)
            {
                return BadRequest("Room already exists");
            }
            Room room = new Room();

            room.RoomNo = roomToAddDto.RoomNo;
            room.RoomCondition = roomToAddDto.RoomCondition;
            room.HasSeats = roomToAddDto.HasSeats;

            room.RoomStatus = "UnOccupied";
            room.RoomType = "Normal";
            room.OccupiedSeats = 0;


            
            room.HallId = hallId.Value;

            //Console.WriteLine(room.RoomNo);

            _adminRoomRepository.AddEntity<Room>(room);
            if (_adminRoomRepository.SaveChanges())
            {
                AdminRoomPageDto adminRoomPageDto = new AdminRoomPageDto();
                adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
                return Ok(adminRoomPageDto);
            }
            else
            {
                return BadRequest("Failed to add room");
            }
        }



        [HttpPut("UpdateCondition/{roomNo}")]
        public async Task<IActionResult> UpdateRoomCondition(string roomNo)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));

            Room room = await Task.Run(() => _adminRoomRepository.GetRoomByRoomNo(roomNo, hallId.Value));
            if (room == null)
            {
                return NotFound("Room not found");
            }
            if (room.RoomCondition == "Good")
            {
                room.RoomCondition = "Need Maintenence";
            }
            else
            {
                room.RoomCondition = "Good";
            }
            Console.WriteLine(roomNo);
            _adminRoomRepository.UpdateEntity(room);
            await Task.Run(() => _adminRoomRepository.SaveChangesAsync());

            AdminRoomPageDto adminRoomPageDto = new AdminRoomPageDto();


            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }
            if (hallId.HasValue)
            {
                adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
            }
            return Ok(adminRoomPageDto);

            //return Ok("Room Condition Updated");

        }


        [HttpPost("GetStudent/{studentId}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            Student student = await Task.Run(() => _adminRoomRepository.GetStudent(studentId));
            StudentDetailsDto studentDetailsDto = new StudentDetailsDto();
            _mapper.Map(student, studentDetailsDto);

            //read from folder
            studentDetailsDto.Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(studentDetailsDto.Image));
            return Ok(studentDetailsDto);
        }



        [HttpDelete("DeleteRoom/{roomNo}")]
        public async Task<IActionResult> DeleteRoom(string roomNo)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));
            Room room = await Task.Run(() => _adminRoomRepository.GetRoomByRoomNo(roomNo, hallId.Value));
            if (room == null)
            {
                return BadRequest("Room not found");
            }

            await Task.Run(()=>_adminRoomRepository.RemoveRoomNo(room.RoomNo, hallId.Value));

            _adminRoomRepository.RemoveEntity(room);
            await Task.Run(() => _adminRoomRepository.SaveChangesAsync());
            AdminRoomPageDto adminRoomPageDto = new AdminRoomPageDto();
            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }
            if (hallId.HasValue)
            {
                adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
            }
            return Ok(adminRoomPageDto);

        }


        [HttpPut("UpdateSingleRoom/{roomNo}")]
        public async Task<IActionResult> UpdateRoom(EditRoomDetailsDto editRoomDetails, string roomNo)
        {
            if (editRoomDetails == null)
            {
                return BadRequest("Null request made");
            }
            Console.WriteLine(roomNo);
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));
            Room room = await Task.Run(() => _adminRoomRepository.GetRoomByRoomNo(roomNo, hallId.Value));
            _logger.LogInformation("Room fetched for update: " + roomNo + " " + hallId.Value);
            _logger.LogInformation("Room details: " + room.RoomNo + " " + room.HallId + " " + room.OccupiedSeats + " " + room.HasSeats);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            //room.RoomNo = editRoomDetails.RoomNo;
            if (editRoomDetails.HasSeats < room.OccupiedSeats)
            {
                return BadRequest("Occupied seats are more than the seats you are trying to update");
            }

            room.RoomCondition = editRoomDetails.RoomCondition;
            room.HasSeats = editRoomDetails.HasSeats;
            _adminRoomRepository.UpdateEntity(room);
            await Task.Run(() => _adminRoomRepository.SaveChangesAsync());
            AdminRoomPageDto adminRoomPageDto = new AdminRoomPageDto();
            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }
            if (hallId.HasValue)
            {
                adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
            }
            return Ok(adminRoomPageDto);
        }


        [HttpPost("AcceptRequest/{studentId}")]
        public async Task<IActionResult> AcceptRequest(int studentId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));
            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }
            if (hallId.HasValue)
            {
                Student student = await Task.Run(() => _adminRoomRepository.GetStudent(studentId));
                if (student == null)
                {
                    return BadRequest("Student not found");
                }
                

                PendingRoomRequest pendingRoomRequest = await Task.Run(() => _adminRoomRepository.GetPendingRoomRequest(studentId, hallId.Value));

                Room room = await Task.Run(() => _adminRoomRepository.GetRoomByRoomNo(pendingRoomRequest.RoomNo, hallId.Value));

                if (room == null)
                {
                    return BadRequest("Room not found");
                }
                if (room.OccupiedSeats == room.HasSeats)
                {
                    return BadRequest("Room is full");
                }

                room.OccupiedSeats += 1;
                if(room.OccupiedSeats == room.HasSeats)
                {
                    room.RoomStatus = "Occupied";
                }
                else {
                    room.RoomStatus = "UnOccupied";
                }

                _logger.LogInformation("Updated room : " + room.RoomNo +" "+ room.HallId);

                _adminRoomRepository.UpdateEntity(room);


                _logger.LogInformation("Updated room : " + room.RoomNo + " " + room.HallId);


                student.RoomNo = pendingRoomRequest.RoomNo;
                student.HallId = hallId.Value;
                _adminRoomRepository.UpdateEntity(student);
                _adminRoomRepository.SaveChanges();
                _adminRoomRepository.RemoveEntity(pendingRoomRequest);
                await Task.Run(() => _adminRoomRepository.SaveChangesAsync());
                AdminRoomPageDto adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
                return Ok(adminRoomPageDto);

            }
            return BadRequest("Hall Id not found");

        }




        [HttpPost("RejectRequest/{studentId}")]
        public async Task<IActionResult> RejectRequest(int studentId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? hallId = await Task.Run(() => _adminRoomRepository.GetHallId(email));
            if (hallId == null)
            {
                return BadRequest("Hall Id not found");
            }
            if (hallId.HasValue)
            {
                PendingRoomRequest pendingRoomRequest = await Task.Run(() => _adminRoomRepository.GetPendingRoomRequest(studentId, hallId.Value));
                if (pendingRoomRequest == null)
                {
                    return BadRequest("Request not found");
                }
                _adminRoomRepository.RemoveEntity(pendingRoomRequest);
                await Task.Run(() => _adminRoomRepository.SaveChangesAsync());
                AdminRoomPageDto adminRoomPageDto = await _adminRoomHelper.GetAdminHomePage(hallId);
                return Ok(adminRoomPageDto);
            }
            return BadRequest("Hall Id not found");
        }

    }
}
