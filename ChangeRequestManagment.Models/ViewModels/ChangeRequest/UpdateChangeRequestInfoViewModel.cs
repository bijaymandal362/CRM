using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequest
{
    public class UpdateChangeRequestInfoViewModel
    {
        public int ChangeRequestId { get; set; }
        public string ChangeRequestNumber { get; set; }
        public int ChangeRequestTypeListItemId { get; set; }
        public int PriorityListItemId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }
        public string Comment { get; set; }

        public int InsertPersonId { get; set; }
        public int? UpdatePersonId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate {  get; set; } 
    }
}
