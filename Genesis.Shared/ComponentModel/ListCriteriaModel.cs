using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Shared.ComponentModel
{
    public class ListCriteriaModel
    {
        public string FilterColumn { get; set; }
        public string FilterQuery { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
