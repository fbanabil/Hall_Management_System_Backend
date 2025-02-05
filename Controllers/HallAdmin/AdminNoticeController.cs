using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    //[Authorize("StudentPolicy")]
    [ApiController]
    [Route("/[controller]")]
    public class AdminNoticeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INoticeRepository _noticeRepository;
        private readonly IPresentDateTime _presentDateTime;

        public AdminNoticeController(INoticeRepository noticeRepository,IPresentDateTime presentDateTime)
        {
            _noticeRepository = noticeRepository;
            _presentDateTime = presentDateTime;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NoticeToShowDto, Notice>();
                cfg.CreateMap<Notice, NoticeToShowDto>();
                cfg.CreateMap<NoticeToAddDto, Notice>();
                cfg.CreateMap<Notice, NoticeToAddDto>();
            }).CreateMapper();
        }



        
    }
}
