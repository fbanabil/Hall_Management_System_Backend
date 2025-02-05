using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Dtos.DSW;
using Student_Hall_Management.Dtos.HallAdmin;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize(Roles ="HallAdmin,DSW")]
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
                    cfg.CreateMap<HallToEditDto, HallDetails>();
                    cfg.CreateMap<HallDetails, HallToEditDto>();
                    cfg.CreateMap<HallDetails, HallDetailsToShowDto>();
                    cfg.CreateMap<HallDetailsToShowDto, HallDetails>();

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


        [HttpPut("EditHall")]
        public IActionResult EditHall(HallToEditDto hallToEditDto)
        {
            string? email = User.FindFirst("userEmail")?.Value;
            Console.WriteLine(email);
            int? HallId = _hallDetailsManagement.GetHallId(email);
            Console.WriteLine(hallToEditDto.HallName);
            HallDetails? hallDetails = _hallDetailsManagement.GetHallDetails(HallId);

            if (hallDetails == null)
            {
                return NotFound(new { message = "Hall not found" });
            }
            Console.WriteLine(hallDetails.HallType);
            hallDetails.HallName = hallToEditDto.HallName;
            hallDetails.HallType = hallToEditDto.HallType;
            Console.WriteLine(hallToEditDto.HallType);

            if (hallToEditDto.ImageData != null || hallToEditDto.ImageData!="")
            {
                string base64Data = hallToEditDto.ImageData;

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
            }

            _hallDetailsManagement.UpdateEntity<HallDetails>(hallDetails);
            if (_hallDetailsManagement.SaveChanges())
            {
                HallDetails hallDetails1 = _hallDetailsManagement.GetHallDetails(HallId);
                HallDetailsToShowDto hallDetailsToShow = _mapper.Map<HallDetailsToShowDto>(hallDetails1);
                hallDetailsToShow.TotalSeats = _hallDetailsManagement.TotalSeats(HallId);
                hallDetailsToShow.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(hallDetails1.ImageData));
                //Console.WriteLine(hallDetails.Established);
                hallDetailsToShow.Established = DateOnly.ParseExact(hallDetails1.Established.ToString("dd/MM/yyyy"), "dd/MM/yyyy");
                return Ok(hallDetailsToShow);
            }
            else
            {
                return BadRequest(new { message = "Hall Details Editing Failed" });
            }
        }





    }
}
