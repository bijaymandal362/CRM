using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChangeRequestManagment.Models.Client
{
    public class AddClientViewModel
    {

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}
