using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class ComplaintManagementRepository : IComplaintManagementRepository
    {
        DataContextDapper _dapper;
        DataContextEF _entityFramework;
        IPresentDateTime _presentDateTime;

        public ComplaintManagementRepository(IConfiguration entityFramework, IPresentDateTime presentDateTime)
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

        public async Task<Tuple<int,int,int,int>> GetComplaintDetails(int hallId)
        {
            int totalComplaints =await _entityFramework.Complaints
                .Where(c => c.HallId == hallId)
                .CountAsync();
            int pendingComplaints =await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "Pending")
                .CountAsync();
            int inProgressComplaints = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "In Progress")
                .CountAsync();
            int resolvedComplaints = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "Resolved")
                .CountAsync();
            return new Tuple<int, int, int, int>(totalComplaints, pendingComplaints,inProgressComplaints, resolvedComplaints);
        }

    }
}
