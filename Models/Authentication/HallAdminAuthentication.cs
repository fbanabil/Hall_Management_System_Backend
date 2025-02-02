namespace Student_Hall_Management.Models
{
    public partial class HallAdminAuthentication
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public HallAdminAuthentication()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}
