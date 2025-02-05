namespace Student_Hall_Management.Models
{
    public class AssignedDinningFee
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public int HallId{ get; set; }

        public AssignedDinningFee()
        {
            if (Month == null)
            {
                Month = "";
            }
        }
    }
}
