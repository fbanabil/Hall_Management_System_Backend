namespace Student_Hall_Management.Dtos
{
    public class StudentVerificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string ImageData { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }

        public StudentVerificationDto()
        {
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
            if (Password == null)
            {
                Password = "";
            }
            if (ConfirmPassword == null)
            {
                ConfirmPassword = "";
            }
            if (VerificationCode == null)
            {
                VerificationCode = "";
            }
            if (ImageData == null)
            {
                ImageData = "";
            }
        }
    }
}
