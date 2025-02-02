namespace Student_Hall_Management.Dtos
{
    public class HomePageDto
    {
        public string ImageData { get; set; }

        public HomePageDto()
        {
            if (ImageData == null)
            {
                ImageData = "";
            }
        }
    }
}
