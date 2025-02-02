namespace Student_Hall_Management.Models.Hall
{
    public partial class HallReview
    {
        public int ReviewId { get; set; }
        public int HallId { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public int Reviewer { get; set; }
        public DateTime ReviewDate { get; set; }
        public HallReview()
        {
            ReviewId = 0;
            HallId = 0;
            Review = "";
            Rating = 0;
            ReviewDate = DateTime.Now;
        }
    }
}
