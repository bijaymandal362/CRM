using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.Module;
using ChangeRequestManagment.Models.ViewModels.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class ModuleService : IModuleService
    {
        private readonly CRMDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        private readonly ILogger<ModuleService> _logger;

        public ModuleService(
            CRMDbContext context,
            ILogger<ModuleService> logger,
            IHttpContextAccessor httpContext
            )
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<int> CreateModuleAsync(ModuleViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;

                if (model.ParentModulelId != null)
                {
                    var module = new Module
                    {
                        ModuleName = model.ModuleName.Trim(),
                        ParentModulelId = model.ParentModulelId,
                        ProjectId = model.ProjectId,
                        InsertPersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                        InsertDate = model.InsertDate
                    };
                    var result = await _context.Module.AddAsync(module);
                    await _context.SaveChangesAsync();
                    return result.Entity.ProjectId;
                }
                else
                {
                    Module module = new()
                    {
                        ModuleName = model.ModuleName.Trim().ToUpper(),
                        ProjectId = model.ProjectId,
                        ParentModulelId = null
                    };
                    var result = await _context.AddAsync(module);
                    _context.SaveChanges();
                    return result.Entity.ProjectId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding new module record for moduleId: {model.ModuleId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            try
            {
                if (moduleId > 0)
                {
                    var isFlag = await _context.Module.Where(x => x.ParentModulelId == moduleId).AnyAsync();
                    if (!isFlag)
                    {
                        var module = await _context.Module.FindAsync(moduleId);
                        _context.Module.Remove(module);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception("Module can't be deleted.");
            }
        }

        public async Task<IEnumerable<ModuleViewModel>> GetAllModuleAsync()
        {
            try
            {
                var result = await (from m in _context.Module
                                    select new ModuleViewModel
                                    {
                                        ModuleId = m.ModuleId,
                                        ModuleName = m.ModuleName
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching all module records. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<ModuleViewModel> GetModuleByIdAsync(int moduleId)
        {
            try
            {
                if (moduleId != 0)
                {
                    var result = await (from m in _context.Module
                                        join p in _context.Project on
                                        m.ProjectId equals p.ProjectId
                                        where m.ModuleId == moduleId
                                        select new ModuleViewModel
                                        {
                                            ModuleId = m.ModuleId,
                                            ModuleName = m.ModuleName,
                                            ProjectId = p.ProjectId,
                                            ParentModulelId = m.ParentModulelId
                                        }).FirstOrDefaultAsync();
                    return result;
                }
                else
                {
                    throw new Exception("Module id is NULL!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching the module record by moduleId: {moduleId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProjectViewModel>> GetProjectDropDownAsync()
        {
            var result = await (from p in _context.Project
                                select new ProjectViewModel
                                {
                                    ProjectId = p.ProjectId,
                                    ProjectName = p.ProjectName
                                }).ToListAsync();

            result.Insert(0, new ProjectViewModel { ProjectId = 0, ProjectName = "--Select--" });
            return result;
        }

        public async Task<List<ModuleViewModel>> GetModuleByProjectIdAsync(int projectId)
        {
            try
            {
                var result = await (from m in _context.Module
                                    join p in _context.Project on
                                    m.ProjectId equals p.ProjectId
                                    where p.ProjectId == projectId
                                    select new
                                    {
                                        m.ModuleId,
                                        m.ModuleName,
                                        p.ProjectId,
                                        m.ParentModulelId
                                    }).ToListAsync();

                var list = new List<ModuleViewModel>();
                foreach (var item in result)
                {
                    if (item.ParentModulelId == null)
                    {
                        list.Add(new ModuleViewModel
                        {
                            ProjectId = item.ProjectId,
                            ParentModulelId = item.ParentModulelId,
                            ModuleId = item.ModuleId,
                            ModuleName = item.ModuleName
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching module records for projectId: {projectId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<int?> UpdateModuleAsync(ModuleViewModel model)
        {
            try
            {
                var module = await _context.Module.FirstOrDefaultAsync(m => m.ModuleId == model.ModuleId);
                _context.Entry(module).State = EntityState.Detached;

                ClaimsPrincipal cp = _httpContext.HttpContext.User;
                if (model.ModuleId == module.ModuleId)
                {
                    var result = _context.Module.Update(new Module
                    {
                        ModuleId = model.ModuleId,
                        ModuleName = model.ModuleName.Trim(),
                        ProjectId = model.ProjectId,
                        ParentModulelId = model.ParentModulelId,
                        InsertPersonId = module.InsertPersonId,
                        InsertDate = module.InsertDate,
                        UpdatePersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                        UpdateDate = model.UpdateDate
                    });
                    _context.SaveChanges();
                    var a = result.Entity.ParentModulelId;
                    return a;
                }
                else
                {
                    throw new Exception($"Record for moduleId: {model.ModuleId} doesn't exists in our database!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating module record. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ModuleViewModel>> GetSubModuleByParentModuleIdAsync(int moduleId)
        {
            try
            {
                var subModuleInfo = await (from m in _context.Module
                                           join p in _context.Project on
                                           m.ProjectId equals p.ProjectId
                                           where m.ParentModulelId == moduleId
                                           select new ModuleViewModel
                                           {
                                               ModuleId = m.ModuleId,
                                               ModuleName = m.ModuleName,
                                               ProjectId = p.ProjectId,
                                               ParentModulelId = m.ParentModulelId
                                           }).ToListAsync();
                return subModuleInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching submodule record. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetProjectIdByParentModuleIdAsync(int parentModuleId)
        {
            try
            {
                var result = await _context.Module
                    .Include(x => x.Project)
                    .Where(x=>x.ModuleId == parentModuleId)
                    .Select(x=>x.ProjectId ).FirstAsync(); ;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching projectId with {parentModuleId}. Exception message: {ex.Message} ");
                throw;
            }
        }

        public async Task<int?> IsParentModule(int moduleId) 
        {
            try
            {
                var result = await _context.Module
                    .Where(x => x.ModuleId == moduleId).FirstAsync();
                return result.ParentModulelId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured. Exception message: {ex.Message} ");
                throw;
            }
        }
    }
}