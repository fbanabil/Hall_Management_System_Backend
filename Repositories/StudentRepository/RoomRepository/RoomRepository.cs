using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        DataContextEF _entityFramework;
        private readonly DataContextDapper _dapper;
        public RoomRepository(IConfiguration entityFramework,IConfiguration config)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _dapper = new DataContextDapper(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }
        public void UpdateEntity<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _entityFramework.Update(entityToUpdate);
            }
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _entityFramework.Rooms.ToList();
        }

        public IEnumerable<Student> GetStudentsInRoom(string roomNo)
        {
            return _dapper.LoadData<Student>("select * from HallManagementSchema.Students where RoomNo='" + roomNo + "'");
        }

        public IEnumerable<Room> GetAllRoomsOfHall(int? hallId)
        {
            if(hallId == null)
            {
                return null;
            }
            IEnumerable<Room> rooms = _entityFramework.Rooms
                .Where(u => u.HallId == hallId)
                .ToList();
            return rooms;
        }

        public Student GetSingleStudent(string email)
        {
            if(email==null) Console.WriteLine("HI");
            Student? student = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();
            return student;
        }

        public PendingRoomRequest? GetPendingRoomRequest(int studentId, int? hallId)
        {
            PendingRoomRequest pendingRoomRequest = _entityFramework.PendingRoomRequests
                .Where(u => u.StudentId == studentId && u.HallId == hallId)
                .FirstOrDefault<PendingRoomRequest>();
            return pendingRoomRequest;
        }

        public Room GetRoomByRoomNo(string roomNo)
        {
            Room room = _entityFramework.Rooms
                .Where(u => u.RoomNo == roomNo)
                .FirstOrDefault<Room>();
            return room;
        }


    }
}
