using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.IService
{
    /// <summary>
    /// 短信发送接口
    /// </summary>
    public interface ISMSService
    {
        string Send(string mobile, string content);

        string Send(int customerId, string content);
    }
}
