using System;
using static ChangeRequestManagment.Models.Enums.AuthenticationValidationEnum;

namespace ChangeRequestManagment.Models.Authentication
{
    public class AuthenticationInfoViewModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public bool IsSuccess { get; set; } = false;
        public Error ErrorType { get; set; }
    }
}
