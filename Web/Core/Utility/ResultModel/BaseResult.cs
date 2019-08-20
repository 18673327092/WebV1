using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.ResultModel
{
    public class BaseResult
    {
        /// <summary>
        /// 操作是否成功。
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 服务器返回的提示。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 服务器返回的执行结果编码。
        /// </summary>
        public int Code { get; set; }
    }
}
