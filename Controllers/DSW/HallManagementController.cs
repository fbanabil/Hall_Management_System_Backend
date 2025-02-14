using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles ="DSW")]
    [ApiController]
    [Route("/[controller]")]
    public class HallManagementController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHallManagementRepository _hallManagementRepository;
        private readonly IPresentDateTime _presentDateTime;
        private readonly DSWHallManagementHelper _dSWHallManagementHelper;
        public HallManagementController(IHallManagementRepository hallManagementRepository,IPresentDateTime presentDateTime)
        {
            _hallManagementRepository = hallManagementRepository;
            _presentDateTime = presentDateTime;
            _dSWHallManagementHelper = new DSWHallManagementHelper(_hallManagementRepository);
            _mapper = new MapperConfiguration(cfg =>
            {

            }).CreateMapper();
        }



        //public async Task<HallManagementPage> GetHallManagementPage()
        //{


        //    HallManagementPage hallManagementPage = new HallManagementPage();
        //    List<HallDetails> hallDetails = await _hallManagementRepository.GetHalls();
        //    List<HallsToShow> hallsToShow1 = new List<HallsToShow>();
        //    foreach (var hall in hallDetails)
        //    {
        //        HallsToShow hallsToShow = new HallsToShow();
        //        hallsToShow.hallId = hall.HallId;
        //        hallsToShow.hallName = hall.HallName;
        //        hallsToShow.hallType = hall.HallType;
        //        hallsToShow.availableSeats = hall.AvailableSeats;
        //        hallsToShow.totalSeats = await _hallManagementRepository.GetTotalSeats(hall.HallId);
        //        hallsToShow.occupiedSeats = hall.OccupiedSeats;
        //        hallsToShow.imageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(hall.ImageData));
        //        hallsToShow1.Add(hallsToShow);
        //    }
        //    hallManagementPage.HallsToShow = hallsToShow1;
        //    return hallManagementPage;
        //}


        [HttpGet("GetHalls")]
        public async Task<IActionResult> GetHalls()
        {
            HallManagementPage hallManagementPage =await  _dSWHallManagementHelper.GetHallManagementPage();
            //HallManagementPage hallManagementPage = new HallManagementPage();
            
            return Ok(hallManagementPage);
        }

        [HttpPost("AddHall")]
        public async Task<IActionResult> AddHall(HallToAddDto hallDetails)
        {
            HallDetails hallDetails1 = new HallDetails();

            Console.WriteLine(hallDetails.HallName);


            hallDetails1.HallName = hallDetails.HallName;
            hallDetails1.HallType = hallDetails.HallType;
            hallDetails1.TotalSeats = 0;
            hallDetails1.AvailableSeats = 0;
            hallDetails1.OccupiedSeats = 0;
            hallDetails1.ImageData = hallDetails.ImageData;


            string base64Data = hallDetails1.ImageData;

            if (base64Data.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                int commaIndex = base64Data.IndexOf(',');
                if (commaIndex != -1)
                {
                    base64Data = base64Data.Substring(commaIndex + 1);
                }
            }
            byte[] imageBytes = Convert.FromBase64String(base64Data);

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Hall", "Images");

            string uniqueFileName = $"{hallDetails1.HallName.Trim()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";
            //Console.WriteLine(imageName);

            string fileName = uniqueFileName + ".jpg";
            string filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            try
            {
                System.IO.File.WriteAllBytes(filePath, imageBytes);
            }
            catch (Exception e)
            {
                return BadRequest("Image Saving Failed");
            }

            hallDetails1.ImageData = filePath;

            hallDetails1.Established = DateOnly.FromDateTime(_presentDateTime.GetPresentDateTime()); // Fixed conversion


            _hallManagementRepository.AddEntity<HallDetails>(hallDetails1);





            if (_hallManagementRepository.SaveChanges())
            {
                HallManagementPage hallManagementPage =await _dSWHallManagementHelper.GetHallManagementPage();
                return Ok(hallManagementPage);
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
