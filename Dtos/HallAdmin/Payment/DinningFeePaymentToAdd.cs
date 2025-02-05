namespace Student_Hall_Management.Dtos
{
    public class DinningFeePaymentToAdd
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int TotalAmount { get; set; }

        public DinningFeePaymentToAdd()
        {
            if (Month == null)
            {
                Month = "";
            }
        }
    }
}
