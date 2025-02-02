using Student_Hall_Management.Dtos;

namespace Student_Hall_Management.Repositories
{
    public interface IOverviewRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task<IEnumerable<ComplaintOverviewDto>> GetRecentComplaints(int? hallId);
        public Task<int> GetTotalStudents(int? hallId);
        public Task<Tuple<int, int, int, int>> GetRoomAndSeatDetails(int? hallId);
        public Task<Tuple<int, int, int>> GetComplaintStatusCount(int? hallId);
        public Task<int> GetTotalNotices(int? hallId);
        public Task<double> GetReview(int? hallId);
        public Task<Tuple<int, int, int, int>> GetComplaintCategoryDetails(int? hallId);
        public Task<int> GetTotalReviews(int? hallId);



    }
}
