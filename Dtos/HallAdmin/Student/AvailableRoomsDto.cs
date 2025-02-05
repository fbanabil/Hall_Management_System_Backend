namespace Student_Hall_Management.Dtos
{
    public class AvailableRoomsDto
    {
        public string RoomNo { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }


        public AvailableRoomsDto()
        {
            if (RoomNo == null)
            {
                RoomNo = "";
            }
        }
    }
}
