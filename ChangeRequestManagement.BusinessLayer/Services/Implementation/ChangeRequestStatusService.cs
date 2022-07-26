using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ListItemAndListItemCategoryEnum;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class ChangeRequestStatusService: IChangeRequestStatusService
    {
        private readonly CRMDbContext _context;
        private readonly ILogger<ChangeRequestStatusService> _logger;

        public ChangeRequestStatusService(
            CRMDbContext context,
            ILogger<ChangeRequestStatusService> logger
            ) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ChangeRequestStatus> AddChangeRequestStatusAsync(AddChangeRequestStatusViewModel model) 
        {
            try
            {
                var changeRequestData = new ChangeRequestStatus
                {
                    ChangeRequestId = model.ChangeRequestId,
                    ChangeRequestStatusListItemId = model.ChangeRequestStatusListItemId,
                    Comment = model.Comment,
                    InsertPersonId = model.InsertPersonId,
                    InsertDate = model.InsertDate
                };

                var addChangeRequestResult = await _context.ChangeRequestStatus.AddAsync(changeRequestData);

                await _context.SaveChangesAsync();
                return addChangeRequestResult.Entity;

            }catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ChangeRequestStatusInfoViewModel>> GetChangeRequestStatusInfoAsync(int changeRequestId) 
        {
            try
            {
                var changeRequestStatusData = await (from crs in _context.ChangeRequestStatus

                                              join cr in _context.ChangeRequest on
                                              crs.ChangeRequestId equals cr.ChangeRequestId

                                              join li in _context.ListItem on
                                              crs.ChangeRequestStatusListItemId equals li.ListItemId

                                              orderby crs.InsertDate ascending

                                              where crs.ChangeRequestId == changeRequestId

                                              select new 
                                              {
                                                  crs.ChangeRequestStatusId,
                                                  crs.ChangeRequestId,
                                                  crs.ChangeRequestStatusListItemId,
                                                  li.ListItemName,
                                                  crs.Comment,
                                                  crs.InsertPersonId,
                                                  crs.InsertDate,
                                                  crs.UpdatePersonId,
                                                  crs.UpdateDate
                                              }).ToListAsync();

                var changeRequestStatusInfoViewModelList = new List<ChangeRequestStatusInfoViewModel>();

                foreach (var item in changeRequestStatusData) 
                {
                    var insertPerson = await _context.Person.FindAsync(item.InsertPersonId);
                    var updatePerson = await _context.Person.FindAsync(item.UpdatePersonId);
                    changeRequestStatusInfoViewModelList.Add(new ChangeRequestStatusInfoViewModel 
                    {
                        ChangeRequestStatusId = item.ChangeRequestStatusId,
                        ChangeRequestId = item.ChangeRequestId,
                        ChangeRequestStatusListItemId = item.ChangeRequestStatusListItemId,
                        ListItemName = item.ListItemName,
                        Comment = item.Comment,
                        InsertPersonId = item.InsertPersonId,
                        InsertPersonFirstName = insertPerson.FirstName,
                        InsertPersonLastName = insertPerson.LastName,
                        InsertDate = item.InsertDate,
                        UpdatePersonFirstName = updatePerson?.FirstName,
                        UpdatePersonLastName = updatePerson?.LastName,
                        UpdateDate = item.UpdateDate
                    });
                }

                return changeRequestStatusInfoViewModelList;

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CountTotalPendingChangeRequestsAsync() 
        {
            var count = 0;
            var ChangeRequestIdList =  await _context.ChangeRequestStatus
                .Select(x=>x.ChangeRequestId)
                .Distinct().ToListAsync();
            foreach (var item in ChangeRequestIdList)
            {
                var result = await _context.ChangeRequestStatus
                    .Where(x => x.ChangeRequestId == item)
                    .OrderByDescending(x => x.InsertDate).FirstAsync();
                if (result.ChangeRequestStatusListItemId == (int)ChangeRequestStatusEnum.Pending) 
                {
                    count++;
                }
            }
            return count;
        }
        public async Task<int> CountTotalApprovedChangeRequestsAsync()
        {
            var count = 0;
            var ChangeRequestIdList = await _context.ChangeRequestStatus
                .Select(x => x.ChangeRequestId)
                .Distinct().ToListAsync();
            foreach (var item in ChangeRequestIdList)
            {
                var result = await _context.ChangeRequestStatus
                    .Where(x => x.ChangeRequestId == item)
                    .OrderByDescending(x => x.InsertDate).FirstAsync();
                if (result.ChangeRequestStatusListItemId == (int)ChangeRequestStatusEnum.Approved)
                {
                    count++;
                }
                else if(result.ChangeRequestStatusListItemId != (int)ChangeRequestStatusEnum.Approved &&
                    result.ChangeRequestStatusListItemId != (int)ChangeRequestStatusEnum.Pending) 
                {
                    count++;
                }
            }
            return count;
        }
    }
}
