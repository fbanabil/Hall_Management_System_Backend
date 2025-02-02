namespace Student_Hall_Management.Dtos
{
    public class StudentInRoomDto
    {
        public string Name { get; set; }
        public int StudentId { get; set; }

        public StudentInRoomDto()
        {
            if (Name == null)
            {
                Name = "";
            }
        }
    }
}
