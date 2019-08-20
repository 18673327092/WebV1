using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    public class PageConfigModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public bool IsHaveChildMemu { get; set; }
        public string ReportsTo { get; set; }
        public string OperationJSON { get; set; }
        public string OperationCheck { get; set; }
        public string AccessOperation { get; set; }
    }
}
