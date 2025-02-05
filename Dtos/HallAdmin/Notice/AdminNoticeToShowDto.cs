namespace Student_Hall_Management.Dtos
{
    public class AdminNoticeToShowDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string NoticeType { get; set; }
        public string NoticeId { get; set; }
        public string Date { get; set; }
        public int Views { get; set; }


        public AdminNoticeToShowDto()
        {
            if (Title == null)
            {
                Title = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            if (NoticeType == null)
            {
                NoticeType = "";
            }
        }
    }
}
