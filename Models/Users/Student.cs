namespace Student_Hall_Management.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string ImageData { get; set; }
        public string? RoomNo { get; set; } // Foreign key
        public int? HallId { get; set; } // Foreign key

        public bool? IsActive { get; set; }
        public int? Batch { get; set; }
        public Room Room { get; set; } // Navigation property
        public HallDetails Hall { get; set; } // Navigation property
        
        public Student()
        {
            if (Name == null)
            {
                Name = "";
            }
            if (Email == null)
            {
                Email = "";
            }
            if (Department == null)
            {
                Department = "";
            }
            if (ImageData == null)
            {
                ImageData = "";
            }
            if (Role == null)
            {
                Role = "Student";
            }
        }
    }
}