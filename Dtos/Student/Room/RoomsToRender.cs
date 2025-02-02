namespace Student_Hall_Management.Dtos
{
    public class RoomsToRender
    {
        public bool IsAlloted { get; set; }
        public IEnumerable<RoomsToShow> RoomsToShow { get; set; }

        public RoomsToRender()
        {
            RoomsToShow = new List<RoomsToShow>();
        }
    }
}
