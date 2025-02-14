namespace Student_Hall_Management.Dtos
{
    public class HallToAddDto
    {
        public string HallName { get; set; }
        
        public string HallType { get; set; } //Male or female
        public string ImageData { get; set; }
       // public DateTime Established { get; set; }
        public HallToAddDto() 
        {
            if(HallName == null)
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
