using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Repositories;
using Student_Hall_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Microsoft.Extensions.Logging.Abstractions;




namespace Student_Hall_Management.Controllers
{
    [Authorize("StudentPolicy")]
    [ApiController]
    [Route("/[controller]")]
    public class RoomController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly IPresentDateTime _presentDateTime;

        public RoomController(IRoomRepository roomRepository,IConfiguration config,IPresentDateTime presentDateTime)
        {
            _roomRepository = roomRepository;
            _presentDateTime = presentDateTime;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomsToShow, Room>();
                cfg.CreateMap<Room, RoomsToShow>();
            }).CreateMapper();
        }

        //[Authorize(Policy ="StudentPolicy")]
        [HttpGet("GetRoomData")]
        public ActionResult<IEnumerable<RoomsToShow>>  GetRoomsToShow()
        {
            
            string email = User.FindFirst("userEmail")?.Value;

            Student student = _roomRepository.GetSingleStudent(email);

            IEnumerable<Room> rooms = _roomRepository.GetAllRoomsOfHall(student.HallId);


            if (student.HallId == null)
            {
                return BadRequest("No hall found, Wait for Hall Admin to Assign a Hall");
            }

            
             if(rooms == null)
             {
                return BadRequest("No rooms found");
             }


            PendingRoomRequest? pendingRoomRequest = _roomRepository.GetPendingRoomRequest(student.Id, student.HallId);

            string? RoomRequested = null;
            if(pendingRoomRequest != null)
            {
                RoomRequested = pendingRoomRequest.RoomNo;
            }
               

            List<RoomsToShow> roomsToShow = new List<RoomsToShow>();


            foreach (Room room in rooms)
              {
                RoomsToShow roomsToShow1 = new RoomsToShow();
                _mapper.Map(room, roomsToShow1);
                IEnumerable<Student> studentInRoom = _roomRepository.GetStudentsInRoom(room.RoomNo);

                foreach (Student student1 in studentInRoom)
                {
                    student1.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(student1.ImageData));
                }
                roomsToShow1.Students = studentInRoom;
                roomsToShow1.Available = room.HasSeats - room.OccupiedSeats;
                roomsToShow1.IsRequested = false;
                if (RoomRequested != null)
                {
                    if (RoomRequested == room.RoomNo)
                    {
                        roomsToShow1.IsRequested = true;
                    }
                }
                if (student.RoomNo == room.RoomNo)
                {
                    roomsToShow1.IsAlloted = true;
                }
                else
                {
                    roomsToShow1.IsAlloted = false;
                }
                //roomsToShow1.IsAlloted = alloted;

                roomsToShow.Add(roomsToShow1);
            }
            roomsToShow = roomsToShow
                .OrderByDescending(r => r.Students.Any(s => s.Id == student.Id))
                .ThenByDescending(r => r.IsRequested)
                .ThenByDescending(r => r.OccupiedSeats)
                .ToList();

            if (roomsToShow == null)
            {
                return BadRequest("No rooms found");
            }




            return Ok(roomsToShow);
            
        }






        [HttpPost("Apply/{RoomNo}")]
        public IActionResult ApplyRoom(string RoomNo)
        {
            string email = User.FindFirst("userEmail")?.Value;
            Student student = _roomRepository.GetSingleStudent(email);
            if (student == null)
            {
                return BadRequest("You are not authorized to apply for room");
            }
            if(student.HallId == null)
            {
                return BadRequest("No Hall Assigned");
            }
            if(student.RoomNo != null)
            {
                return BadRequest("You have already been alloted a room");
            }
            PendingRoomRequest pendingRoomRequest=_roomRepository.GetPendingRoomRequest(student.Id,student.HallId);
            if (pendingRoomRequest != null)
            {
                return BadRequest("You have already applied for a room");
            }

            Room room = _roomRepository.GetRoomByRoomNo(RoomNo);

            if (room == null)
            {
                return BadRequest("Room not found");
            }

            if (room.OccupiedSeats >= room.HasSeats)
            {
                return BadRequest("Room is full");
            }

            PendingRoomRequest pendingRoomRequest1 = new PendingRoomRequest();
            pendingRoomRequest1.StudentId = student.Id;
            pendingRoomRequest1.HallId = student.HallId;
            pendingRoomRequest1.RoomNo = RoomNo;
            pendingRoomRequest1.RequestedAt = _presentDateTime.GetPresentDateTime();

            

            _roomRepository.AddEntity<PendingRoomRequest>(pendingRoomRequest1);



            Room rooms = _roomRepository.GetRoomByRoomNo(RoomNo);
            RoomsToShow roomsToShow = new RoomsToShow();
            _mapper.Map(rooms, roomsToShow);

            roomsToShow.IsAlloted = false;
            roomsToShow.IsRequested = true;

            roomsToShow.Available = rooms.HasSeats - rooms.OccupiedSeats;

            IEnumerable<Student> studentInRoom = _roomRepository.GetStudentsInRoom(RoomNo);

            roomsToShow.Students = studentInRoom;



            if (_roomRepository.SaveChanges())
            {
                return Ok(roomsToShow);
            }
            return BadRequest("Failed to apply for room");

        }


        [HttpDelete("CancelRequest/{roomNumber}")]
        public IActionResult CancelRequest(string roomNumber)
        {
            string email = User.FindFirst("userEmail")?.Value;

            Student student = _roomRepository.GetSingleStudent(email);

            if (student == null)
            {
                return BadRequest("Authorization Failed");
            }

            PendingRoomRequest  pendingRoomRequest=_roomRepository.GetPendingRoomRequest(student.Id,student.HallId);

            if(pendingRoomRequest == null)
            {
                return BadRequest("No request is available");
            }

            if(pendingRoomRequest.RoomNo!=roomNumber)
            {
                return BadRequest("Something Went Wrong");
            }

            _roomRepository.RemoveEntity<PendingRoomRequest>(pendingRoomRequest);
            Room rooms = _roomRepository.GetRoomByRoomNo(roomNumber);
            RoomsToShow roomsToShow = new RoomsToShow();
            _mapper.Map(rooms,roomsToShow);

            roomsToShow.IsAlloted = false;
            roomsToShow.IsRequested = false;

            roomsToShow.Available=rooms.HasSeats-rooms.OccupiedSeats;

            IEnumerable<Student> studentInRoom = _roomRepository.GetStudentsInRoom(roomNumber);

            roomsToShow.Students = studentInRoom;

            if (_roomRepository.SaveChanges())
            {
                return Ok(roomsToShow);
            }
            return BadRequest("Something Went Wrong");
        }

    }
}
