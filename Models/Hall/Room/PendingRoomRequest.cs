namespace Student_Hall_Management.Models
{
    public partial class PendingRoomRequest
    {
        public string RoomNo { get; set; }
        public int StudentId { get; set; }
        public int? HallId { get; set; }
        public DateTime RequestedAt { get; set; }

        public PendingRoomRequest()
        {
            if (RoomNo == null)
            {
                RoomNo = "";
            }
        }   

    }
}
