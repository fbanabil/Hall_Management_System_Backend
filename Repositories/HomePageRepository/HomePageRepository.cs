using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class HomePageRepository : IHomePageRepository
    {
        DataContextEF _entityFramework;
        public HomePageRepository(IConfiguration entityFramework)
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
        //specific attribute of Student entity
        public string GetSingleStudentImageData(string email)
        {
            Student? studentProfile = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();

            if (studentProfile != null)
            {
                return studentProfile.ImageData;
            }
            return null;
        }

        public async Task<int?> GetHallId(string email)
        {
            HallAdmin? hallAdmin =await _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return hallAdmin?.HallId ?? null;
        }

        public async Task<string?> GetSingleHallImageData(int hallId)
        {
            HallDetails? hall =await _entityFramework.HallDetails
                .Where(u => u.HallId == hallId)
                .FirstOrDefaultAsync();
            return hall?.ImageData ?? null; 

        }
    }
}
