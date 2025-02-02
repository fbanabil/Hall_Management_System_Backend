using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IComplaintRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Student GetSingleStudent(string email);
        public DateTime PresentDateTime();
        public IEnumerable<Complaint> GetComplaintsOfHall(int? hallId, int pageNumber, int pageSize);
        public List<Comment> GetCommentsByComplaitId(int complaintId);
    }
}
