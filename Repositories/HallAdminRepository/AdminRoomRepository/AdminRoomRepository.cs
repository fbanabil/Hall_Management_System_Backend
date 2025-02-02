using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class AdminRoomRepository : IAdminRoomRepository
    {
        DataContextEF _entityFramework;
        DataContextDapper _dapper;
        public AdminRoomRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _dapper = new DataContextDapper(entityFramework);

        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _entityFramework.SaveChangesAsync() > 0;
        }



        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }



        public async Task AddEntityAsync<T>(T entityToAdd)
        {

            if (entityToAdd != null)
            {
                await _entityFramework.AddAsync(entityToAdd);
            }

        }


        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public async Task RemoveEntityAsync<T>(T entityToRemove)
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
        public async Task<int?> GetHallId(string? email)
        {
            HallAdmin? hallAdmin = await _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return hallAdmin?.HallId ?? null;
        }

        public async Task<Tuple<int, int, int, int, int>> GetOverallRoomDetails(int? hallId)
        {
            int? totalRooms = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .CountAsync();

            int? totalSeats = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .SumAsync(r => r.HasSeats);

            int? availableRooms = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId && r.OccupiedSeats < r.HasSeats)
                .CountAsync();

            int? availableSeats = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .SumAsync(r => r.HasSeats - r.OccupiedSeats);

            int? roomConditionNotOk = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId && r.RoomCondition != "Good")
                .CountAsync();

            return Tuple.Create(totalRooms ?? 0, totalSeats ?? 0, availableRooms ?? 0, availableSeats ?? 0, roomConditionNotOk ?? 0);
        }

        public async Task<IEnumerable<Room>> GetRooms(int? hallId)
        {
            return await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .ToListAsync();
        }

        public IEnumerable<Student> GetStudentByRoomNo(string? roomNo)
        {
            return _dapper.LoadData<Student>("SELECT * FROM HallManagementSchema.Students WHERE RoomNo='" + roomNo + "'");
        }

        public async Task<IEnumerable<PendingRoomRequest>> GetPendingRoomRequests(int? hallId)
        {
            return await _entityFramework.PendingRoomRequests
                .Where(r => r.HallId == hallId)
                .ToListAsync();
        }

        public async Task<Room> GetRoomByRoomNo(string roomNo,int hallId)
        {
            return await _entityFramework.Rooms
                .Where(r => r.RoomNo == roomNo && r.HallId==hallId)
                .FirstOrDefaultAsync();
        }

        public async Task<Student> GetStudent(int studentId)
        {
            Student student = await _entityFramework.Students
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();
            return student;
        }


        public async Task<PendingRoomRequest> GetPendingRoomRequest(int studentId,int hallId)
        {
            return await _entityFramework.PendingRoomRequests
                .Where(r => r.StudentId == studentId && r.HallId == hallId)
                .FirstOrDefaultAsync();
        }

    }
}
