using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.ViewModels.Module
{
    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public int ProjectId { get; set; }
        public int? ParentModulelId { get; set; }

        [Required, Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}