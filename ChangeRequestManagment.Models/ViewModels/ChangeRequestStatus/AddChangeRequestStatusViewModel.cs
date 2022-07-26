using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.ChangeRequestStatus
{
    public class AddChangeRequestStatusViewModel
    {
        public int ChangeRequestId { get; set; }
        public int ChangeRequestStatusListItemId { get; set; }
        public string Comment { get; set; }
        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
