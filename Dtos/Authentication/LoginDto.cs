namespace Student_Hall_Management.Dtos
{
    public partial class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public LoginDto()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
            if (Role == null)
            {
                Role = "";
            }
        }
    }
}
