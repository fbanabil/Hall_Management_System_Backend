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


        public async Task<StudentManagementPageDto> GetStudentManagementPage(int hallId)
        {
            Tuple<int, int> TotalStudentAndIsActive = await Task.Run(() => _studentManagementRepository.GetTotalStudentAndIsActive(hallId));
            StudentManagementPageDto studentManagementPageDto = new StudentManagementPageDto();
            studentManagementPageDto.TotalStudents = TotalStudentAndIsActive.Item1;
            studentManagementPageDto.ActiveStudents = TotalStudentAndIsActive.Item2;

            IEnumerable<Student> students = new List<Student>();

            students = await Task.Run(() => _studentManagementRepository.GetStudents(hallId));

            students = students.ToList();

            List<StudentToShowDto> studentToShowDtos = new List<StudentToShowDto>();


            foreach (var student in students)
            {
                StudentToShowDto studentToShowDto = new StudentToShowDto();
                _mapper.Map(student, studentToShowDto);
                studentToShowDto.PaymentStatus = "Paid";
                studentToShowDto.Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(studentToShowDto.Image));
                studentToShowDtos.Add(studentToShowDto);
            }

            studentManagementPageDto.Students = studentToShowDtos;

            return studentManagementPageDto;

        }

        
    }
}
