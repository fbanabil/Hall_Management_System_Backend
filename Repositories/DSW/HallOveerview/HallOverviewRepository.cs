using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class HallOverviewRepository : IHallOverviewRepository
    {
        DataContextEF _entityFramework;
        public HallOverviewRepository(IConfiguration entityFramework)
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
        //gethallId
        public int? GetHallId(string email)
        {
            HallAdmin? hallAdmin = _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefault<HallAdmin>();
            return hallAdmin?.HallId;
        }
        //GetHalls
        public async Task<List<HallDetails>> GetHalls()
        {
            List<HallDetails> hallList = _entityFramework.HallDetails.ToList();

            return hallList;
        }
    }
}
