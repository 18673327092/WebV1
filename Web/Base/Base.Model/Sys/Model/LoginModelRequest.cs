using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    /// <summary>
    ///用户登录模型
    /// </summary>
    public class LoginModelRequest
    {
        public string LoginAccount { get; set; }
        public string LoginPassword { get; set; }
    }
}
