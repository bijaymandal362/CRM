using System;

namespace ChangeRequestManagment.Models.Authentication
{
    public class PersonInfoViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleName { get; set; }
        public bool IsSuccess { get; set; }
    }
}
