using ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequest.Common
{
    public class ChangeRequestInfoCommonViewModel
    {
        public ChangeRequestInfoViewModel ChangeRequestInfoViewModel { get; set; } = new();
        public List<ChangeRequestDocumentInfoViewModel> ChangeRequestDocumentInfoViewModel { get; set; } = new();
        public List<ChangeRequestStatusInfoViewModel> ChangeRequestStatusInfoViewViewModel { get; set; } = new();
    }
}
