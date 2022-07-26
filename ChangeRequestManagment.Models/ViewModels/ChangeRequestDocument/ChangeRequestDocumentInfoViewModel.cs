using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument
{
    public class ChangeRequestDocumentInfoViewModel
    {
        public int ChangeRequestDocumentId { get; set; }

        public int ChangeRequestId { get; set; }
        public string DocumentPath { get; set; }
        public int ChangeRequestStatusId { get; set; }
    }
}
