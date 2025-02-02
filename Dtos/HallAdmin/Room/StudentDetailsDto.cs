namespace Student_Hall_Management.Dtos.HallAdmin.Room
{
    public class StudentDetailsDto
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public int StudentId { get; set; }

        public StudentDetailsDto() 
        {
            if (Name == null)
            {
                Name = "";
            }
            if (Department == null)
            {
                Department = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (Image == null)
            {
                Image = "";
            }
        }


    }
}
