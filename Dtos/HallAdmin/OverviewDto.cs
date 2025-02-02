namespace Student_Hall_Management.Dtos
{
    public class OverviewDto
    {
        public int TotalStudents { get; set; }
        public int OccupiedRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int PendingComplaints { get; set; }
        public int TotalNotices { get; set; }
        public int TotalSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public int TotalRooms { get; set; }
        public int InProgressComplaints { get; set; }
        public int ResolvedComplaints { get; set; }
        public IEnumerable<ComplaintOverviewDto> RecentComplaints { get; set; }
        public double Review { get; set; }
        public Dictionary<string, int> ComplaintsCategory { get; set; }
        public int TotalReview { get; set; }


        public OverviewDto()
        {
            TotalStudents = 0;
            OccupiedRooms = 0;
            AvailableRooms = 0;
            PendingComplaints = 0;
            TotalNotices = 0;
            TotalSeats = 0;
            OccupiedSeats = 0;
            TotalRooms = 0;
            InProgressComplaints = 0;
            ResolvedComplaints = 0;
            RecentComplaints = new List<ComplaintOverviewDto>();
            Review = 0;
            ComplaintsCategory = new Dictionary<string, int>();
        }


    }
}
