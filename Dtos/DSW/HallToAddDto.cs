namespace Student_Hall_Management.Dtos.DSW
{
    public class HallToAddDto
    {
        public string HallName { get; set; }
        public string Institution { get; set; }
        public int TotalSeats { get; set; }
        public string HallType { get; set; } //Male or female
        public string ImageData { get; set; }

        public HallToAddDto() 
        {
            if(HallName == null)
            {
                HallName = "";
            }
            if (Institution == null)
            {
                Institution = "CUET";
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
