using Student_Hall_Management.Models;

namespace Student_Hall_Management.Dtos
{
    public class AdminRoomPageDto
    {
        public int TotalRooms { get; set; }
        public int TotalSeats { get; set; }

        public int AvailableRooms { get; set; }
        public int AvailableSeats { get; set; }

        public int RoomConditionNotOk { get; set; }


        public IEnumerable<AdminRoomToShowDto> AdminRoomToShow { get; set; }
        public IEnumerable<PendingRoomRequest> PendingRoomRequests { get; set; }

        public AdminRoomPageDto() 
        {
          
            AdminRoomToShow = new List<AdminRoomToShowDto>();
        }
    }
}
