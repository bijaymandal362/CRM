using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    [Index(nameof(ListItemCategoryId), nameof(ListItemName), IsUnique = true)]
    [Index(nameof(ListItemSystemName), IsUnique = true)]
    public class ListItem : BaseEntity
    {
        [Key]
        public int ListItemId { get; set; }

        [Required]
        public int ListItemCategoryId { get; set; }

        [Required, StringLength(50)]
        public string ListItemName { get; set; }

        [Required, StringLength(50)]
        public string ListItemSystemName { get; set; }

        [ForeignKey(nameof(ListItemCategoryId))]
        public ListItemCategory ListItemCategory { get; set; }
    }
}