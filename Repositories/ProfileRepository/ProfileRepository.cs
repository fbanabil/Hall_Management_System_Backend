using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        DataContextEF _entityFramework;
        public ProfileRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
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

        public Student GetSingleStudentProfile(string email)
        {
            Student? studentProfile = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();
            
            return studentProfile;
        }
        public string GetHallName(int? hallId)
        {
            HallDetails? hall = _entityFramework.HallDetails
                .Where(u => u.HallId == hallId)
                .FirstOrDefault<HallDetails>();
            return hall.HallName;
        }


        public async Task<int?> GetHallId(string? email)
        {
            HallAdmin? hallAdmin = await _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return hallAdmin?.HallId ?? null;
        }

        public async Task<HallDetails> GetHallDetails(int? hallId)
        {
            HallDetails? hall = await _entityFramework.HallDetails
                .Where(u => u.HallId == hallId)
                .FirstOrDefaultAsync();
            return hall?? null;
        }

        public int TotalSeats(int? hallId)
        {
            int totalSeats = _entityFramework.Rooms
                .Where(u => u.HallId == hallId)
                .Sum(u => u.HasSeats);
            return totalSeats;
        }

        //GetSingleStudentAuthentication
        public StudentAuthentication GetSingleStudentAuthentication(string email)
        {
            StudentAuthentication? studentAuthentication = _entityFramework.StudentAuthentication
                .Where(u => u.Email == email)
                .FirstOrDefault<StudentAuthentication>();
            return studentAuthentication;
        }
    }
}


