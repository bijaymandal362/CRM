using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.BusinessLayer.Services.Implementation;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize(Roles = "ADMIN, CLIENT")]
    public class TimelineController : BaseController
    {
        private readonly IChangeRequestService _changeRequestService;
        private readonly IChangeRequestStatusService _changeRequestStatusService;
        private readonly IChangeRequestDocumentService _changeRequestDocumentService;

        public TimelineController(
            IChangeRequestService changeRequestService,
            IChangeRequestStatusService changeRequestStatusService,
            IChangeRequestDocumentService changeRequestDocumentService) 
        {
            _changeRequestService = changeRequestService;
            _changeRequestStatusService = changeRequestStatusService;
            _changeRequestDocumentService = changeRequestDocumentService;
        }
        
        public async Task<IActionResult> Index(int id=8)
        {
            ViewBag.Controller = "Timeline";

            if (id != 0) 
            {
                var changeRequestInfoCommonViewModel = new ChangeRequestInfoCommonViewModel();
                changeRequestInfoCommonViewModel.ChangeRequestInfoViewModel = 
                    await _changeRequestService.GetChangeRequestByChangeRequestIdAsync(id);

                changeRequestInfoCommonViewModel.ChangeRequestStatusInfoViewViewModel
                    .AddRange(await _changeRequestStatusService.GetChangeRequestStatusInfoAsync(id));

                changeRequestInfoCommonViewModel.ChangeRequestDocumentInfoViewModel
                    .AddRange(await _changeRequestDocumentService.GetChangeRequestDocumentByChangeRequestId(id));
                return View(changeRequestInfoCommonViewModel);
            }

            return View();
        }
    }
}
