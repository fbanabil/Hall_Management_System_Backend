using AutoMapper;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Helpers
{
    public class AdminComplaintHelper
    {
        private readonly IMapper _mapper;
        private readonly IComplaintManagementRepository _complaintManagementRepository;

        public AdminComplaintHelper(IComplaintManagementRepository complaintManagementRepository)
        {
            _complaintManagementRepository = complaintManagementRepository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ComplaintToShowDto, Complaint>();
                cfg.CreateMap<Complaint, ComplaintToShowDto>();
                cfg.CreateMap<CommentToShowDto, Comment>();
                cfg.CreateMap<Comment, CommentToShowDto>();

            }
            ).CreateMapper();
        }


        public async Task<AdminComplaintPage> GetComplaintPage(int hallId)
        {
            AdminComplaintPage adminComplaintPage = new AdminComplaintPage();

            Tuple<int, int, int, int> complaintDetails = await _complaintManagementRepository.GetComplaintDetails(hallId);

            adminComplaintPage.TotalComplaints = complaintDetails.Item1;
            adminComplaintPage.TotalPendingComplaints = complaintDetails.Item2;
            adminComplaintPage.TotalInProgressComplaints = complaintDetails.Item3;
            adminComplaintPage.TotalResolvedComplaints = complaintDetails.Item4;

            return adminComplaintPage;
        }


        public async Task<IEnumerable<ComplaintToShowDto>> ComplaintToShow(int hallId)
        {
            IEnumerable<Complaint> complaints =await _complaintManagementRepository.GetComplaintsOfHall(hallId);
            List<ComplaintToShowDto> complaintToShowDtos = new List<ComplaintToShowDto>();

            foreach (Complaint c in complaints)
            {
                ComplaintToShowDto complaintToShow = _mapper.Map<ComplaintToShowDto>(c);

                IEnumerable<Comment> comments = await Task.Run(() => _complaintManagementRepository.GetCommentsByComplaitId(c.ComplaintId));
                List<CommentToShowDto> commentToShowDtos = comments.Select(comment => _mapper.Map<CommentToShowDto>(comment)).ToList();

                complaintToShow.Comments = commentToShowDtos;

                if (!string.IsNullOrEmpty(c.ImageData))
                {
                    complaintToShow.ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes(c.ImageData));
                }
                if (!string.IsNullOrEmpty(c.FileData))
                {
                    complaintToShow.FileData = Convert.ToBase64String(System.IO.File.ReadAllBytes(c.FileData));
                }

                complaintToShowDtos.Add(complaintToShow);
            }
            return complaintToShowDtos;
        }
    }
}
