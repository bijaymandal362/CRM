using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Api
{
    [Route("api")]
    public class GeneralListItemController : Controller
    {
        private readonly ICommonService _icommonService;

        public GeneralListItemController(ICommonService icommonService)
        {
            _icommonService = icommonService;
        }

        [HttpGet("GetListItemByCategoryName")]
        public async Task<IActionResult> GetListItemByCategoryName(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                var result = await _icommonService.GetListItemByCategoryNameAsync(categoryName);
                return Ok(result);
            }
            else
            {
                throw new ArgumentException($"'{nameof(categoryName)}' cannot be null or empty.", nameof(categoryName));
            }
        }

        [HttpGet("GetListItemByCategoryId")]
        public async Task<IActionResult> GetListItemByCategoryId(int categoryId)
        {
            if (categoryId > 0)
            {
                var result = await _icommonService.GetListItemByCategoryIdAsync(categoryId);
                return Ok(result);
            }
            else
            {
                throw new ArgumentException($"'{nameof(categoryId)}' cannot be null.", nameof(categoryId));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListItemById(int listItemId)
        {
            if (listItemId > 0)
            {
                var result = await _icommonService.GetListItemByIdAsync(listItemId);
                return View(result);
            }
            else
            {
                throw new ArgumentException($"'{nameof(listItemId)}' cannot be null.", nameof(listItemId));
            }
        }
    }
}