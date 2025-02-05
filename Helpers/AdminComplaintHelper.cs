using AutoMapper;
using Student_Hall_Management.Dtos;
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
    }
}
