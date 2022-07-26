using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.DataTableProperty;
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

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    internal class ProjectService : IProjectService
    {
        private readonly CRMDbContext _context;
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(CRMDbContext context,
            IClientService clientService,
            ILogger<ProjectService> logger,
            IHttpContextAccessor httpContext
            )
        {
            _context = context;
            _httpContext = httpContext;
            _clientService = clientService;
            _logger = logger;
        }

        public async Task DeleteProjectAsync(int id)
        {
            try
            {
                var deleteProjectAsync = await _context.Project.FindAsync(id);
                if (deleteProjectAsync != null)
                {
                    _context.Remove(deleteProjectAsync);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Record with projectId: {id} does not exists!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting the record for projectId: {id}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetProjectAsync(DataTablePropertyViewModel model)
        {
            try
            {
                var getProjectAsync = _context.Project.Include(x => x.Company)
                    .Select(x => new
                    {
                        x.CompanyId,
                        x.Company.CompanyName,
                        x.ProjectId,
                        x.ProjectName,
                        x.CreatedDate
                    });

                if (!(string.IsNullOrEmpty(model.SortColumn) && string.IsNullOrEmpty(model.SortColumnDirection)))
                {
                    getProjectAsync = getProjectAsync.OrderBy(model.SortColumn + " " + model.SortColumnDirection);
                }

                if (!string.IsNullOrEmpty(model.SearchValue))
                {
                    getProjectAsync = getProjectAsync.Where
                        (
                            x =>
                            x.ProjectName.ToLower().Contains(model.SearchValue.ToLower()) ||
                            x.CompanyName.ToLower().Contains(model.SearchValue.ToLower()) ||
                            x.CreatedDate.ToString().Contains(model.SearchValue.ToLower())
                        );
                }
                int recordsTotal = getProjectAsync.Count();
                var data = await getProjectAsync.Skip(model.Skip).Take(model.PageSize).ToListAsync();

                object jsonData = new
                {
                    draw = model.Draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching information. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProjectViewModel>> GetAllProjectAsync()
        {
            try
            {
                var projectList = await _context.Project.ToListAsync();
                var projectViewModelList = new List<ProjectViewModel>();
                foreach (var item in projectList)
                {
                    projectViewModelList.Add(new ProjectViewModel
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName
                    });
                }
                return projectViewModelList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<ProjectViewModel> GetProjectByIdAsync(int projectId)
        {
            try
            {
                if (projectId != 0)
                {
                    var getProjectByIdAsync = await (from p in _context.Project
                                                     where p.ProjectId == projectId
                                                     select new ProjectViewModel
                                                     {
                                                         ProjectId = p.ProjectId,
                                                         ProjectName = p.ProjectName,
                                                         CompanyId = p.CompanyId,
                                                         CreatedDate = p.CreatedDate
                                                     }).FirstOrDefaultAsync();
                    return getProjectByIdAsync;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching records for projectId: {projectId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProjectViewModel>> GetProjectByCompanyIdAsync(int companyId) 
        {
            try
            {
                if (companyId != 0)
                {
                    var getProjectByIdAsync = await (from p in _context.Project
                                                     join c in _context.Company on
                                                     p.CompanyId equals c.CompanyId
                                                     where p.CompanyId == companyId
                                                     orderby p.ProjectName
                                                     select new ProjectViewModel
                                                     {
                                                         CompanyId = p.CompanyId,
                                                         CompanyName = c.CompanyName,
                                                         ProjectId = p.ProjectId,
                                                         ProjectName = p.ProjectName
                                                     }).ToListAsync(); ;
                    return getProjectByIdAsync;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AddProjectAsync(ProjectViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;

                var checkIfProjectExists = await _context.Project
                    .Where(x => x.ProjectName.Trim().ToUpper() == model.ProjectName.Trim().ToUpper())
                    .Where(x => x.CompanyId == model.CompanyId)
                    .AnyAsync();
                if (checkIfProjectExists)
                {
                    return false;
                }
                else
                {
                    if (model.ProjectId == 0)
                    {
                        Project project = new Project();
                        project.ProjectName = model.ProjectName.ToUpper();
                        project.CompanyId = model.CompanyId;
                        project.CreatedDate = model.CreatedDate;
                        project.InsertPersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value);
                        project.InsertDate = model.InsertDate;

                        await _context.AddAsync(project);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding new record!. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProjectAsync(ProjectViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;
                var project = await _context.Project.FirstAsync(a => a.ProjectId == model.ProjectId);
                _context.Entry(project).State = EntityState.Detached;

                if (model.ProjectId == project.ProjectId)
                {
                    _context.Project.Update(new Project
                    {
                        ProjectId = model.ProjectId,
                        ProjectName = model.ProjectName,
                        CompanyId = model.CompanyId,
                        InsertPersonId = project.InsertPersonId,
                        InsertDate = project.InsertDate,
                        UpdatePersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                        UpdateDate = model.UpdateDate
                    });

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Record for ProjectId: {model.ProjectId} doesn't exists!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating record of projectId: {model.ProjectId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CountTotalProjectsAsync() 
        {
            return await _context.Project.CountAsync();
        }
    }
}