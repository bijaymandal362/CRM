using ChangeRequestManagment.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.ClientAdditionValidationEnum;

namespace ChangeRequestManagment.Models.Client
{
    public class AddClientSuccessViewModel
    {
        public bool IsSucces { get; set; } = false;
        public ClientError ErrorType { get; set; }
    }
}
