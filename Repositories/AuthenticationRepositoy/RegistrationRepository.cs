using Student_Hall_Management.Models;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Data;
using System.Linq;

namespace Student_Hall_Management.Repositories
{
    public class RegistrationRepository: IRegistrationRepository
    {
        DataContextEF _entityFramework;
        public RegistrationRepository(IConfiguration entityFramework)
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

        public Student GetSingleStudent(string email)
        {
            Student? student = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();

            if (student != null)
            {
                return student;
            }

            return null;

        }

        public StudentPendingRequest PendingRequest(string email)
        {
            StudentPendingRequest? request = _entityFramework.StudentPendingRequest
                .Where(u => u.Email == email)
                .FirstOrDefault<StudentPendingRequest>();

            if (request != null)
            {
                return request;
            }

            return null;
        }

        // Add VARBINARY Columns in Authentication Table

        public void AddStudentAuthentication(StudentAuthentication studentAuthentication)
        {
            if (studentAuthentication != null)
            {
                _entityFramework.StudentAuthentication.Add(studentAuthentication);
            }   

        }

        public string GetHallAdmin(string email)
        {
            HallAdminAuthentication? hallAdmin = _entityFramework.HallAdminAuthentications
                .Where(u => u.Email == email)
                .FirstOrDefault<HallAdminAuthentication>();
            if(hallAdmin != null)
            {
                return hallAdmin.Email;
            }
            return null;
        }


    }
}
