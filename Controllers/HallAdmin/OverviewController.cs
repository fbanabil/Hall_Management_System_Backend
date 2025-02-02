using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;
using System.Security.Claims;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles = "HallAdmin")]
    [ApiController]
    [Route("/[controller]")]
    public class OverviewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOverviewRepository _overviewRepository;
        private readonly IPresentDateTime _presentDateTime;

        public OverviewController(IOverviewRepository overviewRepository, IPresentDateTime presentDateTime)
        {
            _overviewRepository = overviewRepository;
            _presentDateTime = presentDateTime;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ComplaintOverviewDto, Complaint>();
                cfg.CreateMap<Complaint, ComplaintOverviewDto>();

            }).CreateMapper();
        }

        [HttpGet("GetHallOverview")]
        public async Task<IActionResult> GetHallOverview()
        {

            string email = User.FindFirst("userEmail")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            //Console.WriteLine(roles.Contains("HallAdmin"));
            if (!roles.Contains("HallAdmin"))
            {
                return Unauthorized("Hi");
            }

            int? hallId = await Task.Run(() => _overviewRepository.GetHallId(email));
            
            OverviewDto overviewDto = new OverviewDto();
            IEnumerable<ComplaintOverviewDto> complaintOverviewDto = new List<ComplaintOverviewDto>();


            complaintOverviewDto = await Task.Run(() => _overviewRepository.GetRecentComplaints(hallId));

            overviewDto.RecentComplaints = complaintOverviewDto;

            overviewDto.TotalStudents = await Task.Run(() => _overviewRepository.GetTotalStudents(hallId));
            Tuple<int, int, int, int> roomAndSeatDetails = await Task.Run(() => _overviewRepository.GetRoomAndSeatDetails(hallId));

            overviewDto.TotalRooms = roomAndSeatDetails.Item1;
            overviewDto.TotalSeats = roomAndSeatDetails.Item2;
            overviewDto.OccupiedRooms = roomAndSeatDetails.Item3;
            overviewDto.OccupiedSeats = roomAndSeatDetails.Item4;

            overviewDto.AvailableRooms = overviewDto.TotalRooms - overviewDto.OccupiedRooms;



            Tuple<int, int, int> complaintDetails = await Task.Run(() => _overviewRepository.GetComplaintStatusCount(hallId));

            overviewDto.InProgressComplaints = complaintDetails.Item1;
            overviewDto.PendingComplaints = complaintDetails.Item2;
            overviewDto.ResolvedComplaints = complaintDetails.Item3;

            overviewDto.TotalNotices = await Task.Run(() => _overviewRepository.GetTotalNotices(hallId));
            overviewDto.Review= await Task.Run(() => _overviewRepository.GetReview(hallId));

            Tuple<int, int, int, int> ComplaintCategoryDetails = await Task.Run(() => _overviewRepository.GetComplaintCategoryDetails(hallId));

            overviewDto.ComplaintsCategory["Others"]=ComplaintCategoryDetails.Item1-ComplaintCategoryDetails.Item2-ComplaintCategoryDetails.Item3-ComplaintCategoryDetails.Item4;
            overviewDto.ComplaintsCategory["Electrical"] = ComplaintCategoryDetails.Item2;
            overviewDto.ComplaintsCategory["Cleaning"] = ComplaintCategoryDetails.Item4;
            overviewDto.ComplaintsCategory["Plumbing"] = ComplaintCategoryDetails.Item3;
            

            //overviewDto.ComplaintsCategory.Reverse();

            overviewDto.TotalReview = await Task.Run(() => _overviewRepository.GetTotalReviews(hallId));

            return Ok(overviewDto);

        }

    }
}
