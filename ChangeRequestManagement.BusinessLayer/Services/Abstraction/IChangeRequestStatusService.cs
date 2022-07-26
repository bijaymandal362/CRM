using ChangeRequestManagement.Entities;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IChangeRequestStatusService
    {
        Task<ChangeRequestStatus> AddChangeRequestStatusAsync(AddChangeRequestStatusViewModel model);
        Task<List<ChangeRequestStatusInfoViewModel>> GetChangeRequestStatusInfoAsync(int changeRequestId);
        Task<int> CountTotalPendingChangeRequestsAsync();
        Task<int> CountTotalApprovedChangeRequestsAsync();
    }
}
