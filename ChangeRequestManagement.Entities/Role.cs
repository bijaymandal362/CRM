using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagement.Entities
{
    public class Role : BaseEntity
    {
        [Key]
        public int RoleId { get; set; }

        [Required, Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}