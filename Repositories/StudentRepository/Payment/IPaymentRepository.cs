using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface IPaymentRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public Task<int?> GetHallId(string? email);
        public Task AddEntityAsync<T>(T entityToAdd);
        public Task<bool> SaveChangesAsync();
        public Task RemoveEntityAsync<T>(T entityToRemove);
        public Task<IEnumerable<HallFeePayment>> GetHallPaymentsByStudentId(int studentId);
        public Task<IEnumerable<DinningFeePayment>> GetDinningPaymentsByStudentId(int studentId);
        public Task<int> GetStudentId(string? email);
        public Task<HallFeePayment?> GetHallFeePaymentById(int hallFeePaymentId);

        public Task<DinningFeePayment?> GetDinningFeePaymentById(int dinningFeePaymentId);


    }
}
