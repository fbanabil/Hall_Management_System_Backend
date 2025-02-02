namespace Student_Hall_Management.Dtos
{
    public class CommentToShowDto
    {
        //public int CommentId { get; set; }
        public string CommentText { get; set; }
        //public int ComplaintId { get; set; }
        //public int StudentId { get; set; }
        //public int HallAdminId { get; set; }
        //public int HallId { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentedAt { get; set; }

        public CommentToShowDto()
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
