using ChangeRequestManagment.Models.Authentication;
using System;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IAccountService
    {
        Task<AuthenticationInfoViewModel> LoginPersonAsync(string email, string password);
        Task<PersonInfoViewModel> FindPersonByIdAsync(int id);
        Task<PersonInfoViewModel> FindPersonByEmailAsync(string emailAddress);
    }
}
