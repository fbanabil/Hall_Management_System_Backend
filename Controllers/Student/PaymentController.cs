using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly StudentPaymentHelper _studentPaymentHelper;
        private readonly IPresentDateTime _presentDateTime;

        public PaymentController(IPaymentRepository paymentRepository,IPresentDateTime presentDateTime)
        {
            _studentPaymentHelper = new StudentPaymentHelper(paymentRepository);
            _presentDateTime = presentDateTime;
            _paymentRepository = paymentRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
            }
            ).CreateMapper();
        }


        [HttpGet("GetStudentPaymentPage")]
        public async Task<ActionResult<PaymentPageDto>> GetStudentPaymentPage()
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? studentId = await _paymentRepository.GetStudentId(email);
            PaymentPage paymentPageDto = await _studentPaymentHelper.GetStudentPaymentPage(studentId.Value);
            return Ok(paymentPageDto);
        }

        [HttpPost("PayHallFee/{paymentId}")]
        public async Task<ActionResult> PayHallFee(int paymentId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? studentId = await _paymentRepository.GetStudentId(email);

            HallFeePayment hallFeePayment = await _paymentRepository.GetHallFeePaymentById(paymentId);
            hallFeePayment.PaymentStatus = "Paid";
            hallFeePayment.PaymentDate = _presentDateTime.GetPresentDateTime();
            _paymentRepository.UpdateEntity(hallFeePayment);
            if (await _paymentRepository.SaveChangesAsync())
            {
                return Ok(await _studentPaymentHelper.GetStudentPaymentPage(studentId.Value));
            }
            else
            {
                return BadRequest("SomeThing Went Wrong");



            }
        }

        [HttpPost("PayDinningFee/{paymentId}")]
        public async Task<ActionResult> PayDinningFee(int paymentId)
        {
            string email = User.FindFirst("userEmail")?.Value;
            int? studentId = await _paymentRepository.GetStudentId(email);

            DinningFeePayment? dinningFeePayment = await _paymentRepository.GetDinningFeePaymentById(paymentId);
            dinningFeePayment.PaymentStatus = "Paid";
            dinningFeePayment.PaymentDate = _presentDateTime.GetPresentDateTime();
            _paymentRepository.UpdateEntity(dinningFeePayment);
            if (await _paymentRepository.SaveChangesAsync())
            {
                return Ok(await _studentPaymentHelper.GetStudentPaymentPage(studentId.Value));
            }
            else
            {
                return BadRequest("SomeThing Went Wrong");



            }
        }


    }
}
