using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.ViewModels.Project
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        [Required, Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}