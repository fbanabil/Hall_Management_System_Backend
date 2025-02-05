namespace Student_Hall_Management.Dtos
{
    public class HallPaymentsToShowDto
    {
        public int HallFeePaymentId { get; set; }
        public string LevelAndTerm { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }

        public string PaymentDate { get; set; }


        public HallPaymentsToShowDto()
        {
            if (LevelAndTerm == null)
            {
                LevelAndTerm = "";
            }
            if (Status == null)
            {
                Status = "";
            }
            if (PaymentDate == null)
            {
                PaymentDate = "";
            }
        }

    }
}
