using ChangeRequestManagment.Models.ViewModels.ListItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface ICommonService
    {
        Task<List<ListItemViewModel>> GetListItemByCategoryNameAsync(string listItemCategoryName);

        Task<List<ListItemViewModel>> GetListItemByCategoryIdAsync(int listItemCategoryId);

        Task<ListItemViewModel> GetListItemByIdAsync(int listItemId);
    }
}