namespace Student_Hall_Management.Dtos.DSW
{
    public class AllHallsLiat
    {
        public  List<HallListDto> Halls { get; set; }

        public AllHallsLiat()
        {
            Halls = new List<HallListDto>();
        }
    }
}
