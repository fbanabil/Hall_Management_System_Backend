using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IAdminRoomRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task<Tuple<int, int, int, int, int>> GetOverallRoomDetails(int? hallId);
        public Task<IEnumerable<Room>> GetRooms(int? hallId);
        public IEnumerable<Student> GetStudentByRoomNo(string? roomNo);
        public Task<IEnumerable<PendingRoomRequest>> GetPendingRoomRequests(int? hallId);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task<Room> GetRoomByRoomNo(string roomNo,int hallId);
        public Task<Student> GetStudent(int studentId);
        public Task<PendingRoomRequest> GetPendingRoomRequest(int studentId, int hallId);
        public Task RemoveEntityAsync<T>(T entityToRemove);


    }
}
