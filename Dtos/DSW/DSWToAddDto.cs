namespace Student_Hall_Management.Dto
{
    public class DSWToAddDto
    {
        public string Email { get; set; }
        public string Password { get; set; }


         public DSWToAddDto()
        {
            if (Email == null)
            {
                Email = "";
            }

        }
    }
}
