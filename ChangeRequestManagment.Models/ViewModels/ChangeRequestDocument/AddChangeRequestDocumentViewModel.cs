using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument
{
    public class AddChangeRequestDocumentViewModel
    {
        public int ChangeRequestId { get; set; }
        public int ChangeRequestStatusId { get; set; }
        public string DocumentPath { get; set; }
    }
}
