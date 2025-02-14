using Microsoft.Identity.Client;

namespace Student_Hall_Management.Dtos
{
    public class HallListDto
    {
        public int HallId { get; set; }
        public string HallName { get; set; }


        public HallListDto()
        {
            if (HallId == 0)
            {
                HallId = 0;
            }
            if (HallName == null)
            {
                HallName = " ";
            }
        }

    }
}
