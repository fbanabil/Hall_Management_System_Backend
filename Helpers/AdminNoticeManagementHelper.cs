using AutoMapper;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class AdminNoticeManagementHelper
    {
        private readonly IMapper _mapper;
        private readonly INoticeManagementRepository _noticeManagementRepository;

        public AdminNoticeManagementHelper(INoticeManagementRepository INoticeManagementRepository)
        {
            _noticeManagementRepository = INoticeManagementRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Notice, AdminNoticeToShowDto>();
            }
            ).CreateMapper();
        }


        public async Task<AdminNoticePageDto> GetNoticePage(int hallId)
        {
            AdminNoticePageDto adminNoticePageDto = new AdminNoticePageDto();

            adminNoticePageDto.TotalNotices = await _noticeManagementRepository.TotalNotices(hallId);
            adminNoticePageDto.TotalViews = await _noticeManagementRepository.TotalViews(hallId);
            adminNoticePageDto.TotalFavourites = await _noticeManagementRepository.TotalFavourites(hallId);
            adminNoticePageDto.LastMonth = await _noticeManagementRepository.LastMonth(hallId);

            List<AdminNoticeToShowDto> adminNoticeToShowDtos = new List<AdminNoticeToShowDto>();

            IEnumerable<Notice> notices = await _noticeManagementRepository.GetNotices(hallId);

            foreach (Notice notice in notices)
            {
                AdminNoticeToShowDto adminNoticeToShowDto = new AdminNoticeToShowDto();
                _mapper.Map(notice, adminNoticeToShowDto);
                adminNoticeToShowDto.Date = notice.Date.ToString("dd/MM/yyyy");
                adminNoticeToShowDto.Views = await _noticeManagementRepository.GetViesCountByNoticeId(notice.NoticeId);
                adminNoticeToShowDtos.Add(adminNoticeToShowDto);
            }
            adminNoticeToShowDtos.Reverse();
            adminNoticePageDto.Notices = adminNoticeToShowDtos;

            return adminNoticePageDto;
        }
    }
}
