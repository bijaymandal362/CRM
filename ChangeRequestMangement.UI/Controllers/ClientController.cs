using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ClientAdditionValidationEnum;

namespace ChangeRequestMangement.UI.Controllers
{

    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly ICompanyService _companyService;

        public ClientController(
            IClientService clientService,
            ICompanyService companyService)
        {
            _clientService = clientService;
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.Controller = "Client";
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="CLIENT")]
        public IActionResult ViewStatus() 
        {
            ViewBag.Controller = "Client";
            return View();
        }

        [Route("[Controller]/GetClientInfo")]
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetClientInfo()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            if (sortColumn == "Id")
                sortColumn = "FirstName";
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            var data = await _clientService.GetAllClientInfoAsync(new ClientDataTableHelper
            {
                Draw = int.Parse(draw),
                Start = start,
                SortColumn = sortColumn,
                Length = length,
                SortColumnDirection = sortColumnDirection,
                SearchValue = searchValue
            });
            return Json(data);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddClient()
        {
            ViewBag.Controller = "Client";
            ViewData["CompanyInfoFromClient"] = await _companyService.GetAllCompanyAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddClient(AddClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _clientService.AddClientAsync(model);
                if (!result.IsSucces)
                {
                    ViewData["CompanyInfoFromClient"] = await _companyService.GetAllCompanyAsync();
                    if (result.ErrorType == ClientError.DuplicateEmailError)
                    {
                        ModelState.AddModelError("", "The provided email is already registered");
                        return View(model);
                    }
                    if (result.ErrorType == ClientError.ClientAddError)
                    {
                        ModelState.AddModelError("", "Could not add client, please try again");
                        return View(model);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Controller = "Client";
                ViewData["CompanyInfoFromClient"] = await _companyService.GetAllCompanyAsync();
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateClient(int id)
        {
            ViewBag.Controller = "Client";
            ViewData["CompanyInfoFromClient"] = await _companyService.GetAllCompanyAsync();
            var clientInfo = await _clientService.GetClientInfoAsync(id);

            return View(new UpdateClientInfoViewModel
            {
                PersonId = clientInfo.PersonId,
                ClientId = clientInfo.ClientId,
                FirstName = clientInfo.FirstName,
                LastName = clientInfo.LastName,
                Address = clientInfo.Address,
                EmailAddress = clientInfo.EmailAddress,
                CompanyId = clientInfo.CompanyId,
                PhoneNumber = clientInfo.PhoneNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateClient(UpdateClientInfoViewModel model)
        {
            ViewData["CompanyInfoFromClient"] = await _companyService.GetAllCompanyAsync();
            if (ModelState.IsValid)
            {
                await _clientService.UpdateClientInfoAsync(model);
                return RedirectToAction(nameof(Index));
            }
            else
                return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            bool success = await _clientService.RemoveClientAsync(id);
            return Ok(new { success });
        }

        [HttpGet]
        public async Task<IActionResult> GetChangeRequests()
        {
            return Json(new { data = await _clientService.GetChangeRequestsAsync(User.FindFirstValue(ClaimTypes.Email)) });
        }
    }
}