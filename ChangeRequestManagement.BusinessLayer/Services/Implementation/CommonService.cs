using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.ListItem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly CRMDbContext _context;
        private readonly ILogger<CommonService> _logger;

        public CommonService(CRMDbContext context, ILogger<CommonService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ListItemViewModel>> GetListItemByCategoryIdAsync(int listItemCategoryId)
        {
            try
            {
                var listItems = await (from lc in _context.ListItemCategory
                                       join li in _context.ListItem
                                       on lc.ListItemCategoryId equals li.ListItemCategoryId
                                       where lc.ListItemCategoryId == listItemCategoryId
                                       select new ListItemViewModel
                                       {
                                           ListItemId = li.ListItemId,
                                           ListItemName = li.ListItemName,
                                           ListItemSystemName = li.ListItemSystemName
                                       }).ToListAsync();

                return listItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving list item values of category Id: {listItemCategoryId}. Exception message: {ex.Message} ");
                throw;
            }
        }

        public async Task<List<ListItemViewModel>> GetListItemByCategoryNameAsync(string listItemCategoryName)
        {
            try
            {
                var listItems = await (from lc in _context.ListItemCategory
                                       join li in _context.ListItem
                                       on lc.ListItemCategoryId equals li.ListItemCategoryId
                                       where lc.ListItemCategoryName == listItemCategoryName.ToUpper()
                                       select new ListItemViewModel
                                       {
                                           ListItemId = li.ListItemId,
                                           ListItemName = li.ListItemName,
                                           ListItemSystemName = li.ListItemSystemName
                                       }).ToListAsync();

                return listItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving list item values of category name: {listItemCategoryName}. Exception message: {ex.Message} ");
                throw;
            }
        }

        public async Task<ListItemViewModel> GetListItemByIdAsync(int listItemId)
        {
            try
            {
                var listItems = await (from li in _context.ListItem
                                       where li.ListItemId == listItemId
                                       select new ListItemViewModel
                                       {
                                           ListItemId = li.ListItemId,
                                           ListItemName = li.ListItemName,
                                           ListItemSystemName = li.ListItemSystemName
                                       }).FirstOrDefaultAsync();

                return listItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving list item values of list item Id: {listItemId}. Exception message: {ex.Message} ");
                throw;
            }
        }
    }
}