using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.WeChat.Model
{
    #region 基础类

    /// <summary>
    /// 模板消息基类
    /// </summary>
    public class TemplateBase
    {
        /// <summary>
        /// 接收模板消息用户的UserId 必填
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 接收模板消息token 必填
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id = "";
        public string url { get; set; }

        public string topcolor { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        public object data { get; set; }
    }
    /// <summary>
    /// 返回对象
    /// </summary>
    public class OpenApiResult
    {
        public int Error_code { get; set; }
        public string Error_msg { get; set; }

        public string Msg_id { get; set; }
    }
    /// <summary>
    /// 值
    /// </summary>
    public class TempItem
    {
        public TempItem(string v, string c = "#173177")
        {
            value = v;
            color = c;
        }
        public string value { get; set; }
        public string color { get; set; }
    }

    /// <summary>
    /// 短信返回值
    /// </summary>
    public class SMSResult
    {
        public string code { get; set; }
        public string msgId { get; set; }
        public string time { get; set; }
        public string errorMsg { get; set; }
    }

    #endregion
}
