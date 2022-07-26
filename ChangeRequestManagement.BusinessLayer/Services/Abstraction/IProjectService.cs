using ChangeRequestManagment.Models.ViewModels.DataTableProperty;
using ChangeRequestManagment.Models.ViewModels.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IProjectService
    {
        Task<object> GetProjectAsync(DataTablePropertyViewModel model);
        Task<List<ProjectViewModel>> GetAllProjectAsync();
        Task<bool> AddProjectAsync(ProjectViewModel model);
        Task<ProjectViewModel> GetProjectByIdAsync(int id);
        Task<List<ProjectViewModel>> GetProjectByCompanyIdAsync(int companyId);
        Task UpdateProjectAsync(ProjectViewModel model);
        Task DeleteProjectAsync(int projectId);
        Task<int> CountTotalProjectsAsync();
    }
}
