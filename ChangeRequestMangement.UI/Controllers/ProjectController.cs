using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.ViewModels.DataTableProperty;
using ChangeRequestManagment.Models.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _iProjectService;
        private readonly ICompanyService _icompanyService;

        public ProjectController(IProjectService iProjectService, ICompanyService icompanyService)
        {
            _iProjectService = iProjectService;
            _icompanyService = icompanyService;
        }

        public IActionResult Index()
        {
            ViewBag.Controller = "Project";
            return View();
        }

        public async Task GetCompanyDropDown()
        {
            var getCompanyDropDown = await _icompanyService.GetCompanyDropDownAsync();
            ViewBag.CompanyDropDown = getCompanyDropDown;
        }

        [HttpPost]
        public async Task<IActionResult> GetProject()
        {
            DataTablePropertyViewModel model = new();
            model.Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            model.Start = Request.Form["start"].FirstOrDefault();
            model.Length = Request.Form["length"].FirstOrDefault();
            model.SortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() +
                                          "][name]"].FirstOrDefault();
            model.SortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            model.SearchValue = Request.Form["search[value]"].FirstOrDefault();
            var length = model.Length;
            var start = model.Start;
            model.PageSize = length != null ? Convert.ToInt32(length) : 0;
            model.Skip = start != null ? Convert.ToInt32(start) : 0;

            var getProjectAsync = await _iProjectService.GetProjectAsync(model);

            return Json(getProjectAsync);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProject() 
        {
            return Json(new { data = await _iProjectService.GetAllProjectAsync()});
        }


        public async Task<IActionResult> AddProject()
        {
            await GetCompanyDropDown();
            ViewBag.Controller = "Project";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProject(ProjectViewModel model)
        {
            await GetCompanyDropDown();
            ViewBag.Controller = "Project";

            if (ModelState.IsValid && model.CompanyId != 0)
            {
                await _iProjectService.AddProjectAsync(model);

                return RedirectToAction("Index", "Project");
            }
            else
            {
                if (model.CompanyId == 0)
                {
                    await GetCompanyDropDown();
                    ModelState.AddModelError("", "Select Company Name");
                    return View(model);
                }
                else
                {
                    await GetCompanyDropDown();
                    ModelState.AddModelError("", "Please enter the required filed");
                    return View(model);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            if (id != 0)
            {
                ViewBag.Controller = "Project";
                var result = await _iProjectService.GetProjectByIdAsync(id);
                await GetCompanyDropDown();
                return View(result);
            }
            else
            {
                throw new Exception("ProjectId is NULL!!");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await GetCompanyDropDown();
                await _iProjectService.UpdateProjectAsync(model);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Index", "Project");
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteProject(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _iProjectService.DeleteProjectAsync(id);
                    return Json(new { success = true, message = "Delete Sucessfully" });
                }
                return Json(new { success = false, message = "Delete Failed" });
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}