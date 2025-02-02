using Student_Hall_Management.Models;

namespace Student_Hall_Management.Dtos
{
    public class RoomsToShow
    {
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public string RoomCondition { get; set; }
        public int HasSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public int Available { get; set; }
        public bool IsRequested { get; set; }
        public bool IsAlloted { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public RoomsToShow()
        {
            Students = new List<Student>();
            if (RoomNo == null)
            {
                RoomNo = "";
            }
            if (RoomType == null)
            {
                RoomType = "";
            }
            if (RoomStatus == null)
            {
                RoomStatus = "";
            }
            if (RoomCondition == null)
            {
                RoomCondition = "";
            }

        }
    }

}
