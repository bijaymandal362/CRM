using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class ChangeRequestService : IChangeRequestService
    {
        private readonly CRMDbContext _context;

        public readonly ILogger<ChangeRequestService> _logger;
        private readonly ICommonService _commonService;

        public ChangeRequestService(
            CRMDbContext context,
            ILogger<ChangeRequestService> logger,
            ICommonService commonService
            ) 
        {
            _context = context;
            _logger = logger;
            _commonService = commonService;
        }

        public async Task<ChangeRequest> AddChangeRequestAsync(AddChangeRequestViewModel model)
        {
            var changeRequestData = new ChangeRequest 
            {
                ChangeRequestNumber = model.ChangeRequestNumber,
                ChangeRequestTypeListItemId= model.ChangeRequestTypeListItemId,
                Title = model.Title,
                ClientId = model.ClientId,
                PriorityListItemId = model.PriorityListItemId,
                ModuleId = model.ModuleId,
                Description = model.Description,
                DeadlineDate = model.DeadlineDate,
                InsertPersonId = model.InsertPersonId,
                InsertDate = model.Insertdate,
                Comment = model.Comment,
                
            };

            try
            {
                var changeRequestEntryResult = await _context.ChangeRequest.AddAsync(changeRequestData);
                await _context.SaveChangesAsync();
                return changeRequestEntryResult.Entity;

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                throw;
            }
        }

        public async Task<ChangeRequestInfoViewModel> GetChangeRequestByChangeRequestIdAsync(int changeRequestId)
        {
            var changeRequestInfo = await (from cr in _context.ChangeRequest
                                       join c in _context.Client on
                                            cr.ClientId equals c.ClientId

                                       join li in _context.ListItem on
                                            cr.ChangeRequestTypeListItemId equals li.ListItemId

                                       join m in _context.Module on
                                            cr.ModuleId equals m.ModuleId

                                       join p in _context.Project on
                                            m.ProjectId equals p.ProjectId

                                       join cm in _context.Company on
                                            p.CompanyId equals cm.CompanyId

                                       where cr.ChangeRequestId == changeRequestId

                                       select new 
                                       {
                                           cr.ChangeRequestId,
                                           cr.ChangeRequestNumber,
                                           cr.ChangeRequestTypeListItemId,
                                           cr.PriorityListItemId,
                                           m.ModuleId,
                                           m.ModuleName,
                                           cr.Description,
                                           c.ClientId,
                                           cr.DeadlineDate,
                                           cr.Comment,
                                           cr.Title,
                                           cr.InsertPersonId,
                                           cr.InsertDate,
                                           cr.UpdatePersonId,
                                           cr.UpdateDate
                                       }).FirstAsync();

            var changeRequestTypeSystemName = await _commonService.GetListItemByIdAsync(changeRequestInfo.ChangeRequestTypeListItemId);
            var priorityTypeSystemName = await _commonService.GetListItemByIdAsync(changeRequestInfo.PriorityListItemId);

                var changeRequestInfoViewModel = new ChangeRequestInfoViewModel 
                {
                    ChangeRequestId = changeRequestInfo.ChangeRequestId,
                    ChangeRequestNumber = changeRequestInfo.ChangeRequestNumber,
                    ChangeRequestTypeListItemId = changeRequestInfo.ChangeRequestTypeListItemId,
                    ChangeRequestTypeListItemName = changeRequestTypeSystemName.ListItemSystemName,
                    PriorityListItemId = changeRequestInfo.PriorityListItemId,
                    ListItemPriorityName = priorityTypeSystemName.ListItemSystemName,
                     ModuleId = changeRequestInfo.ModuleId,
                     ModuleName = changeRequestInfo.ModuleName,
                     Description = changeRequestInfo.Description,
                     ClientId = changeRequestInfo.ClientId,
                     DeadlineDate = changeRequestInfo.DeadlineDate,
                     Comment = changeRequestInfo.Comment,
                     Title = changeRequestInfo.Title,
                     InsertPersonId = changeRequestInfo.InsertPersonId,
                     InsertDate = changeRequestInfo.InsertDate,
                     UpdatePersonId = changeRequestInfo.UpdatePersonId,
                     UpdateDate = changeRequestInfo.UpdateDate,
                };

            return changeRequestInfoViewModel;
        }

        public async Task<ChangeRequest> UpdateChangeRequestInfoAsync(UpdateChangeRequestInfoViewModel model)
        {
            try
            {
                var changeRequestData = new ChangeRequest
                {
                    ChangeRequestId = model.ChangeRequestId,
                    Title = model.Title,
                    Description = model.Description,
                    Comment = model.Comment,
                    DeadlineDate = model.DeadlineDate,
                    InsertPersonId = model.InsertPersonId,
                    InsertDate = model.InsertDate,
                    UpdatePersonId = model.UpdatePersonId,
                    UpdateDate = model.UpdateDate,
                    ModuleId = model.ModuleId,
                    ClientId = model.ClientId,
                    ChangeRequestNumber = model.ChangeRequestNumber,
                    ChangeRequestTypeListItemId = model.ChangeRequestTypeListItemId,
                    PriorityListItemId = model.PriorityListItemId
                };

                var result = _context.ChangeRequest.Update(changeRequestData);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetChangeRequestCountByModuleIdAsync(int moduleId) 
        {
            var countChangeRequest = await _context.ChangeRequest
                .Include(c => c.Module)
                .Where(c => c.ModuleId == moduleId)
                .CountAsync();
            return countChangeRequest;
        }

        public async Task<int> CountTotalChangeRequestsAsync() 
        {
            return await _context.ChangeRequest.CountAsync();
        }
    }
}
