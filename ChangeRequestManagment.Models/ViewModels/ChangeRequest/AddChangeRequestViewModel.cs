using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequest
{
    public class AddChangeRequestViewModel
    {
        public string ChangeRequestNumber { get; set; }
        [Required]
        public int ChangeRequestTypeListItemId { get; set; }
        [Required]
        public int PriorityListItemId { get; set; }
        [Required]
        public int ModuleId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }
        [Required]
        public int InsertPersonId { get; set; }
        public DateTime Insertdate { get; set; }
        public string Comment { get; set; }
    }
}
