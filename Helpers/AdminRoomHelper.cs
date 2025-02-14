using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class AdminRoomHelper
    {
        private readonly IMapper _mapper;
        private readonly IAdminRoomRepository _adminRoomRepository;

        public AdminRoomHelper(IAdminRoomRepository adminRoomRepository)
        {
            _adminRoomRepository = adminRoomRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AdminRoomToShowDto, Room>();
                cfg.CreateMap<Room, AdminRoomToShowDto>();
                //cfg.CreateMap<IEnumerable<Room>, List<AdminRoomToShowDto>>();
                cfg.CreateMap<AdminRoomToShowDto, Room>();
                cfg.CreateMap<Room, AdminRoomToShowDto>();
                //cfg.CreateMap<Room, AdminRoomToShowDto>();
                //cfg.CreateMap<AdminRoomPageDto, Room>();
                cfg.CreateMap<Student, StudentInRoomDto>(
                    ).ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


            }
            ).CreateMapper();
        }



            public async Task<AdminRoomPageDto> GetAdminHomePage(int? hallId)
            {
            Tuple<int, int, int, int, int> OverallRoom = await Task.Run(() =>_adminRoomRepository.GetOverallRoomDetails(hallId));

            AdminRoomPageDto adminRoomPageDto = new AdminRoomPageDto();

            adminRoomPageDto.TotalRooms = OverallRoom.Item1;
            adminRoomPageDto.TotalSeats = OverallRoom.Item2;
            adminRoomPageDto.AvailableRooms = OverallRoom.Item3;
            adminRoomPageDto.AvailableSeats = OverallRoom.Item4;
            adminRoomPageDto.RoomConditionNotOk = OverallRoom.Item5;
            //Console.WriteLine(adminRoomPageDto.RoomConditionNotOk);
            IEnumerable<Room> adminRoom = new List<Room>();
            adminRoom = await Task.Run(() => _adminRoomRepository.GetRooms(hallId));
            //Console.WriteLine(adminRoom.Count());
            List<AdminRoomToShowDto> adminRoomToShowDtos = new List<AdminRoomToShowDto>();

            foreach (var room in adminRoom)
            {
                AdminRoomToShowDto adminRoomToShowDto = new AdminRoomToShowDto();
                _mapper.Map(room, adminRoomToShowDto);
                if(room.OccupiedSeats > 0 && room.OccupiedSeats < room.HasSeats)
                {
                    adminRoomToShowDto.RoomStatus = "Partial Occupied";
                }
                else if (room.OccupiedSeats == 0)
                {
                    adminRoomToShowDto.RoomCondition = "Un-Occupied";
                }
                else
                {
                    adminRoomToShowDto.RoomCondition = "Occupied";
                }
                adminRoomToShowDtos.Add(adminRoomToShowDto);

            }

            //sort adminRoomToShowDtos acording to occupied seats
            adminRoomToShowDtos = adminRoomToShowDtos.OrderByDescending(x => x.OccupiedSeats).ThenBy(p=>p.RoomNo).ToList();
            //adminRoomToShowDtos.Reverse();

            adminRoomPageDto.AdminRoomToShow = adminRoomToShowDtos;


            adminRoomPageDto.PendingRoomRequests = await Task.Run(() => _adminRoomRepository.GetPendingRoomRequests(hallId));

            return adminRoomPageDto;
        }



        

    }
}
