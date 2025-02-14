namespace Student_Hall_Management.Dtos
{
    public class HallsToShow
    {

        public int hallId { get; set; }
        public string hallName { get; set; }
        public int totalSeats { get; set; }
        public int occupiedSeats { get; set; }
        public int availableSeats { get; set; }
        public string hallType { get; set; }
        public string imageData { get; set; }
        public HallsToShow()
        {
            if (hallName == null)
            {
                hallName = " ";
            }
            if (totalSeats == 0)
            {
                totalSeats = 0;
            }
            if (occupiedSeats == 0)
            {
                occupiedSeats = 0;
            }
            if (availableSeats == 0)
            {
                availableSeats = 0;
            }
            if (hallType == null)
            {
                hallType = " ";
            }
            if (imageData == null)
            {
                imageData = " ";
            }
        }
    }
}
