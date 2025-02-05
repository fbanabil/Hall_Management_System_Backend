using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        DataContextDapper _dapper;
        DataContextEF _entityFramework;
        IPresentDateTime _presentDateTime;

        public PaymentRepository(IConfiguration entityFramework, IPresentDateTime presentDateTime)
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

        public async Task<IEnumerable <HallFeePayment>> GetHallPaymentsByStudentId(int studentId)
        {
            return await _entityFramework.HallFeePayments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable <DinningFeePayment>> GetDinningPaymentsByStudentId(int studentId)
        {
            return await _entityFramework.DinningFeePayments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<int> GetStudentId(string? email)
        {
            Student? student = await _entityFramework.Students
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return student?.Id ?? 0;
        }

        //GetHallFeePaymentById

        public async Task<HallFeePayment?> GetHallFeePaymentById(int hallFeePaymentId)
        {
            return await _entityFramework.HallFeePayments
                .Where(p => p.HallFeePaymentId == hallFeePaymentId)
                .FirstOrDefaultAsync();
        }

        //GetDinningFeePaymentById

        public async Task<DinningFeePayment?> GetDinningFeePaymentById(int dinningFeePaymentId)
        {
            return await _entityFramework.DinningFeePayments
                .Where(p => p.DinningFeePaymentId == dinningFeePaymentId)
                .FirstOrDefaultAsync();
        }
    }
}
