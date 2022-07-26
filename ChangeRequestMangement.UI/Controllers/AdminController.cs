using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.ViewModels.ClientRequestStatus;
using ChangeRequestManagment.Models.ViewModels.ListItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ListItemAndListItemCategoryEnum;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : BaseController
    {
        private readonly IAdminService _iadminService;
        private readonly ICommonService _icommonService;
        private readonly IChangeRequestStatusService _changeRequestStatusService;

        public AdminController(IAdminService iadminService, 
            ICommonService icommonService,
            IChangeRequestStatusService changeRequestStatusService)
        {
            _iadminService = iadminService;
            _icommonService = icommonService;
            _changeRequestStatusService = changeRequestStatusService;
        }

        public IActionResult ViewStatus()
        {
            ViewBag.Controller = "Admin";
            return View();
        }

        [HttpGet]
        public async Task<List<ListItemViewModel>>  ChangeRequestStatusList(int id) 
        {
            var list = await _icommonService.GetListItemByCategoryIdAsync((int)ListItemCategoryEnum.ChangeRequestStatus);
            return list;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _iadminService.GetClientRequests() });
        }
    }
}