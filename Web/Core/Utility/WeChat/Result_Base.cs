using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.WeChat
{
    public class Result_Base
    {
        private int _errcode = 0;

        /// <summary>
        /// 错误码，0：请求成功
        /// </summary>
        public int errcode { get { return _errcode; } set { _errcode = value; } }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
