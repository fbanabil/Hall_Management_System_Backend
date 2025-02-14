using AutoMapper;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class DSWHallManagementHelper
    {


        private readonly IHallManagementRepository _hallDetailsManagementRepository;
        private readonly IMapper _mapper;

        public DSWHallManagementHelper(IHallManagementRepository hallDetailsManagementRepository)
        {
            _hallDetailsManagementRepository = hallDetailsManagementRepository;
            _mapper = new MapperConfiguration(cfg =>
            { 
            }
            
            ).CreateMapper();
        }

        public async Task<HallManagementPage> GetHallManagementPage()
        {

            HallManagementPage hallManagementPage = new HallManagementPage();
            List<HallDetails> hallDetails = await _hallDetailsManagementRepository.GetHalls();
            Console.WriteLine(hallDetails.Count);
            List<HallsToShow> hallsToShow1 = new List<HallsToShow>();
            foreach (var hall in hallDetails)
            {
                HallsToShow hallsToShow = new HallsToShow();
                hallsToShow.hallId = hall.HallId;
                hallsToShow.hallName = hall.HallName;
                hallsToShow.hallType = hall.HallType;
                hallsToShow.availableSeats = hall.AvailableSeats;
                hallsToShow.totalSeats = await _hallDetailsManagementRepository.GetTotalSeats(hall.HallId);
                hallsToShow.occupiedSeats = hall.OccupiedSeats;
                hallsToShow.imageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(hall.ImageData));
                hallsToShow1.Add(hallsToShow);
            }
            hallManagementPage.HallsToShow = hallsToShow1;
            return hallManagementPage;
        }


    }
}
