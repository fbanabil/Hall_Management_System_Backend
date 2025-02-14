using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class ChatRepository:IChatRepository
    {
        DataContextEF _entityFramework;
        public ChatRepository(IConfiguration entityFramework)
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

        public async Task<List<StudentsMessage>> GetChats(int? hallId)
        {
            List<StudentsMessage> chatList = await _entityFramework.StudentsMessages.Where(r=>r.HallId==hallId).ToListAsync();
            return chatList;
        }

        //getstudent bby email

        public async Task<Student> GetStudentByEmail(string email)
        {
            Student student = await _entityFramework.Students.FirstOrDefaultAsync(s => s.Email == email);
            return student;
        }
        //GetStudentName
        public async Task<string> GetStudentName(int studentId)
        {
            Student student = await _entityFramework.Students.FirstOrDefaultAsync(s => s.Id==studentId);
            return student.Name;
        }
    }
}
