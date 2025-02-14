using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class LoginRepository:ILoginRepository
    {
        DataContextEF _entityFramework;
        public LoginRepository(IConfiguration entityFramework)
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

        public StudentAuthentication GetSingleStudentAuthentication(string email)
        {
            StudentAuthentication? studentAuthentication = _entityFramework.StudentAuthentication
                .Where(u => u.Email == email)
                .FirstOrDefault<StudentAuthentication>();
            if (studentAuthentication != null)
            {
                return studentAuthentication;
            }
            return null;
        }

        public HallAdminAuthentication GetSingleHallAdminAuthentication(string email)
        {
            HallAdminAuthentication? hallAdminAuthentication = _entityFramework.HallAdminAuthentications
                .Where(u => u.Email == email)
                .FirstOrDefault<HallAdminAuthentication>();
            if (hallAdminAuthentication != null)
            {
                return hallAdminAuthentication;
            }
            return null;
        }

        public async Task UpdateActivity(bool isActive, string email)
        {
            Student student = await _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (student != null)
            {
                student.IsActive = isActive;
                _entityFramework.Students.Update(student);
                _entityFramework.SaveChanges();
            }
        }
        //GetSingleDSW
        public DSW GetSingleDSW(string email)
        {
            DSW? dSW = _entityFramework.DSW
                .Where(u => u.Email == email)
                .FirstOrDefault<DSW>();
            if (dSW != null)
            {
                return dSW;
            }
            return null;
        }

        //UpdateActivity(false, email);

    }
}
