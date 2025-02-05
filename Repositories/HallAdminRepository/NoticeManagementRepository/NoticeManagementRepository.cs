using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class NoticeManagementRepository : INoticeManagementRepository
    {
        DataContextDapper _dapper;
        DataContextEF _entityFramework;
        IPresentDateTime _presentDateTime;

        public NoticeManagementRepository(IConfiguration entityFramework,IPresentDateTime presentDateTime)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _dapper = new DataContextDapper(entityFramework);
            _presentDateTime = presentDateTime;

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


        public async Task<int> TotalNotices(int hallId)
        {
            return await _entityFramework.Notices
                .Where(n => n.HallId == hallId)
                .CountAsync();
        }

        public async Task<int> TotalViews(int hallId)
        {
            IEnumerable<Notice> notices = await _entityFramework.Notices
                .Where(n => n.HallId == hallId)
                .ToListAsync();

            int count = 0;
            foreach (Notice notice in notices)
            {
                count+=await _entityFramework.IsReads
                    .Where(r => r.NoticeId == notice.NoticeId)
                    .CountAsync();
            }
            return count;
        }

        public async Task<int> TotalFavourites(int hallId)
        {
            IEnumerable<Notice> notices = await _entityFramework.Notices
                .Where(n => n.HallId == hallId)
                .ToListAsync();

            int count = 0;
            foreach (Notice notice in notices)
            {
                count += await _entityFramework.NoticePriorities
                    .Where(r => r.NoticeId == notice.NoticeId)
                    .CountAsync();
            }
            return count;
        }

        public async Task<int> LastMonth(int hallId)
        {
            DateTime oneMonthAgo = _presentDateTime.GetPresentDateTime().AddDays(-30);

            int count = await _entityFramework.Notices
                .Where(n => n.HallId == hallId && n.Date >= oneMonthAgo)
                .CountAsync();

            return count;
        }

        public async Task<IEnumerable<Notice>> GetNotices(int hallId)
        {
            return await _entityFramework.Notices
                .Where(n => n.HallId == hallId)
                .ToListAsync();
        }

        public async Task<int> GetViesCountByNoticeId(int noticeId)
        {
            return await _entityFramework.IsReads
                .Where(r => r.NoticeId == noticeId)
                .CountAsync();
        }

        public async Task<Notice> GetNotice(int noticeId)
        {
            return await _entityFramework.Notices
                .Where(n => n.NoticeId == noticeId)
                .FirstOrDefaultAsync();
        }

    }
}
