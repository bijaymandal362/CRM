using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagment.Models.ViewModels.DataTableProperty
{
   public class DataTablePropertyViewModel
    {

        public string Start { get; set; }
        public string Length { get; set; }
        public int Draw { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int RecordTotal { get; set; }

    }
}
