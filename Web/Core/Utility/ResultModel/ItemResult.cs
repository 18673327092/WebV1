using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.ResultModel
{
    public class ItemResult<T> : BaseResult
    {
        public T Data { get; set; }
    }
}
