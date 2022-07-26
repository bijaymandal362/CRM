using ChangeRequestManagment.Models.Client;
using ChangeRequestManagment.Models.ViewModels.ClientRequestStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IClientService
    {
        Task<AddClientSuccessViewModel> AddClientAsync(AddClientViewModel model);
        Task<ClientInfoViewModel> GetClientInfoAsync(int clientid);
        Task<string> GetCompanyNameByClientEmailAsync(string email);
        Task<object> GetAllClientInfoAsync(ClientDataTableHelper tableHelper);
        Task<bool> UpdateClientInfoAsync(UpdateClientInfoViewModel model);
        Task<List<ClientRequestStatusViewModel>> GetChangeRequestsAsync(string email); 
        Task<bool> RemoveClientAsync(int id);
        Task<int> GetClientIdByPersonIdAsync(int personId);
        Task<int> CountTotalClientsAsync();

    }
}
