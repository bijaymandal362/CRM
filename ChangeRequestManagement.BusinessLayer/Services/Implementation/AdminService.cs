using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.ClientRequestStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly CRMDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<AdminService> _logger;

        public AdminService(CRMDbContext context, ILogger<AdminService> logger, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<List<ClientRequestStatusViewModel>> GetClientRequests()
        {
            var changeRequestIdLists = await _context.ChangeRequestStatus.Select(x => x.ChangeRequestId)
                .Distinct()
                .ToListAsync();

            var clientRequestStatusViewModel = new List<ClientRequestStatusViewModel>();

            foreach (var item in changeRequestIdLists) 
            {
                var clientRequestList = await (from cr in _context.ChangeRequest
                                               join crs in _context.ChangeRequestStatus on
                                               cr.ChangeRequestId equals crs.ChangeRequestId

                                               join li in _context.ListItem on
                                               crs.ChangeRequestStatusListItemId equals li.ListItemId

                                               join c in _context.Client on
                                               cr.ClientId equals c.ClientId

                                               join m in _context.Module on
                                               cr.ModuleId equals m.ModuleId

                                               join pr in _context.Project on
                                               m.ProjectId equals pr.ProjectId

                                               join p in _context.Person on
                                               c.PersonId equals p.PersonId

                                               where crs.ChangeRequestId == item

                                               orderby crs.InsertDate descending

                                               select new
                                               {
                                                   cr.ChangeRequestId,
                                                   crs.ChangeRequestStatusId,
                                                   crs.ChangeRequestStatusListItemId,
                                                   cr.ModuleId,
                                                   cr.ClientId,
                                                   cr.ChangeRequestNumber,
                                                   cr.InsertDate,
                                                   cr.DeadlineDate,
                                                   Status = li.ListItemSystemName
                                               }).FirstAsync();

                var personName = await (from c in _context.Client
                                        join p in _context.Person on
                                        c.PersonId equals p.PersonId
                                        where c.ClientId == clientRequestList.ClientId
                                        select new { p.FirstName, p.LastName }).FirstAsync();

                var projectAndModule = await (from m in _context.Module
                                              join p in _context.Project on
                                              m.ProjectId equals p.ProjectId
                                              where m.ModuleId == clientRequestList.ModuleId
                                              select new { p.ProjectName, m.ModuleName }).FirstAsync();

                clientRequestStatusViewModel.Add(new ClientRequestStatusViewModel
                {
                    ChangeRequestStatusId = clientRequestList.ChangeRequestStatusId,
                    ChangeRequestId = clientRequestList.ChangeRequestId,
                    ClientName = personName.FirstName + " " + personName.LastName,
                    ProjectName = projectAndModule.ProjectName,
                    ModuleName = projectAndModule.ModuleName,
                    IssueDate = clientRequestList.InsertDate,
                    DeadlineDate = clientRequestList.DeadlineDate,
                    Status = clientRequestList.Status,
                    ChangeRequestNumber = clientRequestList.ChangeRequestNumber
                });
            }

            return clientRequestStatusViewModel;
        }
    }
}
