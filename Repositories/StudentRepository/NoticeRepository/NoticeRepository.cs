using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        DataContextEF _entityFramework;
        public NoticeRepository(IConfiguration entityFramework)
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
            Student? entity = _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefault<Student>();
            return entity;
        }


        public IEnumerable<Notice> GetNoticesOfHall(int? hallId, int pageNumber=1, int pageSize=5)
        {
            
            IEnumerable<Notice> notices = _entityFramework.Notices
                .Where(u => u.HallId == hallId)
                .OrderByDescending(u => u.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return notices;
        }

        public IEnumerable<Notice> GetNoticesOfHall(int? hallId)
        {
            IEnumerable<Notice> notices = _entityFramework.Notices
                .Where(u => u.HallId == hallId)
                .OrderByDescending(u => u.Date)
                .ToList();
            return notices;
        }

        public Notice GetSingleNotice(int noticeId)
        {
            Notice? entity = _entityFramework.Notices
                .Where(u => u.NoticeId == noticeId)
                .FirstOrDefault<Notice>();
            return entity;
        }

        public int TotalNoticesOfHall(int? hallId)
        {
            return _entityFramework.Notices
                .Where(u => u.HallId == hallId)
                .Count();
        }

        public bool PriorityOrFavourite(int noticeId, int studentId)
        {

            int? noticePriority = _entityFramework.NoticePriorities
                .Where(u => u.NoticeId == noticeId && u.StudentId == studentId)
                .Count<NoticePriority>();

            if (noticePriority == 0) return false;

            return true;
        }

        public NoticePriority? NoticePriority(int noticeId, int studentId)
        {
            NoticePriority? entity = _entityFramework.NoticePriorities
                .Where(u => u.NoticeId == noticeId && u.StudentId == studentId)
                .FirstOrDefault<NoticePriority>();
            return entity;
        }

        public bool IsRead(int noticeId, int studentId)
        {

            int? isRead = _entityFramework.IsReads
                .Where(u => u.NoticeId == noticeId && u.StudentId == studentId)
                .Count<IsRead>();

            if (isRead == 0) return false;

            return true;
        }

        public IsRead? IsReadEntity(int noticeId, int studentId)
        {
            IsRead? entity = _entityFramework.IsReads
                .Where(u => u.NoticeId == noticeId && u.StudentId == studentId)
                .FirstOrDefault<IsRead>();
            return entity;
        }
    }
}
