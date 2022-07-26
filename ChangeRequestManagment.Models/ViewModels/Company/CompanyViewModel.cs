using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.ViewModels.Company
{
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }

        [Required, Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}