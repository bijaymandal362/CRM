using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.Client;
using ChangeRequestManagment.Models.ViewModels.ClientRequestStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ClientAdditionValidationEnum;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly CRMDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<ClientService> _logger;
        private readonly IRoleService _roleService;
        private readonly ICompanyService _companyService;

        public ClientService(
            CRMDbContext context,
            ILogger<ClientService> logger,
            IRoleService roleService,
            ICompanyService companyService,
            IHttpContextAccessor httpContext
            )
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
            _roleService = roleService;
            _companyService = companyService;
        }

        public async Task<AddClientSuccessViewModel> AddClientAsync(AddClientViewModel model)
        {
            var ifEmailExists = await _context.Person
                .Where(x => x.EmailAddress == model.EmailAddress).AnyAsync() ||
                await _context.Client.Where(x => x.Person.EmailAddress == model.EmailAddress).AnyAsync();
            if (ifEmailExists == true)
            {
                _logger.LogWarning($"Client tried to register email address which already exists. Email Address: {model.EmailAddress}");
                return new AddClientSuccessViewModel
                {
                    IsSucces = false,
                    ErrorType = ClientError.DuplicateEmailError
                };
            }
            ClaimsPrincipal cp = _httpContext.HttpContext.User;

            var data = new Person
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                RoleId = await _roleService.GetRoleIdByRoleNameAsync("CLIENT"),
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                InsertPersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                InsertDate = model.InsertDate
            };

            try
            {
                var addPersonResult = await _context.Person.AddAsync(data);
                await _context.SaveChangesAsync();

                var addPersonToClientResult = _context.Client.AddAsync(new Client
                {
                    PersonId = addPersonResult.Entity.PersonId,
                    CompanyId = model.CompanyId,
                    InsertPersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                    InsertDate = model.InsertDate
                });
                await _context.SaveChangesAsync();

                return new AddClientSuccessViewModel
                {
                    IsSucces = true,
                    ErrorType = ClientError.NoAnyError
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not add client to the database. Exception message is: ", ex.Message);
                return new AddClientSuccessViewModel
                {
                    IsSucces = false,
                    ErrorType = ClientError.ClientAddError
                };
            }
        }

        public async Task<ClientInfoViewModel> GetClientInfoAsync(int id)
        {
            var clientInfo = await _context.Client
                .Include(x => x.Company)
                .Include(x => x.Person)
                .Include(x => x.Person.Role)
                .Where(x => x.ClientId == id)
                .Select
                (x => new
                {
                    x.Person.PersonId,
                    x.Person.FirstName,
                    x.Person.LastName,
                    x.Person.Address,
                    x.Person.EmailAddress,
                    x.Person.PhoneNumber,
                    x.Company.CompanyId,
                    x.Company.CompanyName,
                    x.Person.Role.RoleId,
                    x.Person.Role.RoleName,
                    x.ClientId
                }
                ).FirstAsync();

            return new ClientInfoViewModel
            {
                PersonId = clientInfo.PersonId,
                ClientId = clientInfo.ClientId,
                FirstName = clientInfo.FirstName,
                LastName = clientInfo.LastName,
                Address = clientInfo.Address,
                EmailAddress = clientInfo.EmailAddress,
                PhoneNumber = clientInfo.PhoneNumber,
                CompanyId = clientInfo.CompanyId,
                CompanyName = clientInfo.CompanyName,
                RoleId = clientInfo.RoleId,
                RoleName = clientInfo.RoleName
            };
        }

        public async Task<int> GetClientIdByPersonIdAsync(int personId)
        {
            var client = await _context.Client
                                .Where(x => x.PersonId == personId)
                                .FirstAsync();
            return client.ClientId;
        }

        public async Task<string> GetCompanyNameByClientEmailAsync(string email)
        {
            var companyIdByClientEmail = await _context.Client
                .Include(x => x.Company)
                .Where(x => x.Person.EmailAddress == email)
                .Select(x => new { x.CompanyId })
                .FirstAsync();

            var company = await _companyService.GetCompanyByIdAsync(companyIdByClientEmail.CompanyId);

            return company.CompanyName;
        }

        public async Task<object> GetAllClientInfoAsync(ClientDataTableHelper tableHelper)
        {
            var clientList = _context.Client
                .Include(x => x.Company)
                .Include(x => x.Person)
                .Select(x => new
                {
                    x.ClientId,
                    x.Person.FirstName,
                    x.Person.LastName,
                    x.Person.Address,
                    x.Person.PhoneNumber,
                    x.Person.EmailAddress,
                    x.Company.CompanyName
                });

            if (!string.IsNullOrEmpty(tableHelper.SortColumn) && !string.IsNullOrEmpty(tableHelper.SortColumnDirection))
            {
                clientList = clientList.OrderBy(tableHelper.SortColumn + " " + tableHelper.SortColumnDirection);
            }
            if (!string.IsNullOrEmpty(tableHelper.SearchValue))
            {
                clientList = clientList.Where
                    (
                        x =>
                        x.FirstName.ToLower().Contains(tableHelper.SearchValue.ToLower()) ||
                        x.LastName.ToLower().Contains(tableHelper.SearchValue.ToLower()) ||
                        x.Address.ToLower().Contains(tableHelper.SearchValue.ToLower()) ||
                        x.EmailAddress.ToLower().Contains(tableHelper.SearchValue.ToLower()) ||
                        x.PhoneNumber.ToLower().Contains(tableHelper.SearchValue.ToLower()) ||
                        x.CompanyName.ToLower().Contains(tableHelper.SearchValue.ToLower())
                    );
            }

            int pageSize = tableHelper.Length != null ? Convert.ToInt32(tableHelper.Length) : 0;
            int skip = tableHelper.Start != null ? Convert.ToInt32(tableHelper.Start) : 0;

            int totalRecords = clientList.Count();
            var data = await clientList.Skip(skip).Take(pageSize).ToListAsync();
            object jsonData = new
            {
                draw = tableHelper.Draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = data
            };

            return jsonData;
        }

        public async Task<bool> UpdateClientInfoAsync(UpdateClientInfoViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;
                var client = await _context.Client.FirstOrDefaultAsync(x => x.ClientId == model.ClientId);
                _context.Entry(client).State = EntityState.Detached;

                var personResult = _context.Person.Update(new Person
                {
                    PersonId = model.PersonId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    Password = _context.Person.FirstAsync().Result.Password,
                    RoleId = await _roleService.GetRoleIdByRoleNameAsync("CLIENT"),
                    InsertPersonId = client.InsertPersonId,
                    InsertDate = client.InsertDate,
                    UpdatePersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                    UpdateDate = model.UpdateDate
                });
                await _context.SaveChangesAsync();

                _context.Client.Update(new Client
                {
                    ClientId = model.ClientId,
                    PersonId = personResult.Entity.PersonId,
                    CompanyId = model.CompanyId,
                    UpdatePersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                    UpdateDate = model.UpdateDate
                });
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating client. The exception message is: ", ex.Message);
                return false;
            }
        }

        public async Task<bool> RemoveClientAsync(int id)
        {
            try
            {
                _context.Client.Remove(await _context.Client.FindAsync(id));
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While deleting record. Exception Message: ", ex.Message);
                return false;
            }
        }

        public async Task<List<ClientRequestStatusViewModel>> GetChangeRequestsAsync(string email)
        {

            var getCompanyId = await _companyService.GetCompanyIdByClientEmailAsync(email);

            var changeRequestsIdLists = await (from cr in _context.ChangeRequest
                                            join crs in _context.ChangeRequestStatus on
                                            cr.ChangeRequestId equals crs.ChangeRequestId

                                            join m in _context.Module on
                                            cr.ModuleId equals m.ModuleId

                                            join pr in _context.Project on
                                            m.ProjectId equals pr.ProjectId

                                            join c in _context.Company on
                                            pr.CompanyId equals c.CompanyId

                                            where pr.CompanyId == getCompanyId

                                            select cr.ChangeRequestId
                                            ).Distinct().ToListAsync();

            var changeRequestsInfoList = new List<ClientRequestStatusViewModel>();

            foreach (var item in changeRequestsIdLists) 
            {
                var changeRequestsInfo = await (from cr in _context.ChangeRequest
                                                join crs in _context.ChangeRequestStatus on
                                                cr.ChangeRequestId equals crs.ChangeRequestId

                                                join li in _context.ListItem on
                                                crs.ChangeRequestStatusListItemId equals li.ListItemId

                                                join m in _context.Module on
                                                cr.ModuleId equals m.ModuleId

                                                join pr in _context.Project on
                                                m.ProjectId equals pr.ProjectId

                                                join c in _context.Company on
                                                pr.CompanyId equals c.CompanyId

                                                where pr.CompanyId == getCompanyId

                                                where crs.ChangeRequestId == item

                                                orderby crs.InsertDate descending

                                                select new ClientRequestStatusViewModel
                                                {
                                                    ChangeRequestId = cr.ChangeRequestId,
                                                    ProjectName = pr.ProjectName,
                                                    ModuleName = m.ModuleName,
                                                    IssueDate = cr.InsertDate,
                                                    DeadlineDate = cr.DeadlineDate,
                                                    Status = li.ListItemSystemName,
                                                    ChangeRequestNumber = cr.ChangeRequestNumber
                                                }
                                            ).FirstAsync();

                changeRequestsInfoList.Add(changeRequestsInfo);
            }

            return changeRequestsInfoList;
        }

        public async Task<int> CountTotalClientsAsync() 
        {
            return await _context.Client.CountAsync();
        }
    }
}