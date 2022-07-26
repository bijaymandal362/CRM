using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagement.Entities
{
    [Index(nameof(ListItemCategoryName), IsUnique = true)]
    public class ListItemCategory : BaseEntity
    {
        [Key]
        public int ListItemCategoryId { get; set; }

        [Required, StringLength(50)]
        public string ListItemCategoryName { get; set; }
    }
}