using ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequest.Common
{
    public class UpdateChangeRequestInfoCommonViewModel
    {
        public UpdateChangeRequestInfoViewModel UpdateChangeRequestInfoViewModel { get; set; } = new();
        public List<IFormFile> ChangeRequestDocuments { get; set; } = new();
    }
}
