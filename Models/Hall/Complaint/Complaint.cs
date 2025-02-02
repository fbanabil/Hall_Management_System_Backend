namespace Student_Hall_Management.Models
{
    public partial class Complaint
    {
        public int ComplaintId { get; set; }
        public string Title { get; set; }
        public string Catagory { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int? StudentId { get; set; }
        public string Location { get; set; }
        public int? HallId { get; set; }
        public string? ImageData { get; set; }
        public string? FileData { get; set; }
        public DateTime ComplaintDate { get; set; }

        //Student Student { get; set; }
        //HallDetails Hall { get; set; }

        public Complaint()
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
            if (Status == null)
            {
                Status = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            if (Location == null)
            {
                Location = "";
            }
            if (ImageData == null)
            {
                ImageData = "";
            }
            if (FileData == null)
            {
                FileData = "";
            }
        }
    }
}
