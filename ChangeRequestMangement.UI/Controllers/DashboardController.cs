using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize]
    public class DashboardController:BaseController
    {
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        private readonly IClientService _clientService;
        private readonly IChangeRequestService _changeRequestService;
        private readonly IChangeRequestStatusService _changeRequestStatusService;

        public DashboardController(
            IProjectService projectService,
            ICompanyService companyService,
            IClientService clientService,
            IChangeRequestService changeRequestService,
            IChangeRequestStatusService changeRequestStatusService) 
        {
            _projectService = projectService;
            _companyService = companyService;
            _clientService = clientService;
            _changeRequestService = changeRequestService;
            _changeRequestStatusService = changeRequestStatusService;
        }

        public async Task<IActionResult> Index() 
        {

            if (User.FindFirstValue(ClaimTypes.Role) == "CLIENT") 
            {
                return RedirectToAction("ViewStatus", "Client");
            }

            ViewBag.Controller = "Dashboard";
            ViewData["TotalCompanies"] = await _companyService.CountTotalCompaniesAsync();
            ViewData["TotalProjects"] = await _projectService.CountTotalProjectsAsync();
            ViewData["TotalClients"] = await _clientService.CountTotalClientsAsync();
            ViewData["TotalChangeRequests"] = await _changeRequestService
                                                .CountTotalChangeRequestsAsync();
            ViewData["TotalPendingChangeRequests"] = await _changeRequestStatusService
                                                        .CountTotalPendingChangeRequestsAsync();
            @ViewData["TotalApprovedChangeRequests"] = await _changeRequestStatusService
                                                            .CountTotalApprovedChangeRequestsAsync();
            return View();
        }
    }
}
