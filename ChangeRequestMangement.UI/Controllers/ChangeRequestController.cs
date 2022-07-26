#pragma warning disable CS4014
#pragma warning disable CS1998

using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.BusinessLayer.Services.Implementation;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest;
using ChangeRequestManagment.Models.ViewModels.ChangeRequest.Common;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ListItemAndListItemCategoryEnum;

namespace ChangeRequestMangement.UI.Controllers
{
    public class ChangeRequestController:BaseController
    {
        private readonly ILogger<ChangeRequestController> _logger;
        private readonly IChangeRequestService _changeRequestService;
        private readonly IChangeRequestStatusService _changeRequestStatusService;
        private readonly IChangeRequestDocumentService _changeRequestDocumentService;
        private readonly IProjectService _projectService;
        private readonly IModuleService _moduleService;
        private readonly IClientService _clientService;
        private readonly IWebHostEnvironment _environment;
        private readonly ICommonService _commonService;
        private readonly ICompanyService _companyService;

        public ChangeRequestController(
            ILogger<ChangeRequestController> logger,
            IChangeRequestService changeRequestService,
            IChangeRequestStatusService changeRequestStatusService,
            IChangeRequestDocumentService changeRequestDocumentService,
            IProjectService projectService,
            IModuleService moduleService,
            IClientService clientService,
            IWebHostEnvironment environment,
            ICommonService commonService,
            ICompanyService companyService)
        {
            _logger = logger;
            _changeRequestService = changeRequestService;
            _changeRequestStatusService = changeRequestStatusService;
            _changeRequestDocumentService = changeRequestDocumentService;
            _projectService = projectService;
            _moduleService = moduleService;
            _clientService = clientService;
            _environment = environment;
            _commonService = commonService;
            _companyService = companyService;
        }

        public IActionResult Index() 
        {
            if (User.FindFirstValue(ClaimTypes.Role) == "ADMIN")
            {
                return RedirectToAction("ViewStatus", "Admin");
            }
            else 
            {
                return RedirectToAction("ViewStatus", "Client");
            }
        }

        public async Task InitializeChangeRequestTypeAndPriorityAsync()
        {

            var companyId = await _companyService.GetCompanyIdByClientEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            ViewData["Projects"] = await _projectService.GetProjectByCompanyIdAsync(companyId);

            ViewData["ChangeRequestType"] = await _commonService
                                                .GetListItemByCategoryIdAsync(
                                                    (int)ListItemCategoryEnum.ChangeRequestType);

            ViewData["ChangeRequestPriority"] = await _commonService
                                                    .GetListItemByCategoryIdAsync(
                                                        (int)ListItemCategoryEnum.Priority);
        }

