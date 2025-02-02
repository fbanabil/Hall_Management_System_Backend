using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Repositories;
using System.Security.Claims;

namespace Student_Hall_Management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class HomePageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHomePageRepository _homePageRepository;

        public HomePageController(IHomePageRepository homePageRepository)
        {
            _homePageRepository = homePageRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<HomePageDto, HomePage>();
                //cfg.CreateMap<HomePage, HomePageDto>();
            }).CreateMapper();
        }

        [HttpGet("GetHomePageData")]
        public async Task<HomePageDto> GetHomePageData()
        {
            string? email=User.FindFirst("userEmail")?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            HomePageDto homePageDto = new HomePageDto();
            if (email != null && roles.Contains("Student"))
            {
                homePageDto.ImageData = _homePageRepository.GetSingleStudentImageData(email);
                homePageDto.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(homePageDto.ImageData));

            }
            else if (email != null && roles.Contains("HallAdmin"))
            {
                //Console.WriteLine(email);

                int? hallId = await Task.Run(() => _homePageRepository.GetHallId(email));
                //Console.WriteLine(hallId);
                if (hallId != null)
                {
                    //Console.WriteLine(homePageDto.ImageData);
                    homePageDto.ImageData =await Task.Run(() => _homePageRepository.GetSingleHallImageData((int)hallId));
                    //Console.WriteLine(homePageDto.ImageData);

                    if (homePageDto.ImageData != null)
                    {
                        homePageDto.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(homePageDto.ImageData));
                    }
                    else
                    {
                        homePageDto.ImageData = null;
                    }
                }
                else
                {
                    homePageDto.ImageData = null;
                }


                //homePageDto.ImageData = null;

            }
            return homePageDto;
        }
    }
}
