using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    public class ProfileController:BaseController
    {
        private readonly IClientService _clientService;

        public ProfileController(IClientService clientService) 
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index() 
        {
            ViewBag.Controller = "Profile";
            if (User.FindFirstValue(ClaimTypes.Role) == "CLIENT") 
            {
                ViewData["CompanyName"] = await _clientService
                                            .GetCompanyNameByClientEmailAsync
                                            (User.FindFirstValue(ClaimTypes.Email).ToString());
            }
            return View();
        }
    }
}
