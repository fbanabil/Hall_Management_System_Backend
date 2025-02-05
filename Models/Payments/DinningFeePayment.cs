namespace Student_Hall_Management.Models
{
    public class DinningFeePayment
    {
        public int DinningFeePaymentId { get; set; }
        public int StudentId { get; set; }
        public int HallId { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }

        public DinningFeePayment()
        {
            if (PaymentMethod == null)
            {
                PaymentMethod = "";
            }
            if (PaymentStatus == null)
            {
                PaymentStatus = "";
            }
            if (Month == null)
            {
                Month = "";
            }
        }
    }
}
