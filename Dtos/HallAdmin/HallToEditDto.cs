namespace Student_Hall_Management.Dtos.HallAdmin
{
    public class HallToEditDto
    {
        public string HallName { get; set; }
        public string HallType { get; set; }
        public string ImageData { get; set; }


        public HallToEditDto()
        {
            if (HallName == null)
            {
                HallName = "";
            }
            if (HallType == null)
            {
                HallType = "";
            }
            if (ImageData == null)
            {
                ImageData = "";
            }
        }


    }
}
