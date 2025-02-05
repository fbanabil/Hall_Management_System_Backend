using Student_Hall_Management.Models;

namespace Student_Hall_Management.Dtos
{
    public class PaymentPageDto
    {
        public int TotalAmountToGet { get; set; }
        public int TotalHallFeeAmount { get; set; }
        public int TotalDinningFeeAmount { get; set; }
        public Double PercentageOfPayment { get; set; }
        public List<AssignedDinningFeeToShow> AssignedDinningFeeToShow { get; set; }
        public List<AssignedHallFeeToShow> AssignedHallFeeToShow { get; set; }
        public List<HallFeePayment> hallFeePayments { get; set; }
        public List<DinningFeePayment> dinningFeePayments { get; set; }

        public PaymentPageDto()
        {
            if (AssignedDinningFeeToShow == null)
            {
                AssignedDinningFeeToShow = new List<AssignedDinningFeeToShow>();
            }
            if (AssignedHallFeeToShow == null)
            {
                AssignedHallFeeToShow = new List<AssignedHallFeeToShow>();
            }
        }
    }
}
