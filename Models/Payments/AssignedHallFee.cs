namespace Student_Hall_Management.Models
{
    public class AssignedHallFee
    {
        public int HallId { get; set; }
        public int Batch { get; set; }
        public string LevelAndTerm { get; set; }
        public int TotalAmount { get; set; }
        public DateTime Date { get; set; }


        public AssignedHallFee()
        {
            if (LevelAndTerm == null)
            {
                LevelAndTerm = "";
            }
        }

    }
}
