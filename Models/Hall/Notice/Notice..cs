namespace Student_Hall_Management.Models
{
    public partial class Notice
    {
        public int NoticeId { get; set; }
        public string Title { get; set; }
        public string NoticeType { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
        public bool Priority { get; set; }
        public bool IsRead { get; set; }


        public Notice()
        {
            if (Title == null)
            {
                Title = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            if(NoticeType == null)
            {
                NoticeType = "";
            }
        }
    }
}
