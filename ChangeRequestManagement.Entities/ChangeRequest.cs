using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class ChangeRequest : BaseEntity
    {
        [Key]
        public int ChangeRequestId { get; set; }

        [Required]
        public string ChangeRequestNumber { get; set; }

        [ForeignKey(nameof(ListItemChangeRequestType)), Required]
        public int ChangeRequestTypeListItemId { get; set; }

        public ListItem ListItemChangeRequestType { get; set; }

        [ForeignKey(nameof(ListItemPriority)), Required]
        public int PriorityListItemId { get; set; }

        public ListItem ListItemPriority { get; set; }

        [ForeignKey(nameof(Module)), Required]
        public int ModuleId { get; set; }

        public Module Module { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(Client)), Required]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public DateTime DeadlineDate { get; set; }
        public string Comment { get; set; }
    }
}