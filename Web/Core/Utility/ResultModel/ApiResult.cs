using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.ResultModel
{
    public class ApiResult<T> : BaseResult
    {
        public T Data { get; set; }
    }
}
