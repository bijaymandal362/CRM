using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.ViewModels.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ModuleController : BaseController
    {
        private readonly IModuleService _imoduleService;
        private readonly IProjectService _projectService;

        public ModuleController(IModuleService imoduleService, IProjectService projectService)
        {
            _imoduleService = imoduleService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index(int id = 0)
        {
            ViewData["PreSelectProject"] = id;

            ViewBag.Controller = "Module";
            await LoadProjectsDetails();
            return View();
        }

        private async Task SetProjectName(int projectId)
        {
            var projectResult = await _projectService.GetProjectByIdAsync(projectId);

            ViewData["ProjectNameFromModule"] = projectResult.ProjectName;
        }

        [HttpGet]
        public async Task<IActionResult> AddModule(int id)
        {
            ViewBag.Controller = "Module";
            ViewData["ProjectIdFromModuleController"] = id;
            await SetProjectName(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModule(ModuleViewModel model)
        {
            ViewData["ProjectIdFromModuleController"] = model.ProjectId;
            await AddModule(model.ProjectId);
            model.InsertDate = DateTime.UtcNow.AddHours(5).AddMinutes(45);
            if (ModelState.IsValid)
            {
                await _imoduleService.CreateModuleAsync(model);
            }
            return RedirectToAction("Index", "Module", new { id = model.ProjectId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateModule(int id)
        {
            try
            {
                if (id != 0)
                {
                    ViewBag.Controller = "Module";
                    var result = await _imoduleService.GetModuleByIdAsync(id);
                    ViewData["ParentModuleNameFromModuleController"] = result.ModuleName;
                    await SetProjectName(result.ProjectId);
                    return View(result);
                }
                else
                {
                    throw new Exception("The module id is NULL!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var moduleId = await _imoduleService.UpdateModuleAsync(model);
                if (moduleId != null && moduleId > 0)
                {
                    return RedirectToAction("ViewSubModule", "Module", new { id = moduleId });
                }
            }
            return RedirectToAction("Index", "Module", new { id = model.ProjectId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModule()
        {
            return Json(new { data = await _imoduleService.GetAllModuleAsync() });
        }

        [HttpGet]
        [AllowAnonymous, Authorize(Roles = "ADMIN,CLIENT")]
        public async Task<IActionResult> GetModulesByProjectId(int id)
        {
            return Json(new { data = await _imoduleService.GetModuleByProjectIdAsync(id) });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModule(int id)
        {
            if (id > 0)
            {
                if (id != 0)
                {
                    var isSuccess = await _imoduleService.DeleteModuleAsync(id);
                    if (isSuccess)
                    {
                        return Json(new { success = true, message = "Delete successful" });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The following module contains a submodules. Delete that first;"
                        });
                    }
                }
                else
                {
                    throw new Exception("The module id is NULL!");
                }
            }

            return Json(new { success = false });
        }

        public async Task LoadProjectsDetails()
        {
            var projects = await _imoduleService.GetProjectDropDownAsync();
            ViewBag.ListOfProjects = projects;
        }

        [HttpGet]
        public async Task<IActionResult> ViewSubModule(int id)
        {
            ViewBag.Controller = "Module";
            ViewData["ModuleIdSentFromViewSubModule"] = id;
            var module = await _imoduleService.GetModuleByIdAsync(id);
            ViewData["ParentModuleNameFromModuleController"] = module.ModuleName;
            return View();
        }

        [HttpGet]
        [AllowAnonymous, Authorize(Roles = "ADMIN,CLIENT")]
        public async Task<IActionResult> GetSubModules(int id)
        {
            return Json(new { data = await _imoduleService.GetSubModuleByParentModuleIdAsync(id) });
        }

        [HttpGet]
        public async Task<IActionResult> AddSubModule(int id)
        {
            ViewBag.Controller = "Module";
            ViewData["ProjectIdFromSubModuleController"] = await _imoduleService.GetProjectIdByParentModuleIdAsync(id);
            ViewData["ParentModuleIdFromSubModuleController"] = id;
            var module = await _imoduleService.GetModuleByIdAsync(id);
            ViewData["ParentModuleNameFromModuleController"] = module.ModuleName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubModule(ModuleViewModel model)
        {
            var projectId = 0;

            if (ModelState.IsValid)
            {
                projectId = await _imoduleService.CreateModuleAsync(model);
            }
            return RedirectToAction(nameof(Index), new { id = projectId });
        }

        [HttpGet]
        [AllowAnonymous, Authorize(Roles = "ADMIN,CLIENT")]
        public async Task<IActionResult> CheckIfParentModuleExists(int id) 
        {
            return Ok(await _imoduleService.IsParentModule(id));
        }
    }
}