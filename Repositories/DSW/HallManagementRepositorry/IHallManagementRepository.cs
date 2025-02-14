using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IHallManagementRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<List<HallDetails>> GetHalls();

        public Task<int> GetTotalSeats(int hallId);


    }
}
