using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.Client
{
    public class GetAllClientInfoFilterViewModel
    {
        public int skip { get; set; }
        public int take { get; set; }
        public ulong count { get; set; }
    }
}
