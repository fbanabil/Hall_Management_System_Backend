using AutoMapper;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class StudentPaymentHelper
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;

        public StudentPaymentHelper(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = new MapperConfiguration(cfg =>
            {

            }
            ).CreateMapper();
        }

        public async Task<PaymentPage> GetStudentPaymentPage(int studentId)
        {
            PaymentPage paymentPage = new PaymentPage();

            IEnumerable<HallFeePayment> hallPayments = await _paymentRepository.GetHallPaymentsByStudentId(studentId);

            List<HallPaymentsToShowDto> hallPaymentsToShow = new List<HallPaymentsToShowDto>();

            foreach (var hallPayment in hallPayments)
            {
                HallPaymentsToShowDto hallPaymentsToShowDto = new HallPaymentsToShowDto();
                hallPaymentsToShowDto.Amount = hallPayment.PaymentAmount;
                hallPaymentsToShowDto.PaymentDate = hallPayment.PaymentDate?.ToString("dd/MM/yyyy") ?? "N/A";
                hallPaymentsToShowDto.Status = hallPayment.PaymentStatus;
                hallPaymentsToShowDto.LevelAndTerm = hallPayment.LevelAndTerm;
                hallPaymentsToShowDto.HallFeePaymentId = hallPayment.HallFeePaymentId;

                hallPaymentsToShow.Add(hallPaymentsToShowDto);
            }

            IEnumerable<DinningFeePayment> dinningPayments = await _paymentRepository.GetDinningPaymentsByStudentId(studentId);
            List<DinningPaymentsToShowDto> dinningPaymentsToShow = new List<DinningPaymentsToShowDto>();

            foreach (var dinningPayment in dinningPayments)
            {
                DinningPaymentsToShowDto dinningPaymentsToShowDto = new DinningPaymentsToShowDto();
                dinningPaymentsToShowDto.Amount = dinningPayment.PaymentAmount;
                dinningPaymentsToShowDto.PaymentDate = dinningPayment.PaymentDate?.ToString("dd/MM/yyyy") ?? "N/A";
                dinningPaymentsToShowDto.Status = dinningPayment.PaymentStatus;
                dinningPaymentsToShowDto.Month = dinningPayment.Month;
                dinningPaymentsToShowDto.DinningFeePaymentId = dinningPayment.DinningFeePaymentId;
                dinningPaymentsToShowDto.Year = dinningPayment.Year;
                dinningPaymentsToShow.Add(dinningPaymentsToShowDto);
            }

            hallPaymentsToShow = hallPaymentsToShow.OrderBy(x=>x.LevelAndTerm).ToList();
            dinningPaymentsToShow.Reverse();
            paymentPage.HallPayments = hallPaymentsToShow;
            paymentPage.DinningPayments = dinningPaymentsToShow;



            return paymentPage;
        }
    }
}
