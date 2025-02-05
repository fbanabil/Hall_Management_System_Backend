namespace Student_Hall_Management.Dtos
{ 
    public class AdminNoticePageDto
    {
        public int TotalNotices { get; set; }
        public int TotalViews { get; set; }
        public int TotalFavourites {  get; set; }
        public int LastMonth { get; set; }
        public IEnumerable<AdminNoticeToShowDto> Notices { get; set; }


        public AdminNoticePageDto()
        {
            if (Notices == null)
            {
                Notices = new List<AdminNoticeToShowDto>();
            }
        }
    }
}
