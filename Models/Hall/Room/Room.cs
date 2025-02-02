namespace Student_Hall_Management.Models
{
    public partial class Room
    {
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public string RoomCondition { get; set; }
        public int HasSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public int HallId { get; set; }

        public HallDetails Hall { get; set; } //Navigation property
        public ICollection<Student> Students { get; set; }

        public Room()
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
