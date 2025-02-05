namespace Student_Hall_Management.Dtos
{
    public class HallFeePaymentToAdd
    {
        public int Batch { get; set; }
        public string LevelAndTerm { get; set; }
        public int TotalAmount { get; set; }

        public HallFeePaymentToAdd()
        {
            if (LevelAndTerm == null)
            {
                LevelAndTerm = "";
            }
        }
    }
}
