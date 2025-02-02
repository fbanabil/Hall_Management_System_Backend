using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories 
{ 
    public interface IStudentManagementRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task RemoveEntityAsync<T>(T entityToRemove);
        public Task<Tuple<int, int>> GetTotalStudentAndIsActive(int hallId);
        public Task<IEnumerable<Student>> GetStudents(int hallId);
        public Task<Student> GetStudentById(int studentId);



    }
}