        [HttpGet]
        [Authorize(Roles ="CLIENT")]
        public async Task<IActionResult> AddChangeRequest() 
        {
            await InitializeChangeRequestTypeAndPriorityAsync();
            ViewBag.Controller = "ChangeRequest";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> AddChangeRequest(AddChangeRequestCommonViewModel model) 
        {
            if (model.AddChangeRequestViewModel.ModuleId == 0) 
            {
                ModelState.AddModelError("","Please Select a project and its module");
            }
            if (!ModelState.IsValid) 
            {
                ViewBag.Controller = "ChangeRequest";
                await InitializeChangeRequestTypeAndPriorityAsync();
                return View(model);
            }

            var getModule = await _moduleService.GetModuleByIdAsync(model.AddChangeRequestViewModel.ModuleId);

            var splitValueofModule = getModule.ModuleName.Split(" ");
            var generateChangeRequestNumber = "";
            var generateChangeRequestNumberCount = 0;

            foreach (var item in splitValueofModule) 
            {
                generateChangeRequestNumber += item.First();
            }

            generateChangeRequestNumberCount = await _changeRequestService
                                        .GetChangeRequestCountByModuleIdAsync(getModule.ModuleId) + 1;

            var changeRequestResult = await _changeRequestService.AddChangeRequestAsync(new AddChangeRequestViewModel 
            {
                ChangeRequestNumber = "CR-"+ generateChangeRequestNumber+"-"+ generateChangeRequestNumberCount,
                ChangeRequestTypeListItemId = model.AddChangeRequestViewModel
                                                .ChangeRequestTypeListItemId,
                PriorityListItemId = model.AddChangeRequestViewModel.PriorityListItemId,
                ClientId = await _clientService
                            .GetClientIdByPersonIdAsync(int.Parse(User.FindFirst("PersonId").Value)),
                ModuleId = model.AddChangeRequestViewModel.ModuleId,
                Comment = model .AddChangeRequestViewModel.Comment,
                Title = model.AddChangeRequestViewModel.Title,
                Description = model.AddChangeRequestViewModel.Description,
                DeadlineDate = model.AddChangeRequestViewModel.DeadlineDate,
                InsertPersonId = int.Parse(User.FindFirst("PersonId").Value),
                Insertdate = DateTime.UtcNow.AddHours(5).AddMinutes(45)
            });

            var changeRequestStatusResult = await _changeRequestStatusService.AddChangeRequestStatusAsync(new AddChangeRequestStatusViewModel
            {
                ChangeRequestId = changeRequestResult.ChangeRequestId,
                ChangeRequestStatusListItemId = (int)ChangeRequestStatusEnum.Pending,
                Comment = changeRequestResult.Comment,
                InsertDate = changeRequestResult.InsertDate,
                InsertPersonId = int.Parse(User.FindFirst("PersonId").Value)
            });

            await Task.Run( async()=> 
            {

                await FileUploader(
                    model.ChangeRequestDocuments, 
                    changeRequestResult.ChangeRequestId,
                    changeRequestStatusResult.ChangeRequestStatusId);
            });
            return RedirectToAction("ViewStatus", "Client");
        }

        public async Task<IActionResult> AddChangeRequestStatus(int changeRequestId, int ListItemId, string comment)
        {
            var result = await _changeRequestStatusService.AddChangeRequestStatusAsync(new AddChangeRequestStatusViewModel
            {
                ChangeRequestId = changeRequestId,
                ChangeRequestStatusListItemId = ListItemId,
                Comment = comment,
                InsertDate = DateTime.UtcNow.AddHours(5).AddMinutes(45),
                InsertPersonId = int.Parse(User.FindFirst("PersonId").Value)
            });

            var updateChangeRequestInfo = await _changeRequestService.GetChangeRequestByChangeRequestIdAsync(result.ChangeRequestId);
            await _changeRequestService
                    .UpdateChangeRequestInfoAsync(new UpdateChangeRequestInfoViewModel 
                    {
                        ChangeRequestId = result.ChangeRequestId,
                        Title = updateChangeRequestInfo.Title,
                        Comment = updateChangeRequestInfo.Comment,
                        Description = updateChangeRequestInfo.Description,
                        DeadlineDate = updateChangeRequestInfo.DeadlineDate,
                        ChangeRequestTypeListItemId = updateChangeRequestInfo.ChangeRequestTypeListItemId,
                        ChangeRequestNumber= updateChangeRequestInfo.ChangeRequestNumber,
                        PriorityListItemId = updateChangeRequestInfo.PriorityListItemId,
                        ClientId = updateChangeRequestInfo.ClientId,
                        ModuleId = updateChangeRequestInfo.ModuleId,
                        InsertPersonId = updateChangeRequestInfo.InsertPersonId,
                        InsertDate = updateChangeRequestInfo.InsertDate,
                        UpdatePersonId = int.Parse(User.FindFirstValue("PersonId")),
                        UpdateDate = DateTime.UtcNow.AddHours(5).AddMinutes(45)
                    });
            return Ok();
        }

        public async Task FileUploader(List<IFormFile> file, int ChangeRequestId, int ChangeRequestStatusId)
        {
            const string documentFolder = "CRMFiles";
            var webRootPath = Path.Combine(_environment.WebRootPath, documentFolder);
            if (!Directory.Exists(webRootPath)) 
            {
                Directory.CreateDirectory(webRootPath);
            }
            if (file != null && file.Count > 0) 
            {
                var fileNameList = new List<string>();
                foreach (var item in file)
                {
                    var filename = "";
                    var fileExtention = Path.GetExtension(item.FileName);
                    var splitSecquence = item.FileName.Split(fileExtention);
                    foreach (var name in splitSecquence) 
                    {
                        filename += name;
                    }
                    var random = new Random();
                    filename +="-"+Guid.NewGuid().ToString() + fileExtention;
                    try
                    { 
                        fileNameList.Add(filename);
                        item.CopyToAsync(new FileStream(Path.Combine(webRootPath, filename), FileMode.Create));

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Exception occured while uploading file of filename {filename}. Exception Message: {ex.Message}");
                    }
                }

                var addChangeRequestDocumentViewModelList = new List<AddChangeRequestDocumentViewModel>();
                foreach (var item in fileNameList) 
                {
                    addChangeRequestDocumentViewModelList.Add(new AddChangeRequestDocumentViewModel 
                    {
                        ChangeRequestId = ChangeRequestId,
                        DocumentPath = item,
                        ChangeRequestStatusId = ChangeRequestStatusId
                    });
                }

                await _changeRequestDocumentService.AddChangeRequestDocumentAsync(addChangeRequestDocumentViewModelList);
            }
        }

        [HttpGet]
        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> UpdateChangeRequest(int id) 
        {
            await InitializeChangeRequestTypeAndPriorityAsync();

            var changeRequestInfo = await _changeRequestService.GetChangeRequestByChangeRequestIdAsync(id);
            var updateChangeRequestCommonInfoViewModel = new UpdateChangeRequestInfoCommonViewModel();
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel.ChangeRequestId = id;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .ChangeRequestNumber = changeRequestInfo.ChangeRequestNumber;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .ClientId = changeRequestInfo.ClientId;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .Comment = changeRequestInfo.Comment;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .DeadlineDate = changeRequestInfo.DeadlineDate;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .Description = changeRequestInfo.Description;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .Title = changeRequestInfo.Title;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .ModuleId = changeRequestInfo.ModuleId;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .PriorityListItemId = changeRequestInfo.PriorityListItemId;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .ChangeRequestTypeListItemId = changeRequestInfo.ChangeRequestTypeListItemId;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .InsertDate = changeRequestInfo.InsertDate;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .InsertPersonId = changeRequestInfo.InsertPersonId;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .UpdateDate = changeRequestInfo.UpdateDate;
            updateChangeRequestCommonInfoViewModel.UpdateChangeRequestInfoViewModel
                .UpdatePersonId = changeRequestInfo.UpdatePersonId;
            return View(updateChangeRequestCommonInfoViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> UpdateChangeRequest(UpdateChangeRequestInfoCommonViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                await InitializeChangeRequestTypeAndPriorityAsync();
                return View();
            }
            var changeRequestResult = await _changeRequestService
                    .UpdateChangeRequestInfoAsync(new UpdateChangeRequestInfoViewModel
                    {
                        ChangeRequestId = model.UpdateChangeRequestInfoViewModel.ChangeRequestId,
                        Title = model.UpdateChangeRequestInfoViewModel.Title,
                        Comment = model.UpdateChangeRequestInfoViewModel.Comment,
                        Description = model.UpdateChangeRequestInfoViewModel.Description,
                        DeadlineDate = model.UpdateChangeRequestInfoViewModel.DeadlineDate,
                        ChangeRequestTypeListItemId = model.UpdateChangeRequestInfoViewModel.ChangeRequestTypeListItemId,
                        ChangeRequestNumber = model.UpdateChangeRequestInfoViewModel.ChangeRequestNumber,
                        PriorityListItemId = model.UpdateChangeRequestInfoViewModel.PriorityListItemId,
                        ClientId = model.UpdateChangeRequestInfoViewModel.ClientId,
                        ModuleId = model.UpdateChangeRequestInfoViewModel.ModuleId,
                        InsertPersonId = model.UpdateChangeRequestInfoViewModel.InsertPersonId,
                        InsertDate = model.UpdateChangeRequestInfoViewModel.InsertDate,
                        UpdatePersonId = int.Parse(User.FindFirstValue("PersonId")),
                        UpdateDate = DateTime.UtcNow.AddHours(5).AddMinutes(45)
                    });

            var changeRequestStatusResult = await _changeRequestStatusService.AddChangeRequestStatusAsync(new AddChangeRequestStatusViewModel
            {
                ChangeRequestId = changeRequestResult.ChangeRequestId,
                ChangeRequestStatusListItemId = (int)ChangeRequestStatusEnum.Pending,
                Comment = changeRequestResult.Comment,
                InsertDate = DateTime.UtcNow.AddHours(5).AddMinutes(45),
                InsertPersonId = int.Parse(User.FindFirst("PersonId").Value)
            });

            await Task.Run(async () =>
            {

                await FileUploader(
                    model.ChangeRequestDocuments,
                    changeRequestResult.ChangeRequestId,
                    changeRequestStatusResult.ChangeRequestStatusId);
            });

            return RedirectToAction("ViewStatus", "Client");
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectIdByModuleId(int id) 
        {
            var projectId = await _moduleService.GetProjectIdByParentModuleIdAsync(id);
            return Ok(projectId);
        }
    }
}
