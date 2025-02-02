namespace Student_Hall_Management.Dtos.HallAdmin.Room
{
    public class EditRoomDetailsDto
    {
        public string RoomNo { get; set; }
        public int HasSeats { get; set; }
        public string RoomCondition { get; set; }
        public string prevRoomNo { get; set; }

        public EditRoomDetailsDto()
        {
            if (RoomNo == null)
            {
                RoomNo = "";
            }
            if (RoomCondition == null)
            {
                RoomCondition = "";
            }
        }
    }
}
