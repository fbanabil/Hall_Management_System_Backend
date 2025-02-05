using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IAdminPaymentRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task RemoveEntityAsync<T>(T entityToRemove);
        public Task<IEnumerable<Student>> GetStudentsByHallId(int hallId);
        public Task<IEnumerable<HallFeePayment>> GetHallFeePaymentsByHallId(int hallId);
        public Task<IEnumerable<DinningFeePayment>> GetDinningFeePaymentsByHallId(int hallId);
        public Task<AssignedHallFee?> GetAssignedHallFeeByBatchAndLevelAndTerm(int hallId, int batch, string levelAndTerm);
        public Task<AssignedDinningFee?> GetDinningFeePaymentByMonthAndYear(int hallId, string month, int year);
        public Task<Tuple<int, int, int, int>> GetPaaymenyDetails(int hallId);
        public Task<IEnumerable<AssignedHallFee>> GetAssignedHallFeesByHallId(int hallId);
        public Task<IEnumerable<AssignedDinningFee>> GetAssignedDinningFeesByHallId(int hallId);


    }
}
