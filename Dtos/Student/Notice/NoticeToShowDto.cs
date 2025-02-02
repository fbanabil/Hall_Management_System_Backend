namespace Student_Hall_Management.Dtos
{
    public class NoticeToShowDto
    {
        public int NoticeId { get; set; }
        public string Title { get; set; }
        public string NoticeType { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public bool Priority { get; set; }
        public bool IsRead { get; set; }

        public NoticeToShowDto()
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
