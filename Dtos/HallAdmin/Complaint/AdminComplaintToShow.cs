namespace Student_Hall_Management.Dtos
{
    public class AdminComplaintToShow
    {
        public int ComplaintId { get; set; }
        public string Title { get; set; }
        public string Catagory { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public AdminComplaintToShow()
        {
            ComplaintId = 0;
            Title = "";
            Catagory = "";
            Priority = "";
            Status = "";
            Description = "";
            Location = "";
            Image = "";
        }
    }
}
