namespace Student_Hall_Management.Dtos.HallAdmin.Student
{
    public class StudentToShowDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public int Batch { get; set; }
        public string RoomNo { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsActive { get; set; }

        public string Image { get; set; }
        public StudentToShowDto()
        {
            if(Image==null)
            {
                Image = "";
            }
            if (Name == null)
            {
                Name = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (Department == null)
            {
                Department = "";
            }
            if (RoomNo == null)
            {
                RoomNo = "";
            }
            if (PaymentStatus == null)
            {
                PaymentStatus = "";
            }
        }
    }
}
