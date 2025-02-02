namespace Student_Hall_Management.Dtos.HallAdmin.Student
{
    public class StudentManagementPageDto
    {
        public int TotalStudents { get; set; }
        public int PaymentDue { get; set; }
        public int ActiveStudents { get; set; }
        public int DinningAttendenceInPercent { get; set; }
        public IEnumerable<StudentToShowDto> Students { get; set; }


        public StudentManagementPageDto()
        {
            if (Students == null)
            {
                Students = new List<StudentToShowDto>();
            }
        }
    }
}
