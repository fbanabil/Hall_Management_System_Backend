using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Dtos.DSW;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles= "DSW")]
    [ApiController]
    [Route("/[controller]")]
    public class DSWHallOverview : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHallOverviewRepository _hallOverviewRepository;
        private readonly IOverviewRepository _overviewRepository;

        public DSWHallOverview( IHallOverviewRepository hallOverviewRepository,IOverviewRepository overviewRepository)
        {
            _overviewRepository = overviewRepository;

            _hallOverviewRepository = hallOverviewRepository;

            _mapper = new MapperConfiguration(cfg =>
            {
               
            }).CreateMapper();
        }


        [HttpGet("Halls")]
        public async Task<IActionResult> GetHalls()
        {
            List<HallListDto> hallListDtos = new List<HallListDto>();
            List<HallDetails> halls =await _hallOverviewRepository.GetHalls();

            foreach (var hall in halls)
            {
                HallListDto hallListDto = new HallListDto();
                hallListDto.HallId = hall.HallId;
                hallListDto.HallName = hall.HallName;
                hallListDtos.Add(hallListDto);

            }
            AllHallsLiat allHallsLiat = new AllHallsLiat();
            allHallsLiat.Halls = hallListDtos;
            Console.WriteLine(hallListDtos.Count());
            return Ok(allHallsLiat);
        }


        [HttpGet("GetHallOverview/{hallId}")]

        public async Task<IActionResult> GetHallOverview(int hallId)
        {
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
            overviewDto.Review = await Task.Run(() => _overviewRepository.GetReview(hallId));

            Tuple<int, int, int, int> ComplaintCategoryDetails = await Task.Run(() => _overviewRepository.GetComplaintCategoryDetails(hallId));

            overviewDto.ComplaintsCategory["Others"] = ComplaintCategoryDetails.Item1 - ComplaintCategoryDetails.Item2 - ComplaintCategoryDetails.Item3 - ComplaintCategoryDetails.Item4;
            overviewDto.ComplaintsCategory["Electrical"] = ComplaintCategoryDetails.Item2;
            overviewDto.ComplaintsCategory["Cleaning"] = ComplaintCategoryDetails.Item4;
            overviewDto.ComplaintsCategory["Plumbing"] = ComplaintCategoryDetails.Item3;


            //overviewDto.ComplaintsCategory.Reverse();

            overviewDto.TotalReview = await Task.Run(() => _overviewRepository.GetTotalReviews(hallId));

            return Ok(overviewDto);

        }






    }
    
}
