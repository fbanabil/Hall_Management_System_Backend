namespace Student_Hall_Management.Models
{
    public partial class HallAdmin
    {
        public int HallAdminId { get; set; }
        public int HallId { get; set; }
        public string Email { get; set; }

        public HallAdmin()
        {
            if (Email == null)
            {
                Email = "";
            }
        }
    }
}
