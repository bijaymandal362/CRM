using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class Module : BaseEntity
    {
        [Key]
        public int ModuleId { get; set; }

        public int? ParentModulelId { get; set; }

        [Required, Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [ForeignKey(nameof(Project)), Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}