namespace Student_Hall_Management.Dtos
{
    public class HallManagementPage
    {
        public List<HallsToShow> HallsToShow { get; set; }


        public HallManagementPage()
        {
            HallsToShow = new List<HallsToShow>();
        }

    }
}
