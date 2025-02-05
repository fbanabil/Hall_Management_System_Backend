using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Student_Hall_Management.Data;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public class OverviewRepository : IOverviewRepository
    {
        DataContextEF _entityFramework;
        IMapper _mapper;
        public OverviewRepository(IConfiguration entityFramework)
        {
            _entityFramework = new DataContextEF(entityFramework);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ComplaintOverviewDto, ComplaintOverviewDto>();
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

        public async Task<int?> GetHallId(string? email)
        {
            HallAdmin? hallAdmin = await _entityFramework.HallAdmins
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return hallAdmin?.HallId ?? null;
        }

        public async Task<IEnumerable<ComplaintOverviewDto>> GetRecentComplaints(int? hallId)
        {
            IEnumerable<ComplaintOverviewDto> complaintOverviewDto = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId)
                .OrderByDescending(c => c.ComplaintId)
                .Take(5)
                .Select(c => new ComplaintOverviewDto
                {
                    ComplaintId = c.ComplaintId,
                    Title = c.Title,
                    Catagory = c.Catagory,
                    Priority = c.Priority,
                    Status = c.Status,
                    Location = c.Location,
                    ComplaintDate = c.ComplaintDate.ToShortDateString()
                })
                .ToListAsync();
            return complaintOverviewDto;
        }

        public async Task<int> GetTotalStudents(int? hallId)
        {
            return await _entityFramework.Students
                .Where(s => s.HallId == hallId)
                .CountAsync();
        }

        public async Task<Tuple<int, int, int, int>> GetRoomAndSeatDetails(int? hallId)
        {
            int totalRooms = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .CountAsync();
            int totalSeats = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId)
                .Select(r => r.HasSeats)
                .SumAsync();

            int totalOccupiedRooms = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId && r.OccupiedSeats > 0)
                .CountAsync();

            int totalOccupiedSeats = await _entityFramework.Rooms
                .Where(r => r.HallId == hallId && r.OccupiedSeats > 0)
                .Select(r => r.OccupiedSeats)
                .SumAsync();

            return new Tuple<int, int, int, int>(totalRooms, totalSeats, totalOccupiedRooms, totalOccupiedSeats);
        }


        public async Task<Tuple<int, int, int>> GetComplaintStatusCount(int? hallId)
        {
            int inprogressComplaints = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "In-Progress")
                .CountAsync();
            int pendingComplaints = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "Pending")
                .CountAsync();
            int resolvedComplaints = await _entityFramework.Complaints
                .Where(c => c.HallId == hallId && c.Status == "Resolved")
                .CountAsync();
            return new Tuple<int, int, int>(inprogressComplaints, pendingComplaints, resolvedComplaints);
        }

        public async Task<int> GetTotalNotices(int? hallId)
        {
            return await _entityFramework.Notices
                .Where(n => n.HallId == hallId)
                .CountAsync();
        }

        public async Task<double> GetReview(int? hallId)
        {
            Console.WriteLine(hallId);
            if (hallId == null)
            {
                return 0;
            }

            var reviews = _entityFramework.HallReviews
                .Where(r => r.HallId == hallId);

            if (!await reviews.AnyAsync())
            {
                return 0;
            }

            double rating = await reviews.AverageAsync(r => r.Rating);
            return rating;
        }


        public async Task<Tuple<int,int,int,int>> GetComplaintCategoryDetails(int? hallId)
        {
            int electrical = await _entityFramework.Complaints
                .Where(r => r.HallId == hallId && r.Catagory == "Electrical")
                .CountAsync();

            int plumbing = await _entityFramework.Complaints
                .Where(r => r.HallId == hallId && r.Catagory == "Plumbing")
                .CountAsync();

            int cleaning = await _entityFramework.Complaints
                .Where(r => r.HallId == hallId && r.Catagory == "Cleaning")
                .CountAsync();
            int totalComplaints = await _entityFramework.Complaints
                .Where(r => r.HallId == hallId)
                .CountAsync();

            return new Tuple<int, int, int, int>(totalComplaints, electrical,plumbing, cleaning);

        }

        public async Task<int> GetTotalReviews(int? hallId)
        {
            return await _entityFramework.HallReviews
                .Where(r => r.HallId == hallId)
                .CountAsync();
        }
    }
}
