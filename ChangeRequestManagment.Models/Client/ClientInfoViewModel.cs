using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChangeRequestManagment.Models.Client
{
    public class ClientInfoViewModel
{
        public int PersonId { get; set; }
        public int ClientId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
