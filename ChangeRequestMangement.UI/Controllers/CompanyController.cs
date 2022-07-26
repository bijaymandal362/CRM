using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.ViewModels.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _icompanyService;

        public CompanyController(ICompanyService icompanyService)
        {
            _icompanyService = icompanyService;
        }

        public IActionResult Index()
        {
            ViewBag.Controller = "Company";
            return View();
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            ViewBag.Controller = "Company";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _icompanyService.CreateCompanyAsync(model);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Index", "Company");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            if (id != 0)
            {
                ViewBag.Controller = "Company";
                var result = await _icompanyService.GetCompanyByIdAsync(id);
                return View(result);
            }
            else
            {
                throw new Exception("CompanyId is NULL!!");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _icompanyService.UpdateCompanyAsync(model);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Index", "Company");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompany()
        {
            return Json(new { data = await _icompanyService.GetAllCompanyAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (id > 0)
            {
                var result = await _icompanyService.DeleteCompanyAsync(id);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
    }
}