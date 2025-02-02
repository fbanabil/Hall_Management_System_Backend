namespace Student_Hall_Management.Dtos
{
    public class NoticeToAddDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string NoticeType { get; set; }

        public NoticeToAddDto()
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
