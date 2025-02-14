using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class HallManagementRepository:IHallManagementRepository
    {
        DataContextEF _entityFramework;
        IMapper _mapper;
        public HallManagementRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _mapper = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<ComplaintOverviewDto, ComplaintOverviewDto>();
            }
            ).CreateMapper();

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
        public async Task<List<HallDetails>> GetHalls()
        {
            List<HallDetails> hallList =await  _entityFramework.HallDetails.ToListAsync();
            return hallList;
        }

        public async Task<int> GetTotalSeats(int hallId)
        {
            int totalSeats = await _entityFramework.Rooms.Where(r=> r.HallId==hallId).SumAsync(h => h.HasSeats);
            return totalSeats;
        }



    }
}
