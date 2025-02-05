using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using System.Globalization;

namespace Student_Hall_Management.Repositories
{
    public class AdminPaymentRepository : IAdminPaymentRepository
    {
        DataContextDapper _dapper;
        DataContextEF _entityFramework;
        IPresentDateTime _presentDateTime;

        public AdminPaymentRepository(IConfiguration entityFramework, IPresentDateTime presentDateTime)
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




        public async Task<IEnumerable<Student>> GetStudentsByHallId(int hallId)
        {
            return await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HallFeePayment>> GetHallFeePaymentsByHallId(int hallId)
        {
            return await _entityFramework.HallFeePayments
                .Where(p => p.HallId == hallId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DinningFeePayment>> GetDinningFeePaymentsByHallId(int hallId)
        {
            return await _entityFramework.DinningFeePayments
                .Where(p => p.HallId == hallId)
                .ToListAsync();
        }

        public async Task<AssignedHallFee?> GetAssignedHallFeeByBatchAndLevelAndTerm(int hallId, int batch, string levelAndTerm)
        {
            return await _entityFramework.AssignedHallFees
                .Where(f => f.HallId == hallId && f.Batch == batch && f.LevelAndTerm == levelAndTerm)
                .FirstOrDefaultAsync();
        }

        public async Task<AssignedDinningFee?> GetDinningFeePaymentByMonthAndYear(int hallId, string month, int year)
        {
            return await _entityFramework.AssignedDinningFees
                .Where(f => f.HallId == hallId && f.Year == year && f.Month == month)
                .FirstOrDefaultAsync();
        }


        public async Task<Tuple<int, int, int, int>> GetPaaymenyDetails(int hallId)
        {

            int totalHallFee = await _entityFramework.HallFeePayments.Where(p => p.HallId == hallId).SumAsync(p => p.PaymentAmount);
            int totalDinningFee = await _entityFramework.DinningFeePayments.Where(p => p.HallId == hallId).SumAsync(p => p.PaymentAmount);
            int totalFee = totalDinningFee + totalHallFee;
            int paid = await _entityFramework.HallFeePayments.Where(p => p.HallId == hallId && p.PaymentStatus == "Paid").SumAsync(p => p.PaymentAmount) +
                await _entityFramework.DinningFeePayments.Where(p => p.HallId == hallId && p.PaymentStatus == "Paid").SumAsync(p => p.PaymentAmount);
            int paymentCompletedInPercent = (int)(((double)paid / totalFee) * 100);
            Console.WriteLine(paymentCompletedInPercent);
            return new Tuple<int, int, int, int>(totalFee, totalHallFee, totalDinningFee, paymentCompletedInPercent);
        }

        public async Task<IEnumerable<AssignedHallFee>> GetAssignedHallFeesByHallId(int hallId)
        {
            return await _entityFramework.AssignedHallFees
                .Where(f => f.HallId == hallId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssignedDinningFee>> GetAssignedDinningFeesByHallId(int hallId)
        {
            return await _entityFramework.AssignedDinningFees
                .Where(f => f.HallId == hallId)
                .ToListAsync();


        }

    }
    
}
