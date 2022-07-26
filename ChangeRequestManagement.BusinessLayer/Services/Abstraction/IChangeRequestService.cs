using ChangeRequestManagement.Entities;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IChangeRequestService
    {
        Task<ChangeRequest> AddChangeRequestAsync(AddChangeRequestViewModel model);
        Task<ChangeRequestInfoViewModel> GetChangeRequestByChangeRequestIdAsync(int changeRequestId);
        Task<ChangeRequest> UpdateChangeRequestInfoAsync(UpdateChangeRequestInfoViewModel model);
        Task<int> GetChangeRequestCountByModuleIdAsync(int moduleId);
        Task<int> CountTotalChangeRequestsAsync();
    }
}
