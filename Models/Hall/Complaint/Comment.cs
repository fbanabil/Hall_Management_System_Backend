namespace Student_Hall_Management.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int? ComplaintId { get; set; }
        public int? StudentId { get; set; }
        public int? HallAdminId { get; set; }
        public int? HallId { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentedAt { get; set; }
        public Comment()
        {
            if (CommentText == null)
            {
                CommentText = "";
            }
            if (CommentedBy == null)
            {
                CommentedBy = "";
            }
        }
    }
}
