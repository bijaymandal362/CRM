using ChangeRequestManagment.Models.ReturnInfo;
using ChangeRequestManagment.Models.ViewModels.Company;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetAllCompanyAsync();
        Task<CompanyViewModel> GetCompanyByIdAsync(int companyId);
        Task<int> GetCompanyIdByClientEmailAsync(string email);
        Task CreateCompanyAsync(CompanyViewModel model);
        Task UpdateCompanyAsync(CompanyViewModel model);
        Task<InfoHandlerViewModel> DeleteCompanyAsync(int companyId);
        Task<List<CompanyViewModel>> GetCompanyDropDownAsync();
        Task<int> CountTotalCompaniesAsync();
    } 
}
