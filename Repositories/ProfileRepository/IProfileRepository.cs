using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IProfileRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Student GetSingleStudentProfile(string email);
        public string GetHallName(int? hallId);
        public Task<int?> GetHallId(string? email);
        public Task<HallDetails> GetHallDetails(int? hallId);
    }
}
