namespace Student_Hall_Management.Dtos
{
    public class AssignedHallFeeToShow
    {
        public int Batch { get; set; }
        public string LevelAndTerm { get; set; }
        public int TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public AssignedHallFeeToShow()
        {
            if(LevelAndTerm == null)
            {
                LevelAndTerm = "";
            }
        }
    }
}
