using ChangeRequestManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus
{
    public class ChangeRequestStatusInfoViewModel
    {
        public int ChangeRequestStatusId { get; set; }
        public int ChangeRequestId { get; set; }
        public int ChangeRequestStatusListItemId { get; set; }
        public string ListItemName { get; set; }
        public string Comment { get; set; }
        public int InsertPersonId { get; set; }
        public string InsertPersonFirstName { get; set; }
        public string InsertPersonLastName { get; set; }
        public DateTime InsertDate {get; set;}
        public string UpdatePersonFirstName { get; set; }
        public string UpdatePersonLastName { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
