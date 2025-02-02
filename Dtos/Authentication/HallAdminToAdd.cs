namespace Student_Hall_Management.Dtos
{
    public class HallAdminToAdd
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int HallId { get; set; }
        public HallAdminToAdd()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
            if (ConfirmPassword == null)
            {
                ConfirmPassword = "";
            }
        }
    }
}
