namespace Student_Hall_Management.Dtos
{
    public class DinningPaymentsToShowDto
    {
        public int DinningFeePaymentId { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }

        public string PaymentDate { get; set; }

        public DinningPaymentsToShowDto()
        {

            if (Month == null)
            {
                Month = "";
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
