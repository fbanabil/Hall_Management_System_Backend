using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class HallDetailsManagementRepository:IHallDetailsManagementRepository
    {
        DataContextEF _entityFramework;
        public HallDetailsManagementRepository(IConfiguration entityFramework)
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

        //GetHallDetails

        public HallDetails? GetHallDetails(int? hallId)
        {
            HallDetails? hallDetails = _entityFramework.HallDetails
                .Where(u => u.HallId == hallId)
                .FirstOrDefault<HallDetails>();
            return hallDetails;
        }

        public int TotalSeats(int? hallId)
        {
            int totalSeats = _entityFramework.Rooms
                .Where(u => u.HallId == hallId)
                .Sum(u => u.HasSeats);
            return totalSeats;
        }
    }
}
