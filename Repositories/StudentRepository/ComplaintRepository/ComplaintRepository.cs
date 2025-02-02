using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        DataContextEF _entityFramework;
        PresentDateTime _presentDateTime;
        public ComplaintRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _presentDateTime = new PresentDateTime(entityFramework);

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
            Student? studentProfile = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();
            return studentProfile;
        }

        public DateTime PresentDateTime()
        {
            return _presentDateTime.GetPresentDateTime();
        }

        public IEnumerable<Complaint> GetComplaintsOfHall(int? hallId, int pageNumber, int pageSize)
        {
            return _entityFramework.Complaints
                .Where(c => c.HallId == hallId)
                .OrderByDescending(c => c.ComplaintDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<Comment> GetCommentsByComplaitId(int complaintId)
        {
            return _entityFramework.Comments
                .Where(c => c.ComplaintId == complaintId)
                .ToList();
        }
    }
}
