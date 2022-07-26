using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class Project : BaseEntity
    {
        [Key]
        public int ProjectId { get; set; }

        private string _ProjectName { get; set; }

        [Required]
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }

            set
            {
                _ProjectName = value.ToUpper();
            }
        }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}