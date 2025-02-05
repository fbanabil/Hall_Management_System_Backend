namespace Student_Hall_Management.Dtos
{
    public class ChangePasswordDto
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ChangePasswordDto()
        {
            if (Password == null)
            {
                Password = "";
            }
            if (NewPassword == null)
            {
                NewPassword = "";
            }
            if (ConfirmPassword == null)
            {
                ConfirmPassword = "";
            }
        }
    }
}
