using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IRoomRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public IEnumerable<Room> GetAllRooms();
        public IEnumerable<Student> GetStudentsInRoom(string roomNo);
        public Student GetSingleStudent(string email);
        public IEnumerable<Room> GetAllRoomsOfHall(int? hallId);
        public PendingRoomRequest? GetPendingRoomRequest(int studentId, int? hallId);
        public Room GetRoomByRoomNo(string roomNo);

    }
}
