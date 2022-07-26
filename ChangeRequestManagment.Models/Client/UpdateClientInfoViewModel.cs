using System;
using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.Client
{
    public class UpdateClientInfoViewModel
    {
        public int PersonId { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int ClientId { get; set; }
        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}