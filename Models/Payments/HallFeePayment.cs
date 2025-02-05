namespace Student_Hall_Management.Models
{
    public class HallFeePayment
    {
        public int HallFeePaymentId { get;set; }
        public int StudentId { get; set; }
        public int HallId { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string LevelAndTerm { get; set; }

        public HallFeePayment()
        {
            if (PaymentMethod == null)
            {
                PaymentMethod = "";
            }
            if (PaymentStatus == null)
            {
                PaymentStatus = "";
            }
            if (LevelAndTerm == null)
            {
                LevelAndTerm = "";
            }
        }
    }
}
