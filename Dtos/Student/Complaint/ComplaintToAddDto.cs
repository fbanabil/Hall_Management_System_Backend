namespace Student_Hall_Management.Dtos
{
    public class ComplaintToAddDto
    {
        //public int ComplaintId { get; set; }
        public string Title { get; set; }
        public string Catagory { get; set; }
        public string Priority { get; set; }
        //public string Status { get; set; }
        public string Description { get; set; }
        //public int StudentId { get; set; }
        public string Location { get; set; }
        //public string HallId { get; set; }
        public string? ImageData { get; set; }
        public string? FileData { get; set; }
        //public DateTime ComplaintDate { get; set; }

        public ComplaintToAddDto() 
        {
            if (Title == null)
            {
                Title = "";
            }
            if (Catagory == null)
            {
                Catagory = "";
            }
            if (Priority == null)
            {
                Priority = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            if (Location == null)
            {
                Location = "";
            }
        }

    }
}
