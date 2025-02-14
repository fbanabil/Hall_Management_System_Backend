using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IChatRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<List<StudentsMessage>> GetChats(int? hallId);
        public Task<Student> GetStudentByEmail(string email);

        public Task<string> GetStudentName(int studentId);

    }
}
