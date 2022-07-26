using ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequest.Common
{
    public class AddChangeRequestCommonViewModel
    {
        public AddChangeRequestViewModel AddChangeRequestViewModel 
        { get; set; } = new AddChangeRequestViewModel();
        public AddChangeRequestStatusViewModel AddChangeRequestStatusViewModel
        { get; set; } = new AddChangeRequestStatusViewModel();

        public List<IFormFile> ChangeRequestDocuments { get; set; } = new();
    }
}
