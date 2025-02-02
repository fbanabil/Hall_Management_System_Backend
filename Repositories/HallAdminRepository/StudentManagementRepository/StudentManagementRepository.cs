using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class StudentManagementRepository : IStudentManagementRepository
    {
        DataContextEF _entityFramework;
        DataContextDapper _dapper;
        public StudentManagementRepository(IConfiguration entityFramework)
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

        public async Task<Tuple<int, int>> GetTotalStudentAndIsActive(int hallId)
        {
            int totalStudent = await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .CountAsync();
            int isActive = await _entityFramework.Students
                .Where(s => s.HallId == hallId && s.IsActive == true)
                .CountAsync();
            return new Tuple<int, int>(totalStudent, isActive);
        }

        public async Task<IEnumerable<Student>> GetStudents(int hallId)
        {
            IEnumerable<Student> students = await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .ToListAsync();
            return students;
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            Student student = await _entityFramework.Students
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();
            return student;
        }
    }
}
