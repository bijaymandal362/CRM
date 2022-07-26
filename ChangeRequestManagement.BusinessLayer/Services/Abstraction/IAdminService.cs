using ChangeRequestManagment.Models.ViewModels.ClientRequestStatus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IAdminService
    {
        Task<List<ClientRequestStatusViewModel>> GetClientRequests();
    }
}