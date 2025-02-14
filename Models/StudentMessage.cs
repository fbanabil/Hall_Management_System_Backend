namespace Student_Hall_Management.Models
{
    public partial class StudentsMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int Sender { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
        public StudentsMessage()
        {
            if (Id == 0)
            {
                Id = 0;
            }
            if (Message == null)
            {
                Message = " ";
            }
   
            if (Date == null)
            {
                Date = DateTime.Now;
            }
        }
    }
}
