namespace Student_Hall_Management.Models
{
    public partial class StudentPendingRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime Sent { get; set; }
        public StudentPendingRequest()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (Code == null)
            {
                Code = "";
            }
        }
    }
}
