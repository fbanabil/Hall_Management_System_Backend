namespace Student_Hall_Management.Models
{
    public partial class HallDetails
    {
        public int HallId { get; set; }
        public string HallName { get; set; }
        public string Institution { get; set; }
        public int TotalSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public int AvailableSeats { get; set; }
        public string HallType { get; set; } //Male or female
        public string ImageData { get; set; }
        public DateOnly Established{ get; set; }


        public HallDetails()
        {
            
            if(HallName== null)
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
