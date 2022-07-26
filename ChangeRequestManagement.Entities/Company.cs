using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagement.Entities
{
    public class Company : BaseEntity
    {
        [Key]
        public int CompanyId { get; set; }

        [Required, Display(Name = "Company Name")]
        public string CompanyName { get; set; }
    }
}