namespace Student_Hall_Management.Dtos
{
    public class AssignedDinningFeeToShow
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public AssignedDinningFeeToShow()
        {
            if (Month == null)
            {
                Month = "";
            }
        }
    }
}
