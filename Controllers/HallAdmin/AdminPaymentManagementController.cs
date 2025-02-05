using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles="HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class AdminPaymentManagementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdminPaymentRepository _adminPaymentRepository;
        private readonly AdminPaymentHelper _adminPaymentHelper;
        private readonly IPresentDateTime _presentDateTime;

        public AdminPaymentManagementController(IAdminPaymentRepository adminPaymentRepository, IPresentDateTime presentDateTime)
        {
            _adminPaymentRepository = adminPaymentRepository;
            _adminPaymentHelper = new AdminPaymentHelper(adminPaymentRepository);
            _presentDateTime = presentDateTime;
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

                //cfg.CreateMap<DinningFeePaymentToAdd, DinningFeePayment>();
            }
            ).CreateMapper();
        }


        [HttpPost("AssignHallFee")]
        public async Task<IActionResult> AssignHallFee(HallFeePaymentToAdd hallFeePaymentToAdd)
        {
            string email = User.FindFirst("userEmail").Value;
            int? hallId =await Task.Run(()=>  _adminPaymentRepository.GetHallId(email));
            AssignedHallFee? assignedHallFee = await Task.Run(()=> _adminPaymentRepository.GetAssignedHallFeeByBatchAndLevelAndTerm(hallId.Value,hallFeePaymentToAdd.Batch,hallFeePaymentToAdd.LevelAndTerm));

            if (assignedHallFee != null)
            {
                return BadRequest("Already assigned for this batch and level and term");
            }

            AssignedHallFee assignedHallFee1 = new AssignedHallFee();
            _mapper.Map(hallFeePaymentToAdd, assignedHallFee1);
            assignedHallFee1.HallId = hallId.Value;
            assignedHallFee1.Date = _presentDateTime.GetPresentDateTime();

            await Task.Run(() => _adminPaymentRepository.AddEntityAsync(assignedHallFee1));

            IEnumerable<Student> students = await Task.Run(() => _adminPaymentRepository.GetStudentsByHallId(hallId.Value));

            foreach (var student in students)
            {
                if(student.Email.Substring(1,2)==hallFeePaymentToAdd.Batch.ToString())
                {
                    HallFeePayment hallFeePayment = new HallFeePayment();
                    hallFeePayment.StudentId = student.Id;
                    hallFeePayment.HallId = hallId.Value;
                    hallFeePayment.PaymentMethod = "Not Paid";
                    hallFeePayment.PaymentDate = null;
                    hallFeePayment.PaymentStatus = "Not Paid";
                    hallFeePayment.LevelAndTerm = hallFeePaymentToAdd.LevelAndTerm;
                    hallFeePayment.PaymentAmount = hallFeePaymentToAdd.TotalAmount;

                    await Task.Run(() => _adminPaymentRepository.AddEntityAsync(hallFeePayment));
                }
            }

            if(await Task.Run(() => _adminPaymentRepository.SaveChangesAsync()))
            {
                return Ok(await _adminPaymentHelper.GetPaymentPage(hallId.Value));
            }
            return BadRequest("Failed to assign");
        }

        [HttpPost("AssignDinningFee")]
        public async Task<IActionResult> AssignDinningFee(DinningFeePaymentToAdd dinningFeePaymentToAdd)
        {
            string email = User.FindFirst("userEmail").Value;
            int? hallId = await Task.Run(() => _adminPaymentRepository.GetHallId(email));
            AssignedDinningFee? assignedDinningFee = await Task.Run(() => _adminPaymentRepository.GetDinningFeePaymentByMonthAndYear(hallId.Value, dinningFeePaymentToAdd.Month, dinningFeePaymentToAdd.Year));

            if (assignedDinningFee != null)
            {
                return BadRequest("Already assigned for this month and year");
            }

            AssignedDinningFee assignedDinningFee1 = new AssignedDinningFee();
            _mapper.Map(dinningFeePaymentToAdd, assignedDinningFee1);
            assignedDinningFee1.HallId = hallId.Value;
            assignedDinningFee1.Date = _presentDateTime.GetPresentDateTime();

            await Task.Run(() => _adminPaymentRepository.AddEntityAsync(assignedDinningFee1));

            IEnumerable<Student> students = await Task.Run(() => _adminPaymentRepository.GetStudentsByHallId(hallId.Value));

            foreach (var student in students)
            {
                DinningFeePayment dinningFeePayment = new DinningFeePayment();
                
                dinningFeePayment.StudentId = student.Id;
                dinningFeePayment.HallId = hallId.Value;
                dinningFeePayment.PaymentMethod = "Not Paid";
                dinningFeePayment.PaymentDate = null;
                dinningFeePayment.PaymentStatus = "Not Paid";
                dinningFeePayment.Month = dinningFeePaymentToAdd.Month;
                dinningFeePayment.Year = dinningFeePaymentToAdd.Year;
                dinningFeePayment.PaymentAmount = dinningFeePaymentToAdd.TotalAmount;

                await Task.Run(() => _adminPaymentRepository.AddEntityAsync(dinningFeePayment));

            }

            if (await Task.Run(() => _adminPaymentRepository.SaveChangesAsync()))
            {
                return Ok(await _adminPaymentHelper.GetPaymentPage(hallId.Value));
            }
            return BadRequest("Failed to assign");
        }


        [HttpGet("GetPaymentPage")]
        public async Task<IActionResult> GetPaymentPage()
        {
            string email = User.FindFirst("userEmail").Value;
            int? hallId = await Task.Run(() => _adminPaymentRepository.GetHallId(email));
            PaymentPageDto paymentPageDto = await _adminPaymentHelper.GetPaymentPage(hallId.Value);
            return Ok(paymentPageDto);
        }

    }
}
