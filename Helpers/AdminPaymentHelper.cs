using AutoMapper;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class AdminPaymentHelper
    {
        private readonly IMapper _mapper;
        private readonly IAdminPaymentRepository _adminPaymentRepository;

        public AdminPaymentHelper(IAdminPaymentRepository adminPaymentRepository)
        {
            _adminPaymentRepository =  adminPaymentRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DinningFeePayment, DinningFeePaymentToAdd>();
                cfg.CreateMap<DinningFeePaymentToAdd, DinningFeePayment>();
                cfg.CreateMap<HallFeePayment, HallFeePaymentToAdd>();
                cfg.CreateMap<HallFeePaymentToAdd, HallFeePayment>();
                cfg.CreateMap<AssignedHallFee, HallFeePaymentToAdd>();
                cfg.CreateMap<HallFeePaymentToAdd, AssignedHallFee>();
                cfg.CreateMap<AssignedDinningFee, DinningFeePaymentToAdd>();
                cfg.CreateMap<DinningFeePaymentToAdd, AssignedDinningFee>();
                cfg.CreateMap<AssignedDinningFeeToShow, AssignedDinningFee>();
                cfg.CreateMap<AssignedHallFeeToShow, AssignedHallFee>();
                cfg.CreateMap<AssignedDinningFee,AssignedDinningFeeToShow>();
                cfg.CreateMap<AssignedHallFee, AssignedHallFeeToShow>();

                //cfg.CreateMap<DinningFeePaymentToAdd, DinningFeePayment>();

            }
            ).CreateMapper();
        }


        public async Task<PaymentPageDto> GetPaymentPage(int hallId)
        {
            PaymentPageDto paymentPageDto = new PaymentPageDto();

            Tuple<int, int, int, int> paymentDetails = await _adminPaymentRepository.GetPaaymenyDetails(hallId);
            paymentPageDto.TotalAmountToGet = paymentDetails.Item1;
            paymentPageDto.TotalHallFeeAmount = paymentDetails.Item2;
            paymentPageDto.TotalDinningFeeAmount = paymentDetails.Item3;
            paymentPageDto.PercentageOfPayment = paymentDetails.Item4;



            IEnumerable<AssignedHallFee> assignedHallFees = await _adminPaymentRepository.GetAssignedHallFeesByHallId(hallId);
            List<AssignedHallFeeToShow> assignedHallFeeToShows = assignedHallFees.Select(assignedHallFee => _mapper.Map<AssignedHallFeeToShow>(assignedHallFee)).ToList();

            IEnumerable<AssignedDinningFee> assignedDinningFees = await _adminPaymentRepository.GetAssignedDinningFeesByHallId(hallId);
            List<AssignedDinningFeeToShow> assignedDinningFeeToShows = assignedDinningFees.Select(assignedDinningFee => _mapper.Map<AssignedDinningFeeToShow>(assignedDinningFee)).ToList();

            IEnumerable<HallFeePayment> hallFeePayments = await _adminPaymentRepository.GetHallFeePaymentsByHallId(hallId);
            List<HallFeePayment> hallFeePayments1 = hallFeePayments.Select(hallFeePayment => _mapper.Map<HallFeePayment>(hallFeePayment)).ToList();

            IEnumerable<DinningFeePayment> dinningFeePayments = await _adminPaymentRepository.GetDinningFeePaymentsByHallId(hallId);
            List<DinningFeePayment> dinningFeePayments1 = dinningFeePayments.Select(dinningFeePayment => _mapper.Map<DinningFeePayment>(dinningFeePayment)).ToList();

            paymentPageDto.AssignedHallFeeToShow = assignedHallFeeToShows;
            paymentPageDto.AssignedDinningFeeToShow = assignedDinningFeeToShows;
            paymentPageDto.hallFeePayments = hallFeePayments1;
            paymentPageDto.dinningFeePayments = dinningFeePayments1;

            return paymentPageDto;
        }




    }
}
