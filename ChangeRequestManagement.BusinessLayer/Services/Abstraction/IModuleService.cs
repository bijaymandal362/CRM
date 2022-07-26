using ChangeRequestManagment.Models.ReturnInfo;
using ChangeRequestManagment.Models.ViewModels.Module;
using ChangeRequestManagment.Models.ViewModels.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IModuleService
    {
        Task<List<ProjectViewModel>> GetProjectDropDownAsync();
        Task<IEnumerable<ModuleViewModel>> GetAllModuleAsync();
        Task<ModuleViewModel> GetModuleByIdAsync(int moduleId);
        Task<int> CreateModuleAsync(ModuleViewModel model);
        Task<int?> UpdateModuleAsync(ModuleViewModel model);
        Task<bool> DeleteModuleAsync(int moduleId);
        Task<List<ModuleViewModel>> GetModuleByProjectIdAsync(int projectId);
        Task<List<ModuleViewModel>> GetSubModuleByParentModuleIdAsync(int moduleId); 
        Task<int> GetProjectIdByParentModuleIdAsync(int parentModuleId);
        Task<int?> IsParentModule(int moduleId);
    }
}
