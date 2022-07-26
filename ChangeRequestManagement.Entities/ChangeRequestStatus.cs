using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class ChangeRequestStatus : BaseEntity
    {
        [Key]
        public int ChangeRequestStatusId { get; set; }

        [ForeignKey(nameof(ChangeRequest)), Required]
        public int ChangeRequestId { get; set; }

        public ChangeRequest ChangeRequest { get; set; }

        [ForeignKey(nameof(ListItem)), Required]
        public int ChangeRequestStatusListItemId { get; set; }

        public ListItem ListItem { get; set; }

        public string Comment { get; set; }
    }
}