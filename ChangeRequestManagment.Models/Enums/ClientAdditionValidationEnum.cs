using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.Enums
{
    public class ClientAdditionValidationEnum
    {
        public enum ClientError 
        {
            NoAnyError,
            DuplicateEmailError,
            ClientAddError
        }
    }
}
