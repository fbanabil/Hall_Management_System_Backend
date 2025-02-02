using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos.DSW;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class HallDetailsManagement: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHallDetailsManagementRepository _hallDetailsManagement;

        public HallDetailsManagement(IHallDetailsManagementRepository hallDetailsManagement)
        {
            _hallDetailsManagement = hallDetailsManagement;
            _mapper=new MapperConfiguration(cfg=>
                {
                    cfg.CreateMap<HallToAddDto, HallDetails>();
                    cfg.CreateMap<HallDetails, HallToAddDto>();
                }
            ).CreateMapper();
        }


        [HttpPost("AddHallDetails")]
        public IActionResult AddHallDetails(HallToAddDto hallToAddDto)
        {
            HallDetails hallDetails = _mapper.Map<HallDetails>(hallToAddDto);
            //Console.WriteLine(hallDetails.ImageData);
            hallDetails.AvailableSeats = hallDetails.TotalSeats;
            hallDetails.OccupiedSeats = 0;

            string base64Data = hallDetails.ImageData;

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

            string uniqueFileName = $"{hallDetails.HallName.Trim()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}";
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

            hallDetails.ImageData = filePath;

            _hallDetailsManagement.AddEntity<HallDetails>(hallDetails);
            if (_hallDetailsManagement.SaveChanges())
            {
                return Ok(hallDetails);
            }
            else
            {
                return BadRequest(new { message = "Hall Details Adding Failed" });
            }
        }



        


    }
}
