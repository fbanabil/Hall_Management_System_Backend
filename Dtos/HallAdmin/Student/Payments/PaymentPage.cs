namespace Student_Hall_Management.Dtos
{ 
    public class PaymentPage
    {
        public IEnumerable<HallPaymentsToShowDto> HallPayments { get; set; }
        public IEnumerable<DinningPaymentsToShowDto> DinningPayments { get; set; }

        public PaymentPage()
        {
            if (HallPayments == null)
            {
                HallPayments = new List<HallPaymentsToShowDto>();
            }
            if (DinningPayments == null)
            {
                DinningPayments = new List<DinningPaymentsToShowDto>();
            }
        }
    }
}
