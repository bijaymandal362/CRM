using System.ComponentModel.DataAnnotations;

namespace ChangeRequestManagment.Models.Authentication
{
    public class LoginViewModel
    {

        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        [Required, DataType(DataType.Password)]
       
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
