namespace Student_Hall_Management.Dtos
{
    public class ComplaintOverviewDto
    {
        public int ComplaintId { get; set; }
        public string Title { get;set; }
        public string Catagory { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string ComplaintDate { get; set; }

        public ComplaintOverviewDto()
        {
            ComplaintId = 0;
            Title = "";
            Catagory = "";
            Priority = "";
            Status = "";
            Location = "";
            ComplaintDate = "";
        }

    }
}
