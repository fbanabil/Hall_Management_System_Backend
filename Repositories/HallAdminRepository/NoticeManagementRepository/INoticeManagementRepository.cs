using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface INoticeManagementRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task RemoveEntityAsync<T>(T entityToRemove);
        public Task<int> TotalNotices(int hallId);
        public Task<int> TotalViews(int hallId);
        public Task<int> TotalFavourites(int hallId);
        public Task<int> LastMonth(int hallId);
        public Task<IEnumerable<Notice>> GetNotices(int hallId);
        public Task<int> GetViesCountByNoticeId(int noticeId);
        public Task<Notice> GetNotice(int noticeId);


    }
}
