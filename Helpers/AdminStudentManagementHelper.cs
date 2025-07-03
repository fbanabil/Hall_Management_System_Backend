using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos.HallAdmin.Student;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class AdminStudentManagementHelper
    {
        private readonly IMapper _mapper;
        private readonly IStudentManagementRepository _studentManagementRepository;

        public AdminStudentManagementHelper(IStudentManagementRepository studentManagementRepository)
        {
            _studentManagementRepository = studentManagementRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentToShowDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageData));
            }
            ).CreateMapper();
        }


        public async Task<StudentManagementPageDto?> GetStudentManagementPage(int hallId)
        {
            List<Student> students1=await _studentManagementRepository.GetStudentsByHallId(hallId);
            //if(students1.Count == 0)
            //{
            //    return null;
            //}
            Tuple<int, int> TotalStudentAndIsActive = await Task.Run(() => _studentManagementRepository.GetTotalStudentAndIsActive(hallId));
            StudentManagementPageDto studentManagementPageDto = new StudentManagementPageDto();
            studentManagementPageDto.TotalStudents = TotalStudentAndIsActive.Item1;
            studentManagementPageDto.ActiveStudents = TotalStudentAndIsActive.Item2;
            studentManagementPageDto.PaymentDue = await Task.Run(() => _studentManagementRepository.PaymentDue(hallId));
            studentManagementPageDto.DinningAttendenceInPercent = await Task.Run(() => _studentManagementRepository.DinningAttendence(hallId));

            IEnumerable<Student> students = new List<Student>();

            students = await Task.Run(() => _studentManagementRepository.GetStudents(hallId));

            students = students.ToList();

            List<StudentToShowDto?> studentToShowDtos = new List<StudentToShowDto?>();


            foreach (var student in students)
            {
                StudentToShowDto studentToShowDto = new StudentToShowDto();
                _mapper.Map(student, studentToShowDto);
                bool notPaid = await Task.Run(() => _studentManagementRepository.NotPaidByStudentId(student.Id));
                if(!notPaid) studentToShowDto.PaymentStatus = "Paid";
                else studentToShowDto.PaymentStatus = "Not Paid";
                studentToShowDto.Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(studentToShowDto.Image));
                studentToShowDtos.Add(studentToShowDto);
            }

            studentManagementPageDto.Students = studentToShowDtos;

            return studentManagementPageDto;

        }



        public async Task<bool> DeleteStudent(int studentId)
        {
            Student student = await _studentManagementRepository.GetStudentById(studentId);

            IEnumerable<HallFeePayment> hallFeePayments = await _studentManagementRepository.HallFeePayment(studentId);
            IEnumerable<DinningFeePayment> dinningFeePayments = await _studentManagementRepository.DinningFeePayments(studentId);
            IEnumerable<HallReview> hallReviews = await _studentManagementRepository.HallReviews(studentId);
            IEnumerable<Comment> comments = await _studentManagementRepository.Comments(studentId);
            IEnumerable<Complaint> complaints = await _studentManagementRepository.Complaints(studentId);
            IEnumerable<PendingRoomRequest> pendingRoomRequests = await _studentManagementRepository.PendingRoomRequests(studentId);
            if (student != null)
            {
                StudentAuthentication studentAuthentication = await _studentManagementRepository.StudentAuthentication(student.Email);
                _studentManagementRepository.RemoveEntity<StudentAuthentication>(studentAuthentication);
            }
            if (student.RoomNo != null)
            {
                Room room = await _studentManagementRepository.Room(student.RoomNo);
                room.OccupiedSeats -= 1;
                _studentManagementRepository.UpdateEntity(room);
            }

            foreach (var hallFeePayment in hallFeePayments)
            {
                _studentManagementRepository.RemoveEntity<HallFeePayment>(hallFeePayment);
            }

            foreach (var dinningFeePayment in dinningFeePayments)
            {
                _studentManagementRepository.RemoveEntity<DinningFeePayment>(dinningFeePayment);
            }

            foreach (var hallReview in hallReviews)
            {
                _studentManagementRepository.RemoveEntity<HallReview>(hallReview);
            }

            foreach (var comment in comments)
            {
                _studentManagementRepository.RemoveEntity<Comment>(comment);
            }

            foreach (var complaint in complaints)
            {
                _studentManagementRepository.RemoveEntity<Complaint>(complaint);
            }

            foreach (var pendingRoomRequest in pendingRoomRequests)
            {
                _studentManagementRepository.RemoveEntity<PendingRoomRequest>(pendingRoomRequest);
            }

            _studentManagementRepository.RemoveEntity<Student>(student);

            return await _studentManagementRepository.SaveChangesAsync();
        }


    }
}
