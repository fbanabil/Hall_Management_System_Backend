namespace Student_Hall_Management.Models
{
    public class DSW
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


        public DSW()
        {
            if (Email == null)
            {
                Email = "";
            }
        }
    }
}
