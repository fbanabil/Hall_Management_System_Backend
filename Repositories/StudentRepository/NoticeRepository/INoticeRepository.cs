using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface INoticeRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Student GetSingleStudent(string email);
        public IEnumerable<Notice> GetNoticesOfHall(int? hallId, int pageNumber, int pageSize);
        public Notice GetSingleNotice(int noticeId);
        public int TotalNoticesOfHall(int? hallId);
        public IEnumerable<Notice> GetNoticesOfHall(int? hallId);
        public bool PriorityOrFavourite(int noticeId, int studentId);
        public NoticePriority? NoticePriority(int noticeId, int studentId);
        public bool IsRead(int noticeId, int studentId);
        public IsRead? IsReadEntity(int noticeId, int studentId);


    }
}
