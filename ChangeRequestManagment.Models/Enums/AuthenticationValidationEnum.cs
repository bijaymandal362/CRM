
namespace ChangeRequestManagment.Models.Enums
{
    public class AuthenticationValidationEnum
    {
        public enum Error 
        {
            EmailAddressNotFound,
            EmailAddressNotProvided,
            PasswordNotProvided,
            PasswordIsIncorrectOrNoMatch,
            NoAnyError
        }
    }
}
