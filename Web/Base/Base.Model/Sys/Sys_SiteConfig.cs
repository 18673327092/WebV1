using Base.Model;
using System;
using System.Runtime.Serialization;
namespace Base.Model
{
    /// <summary>
    /// 网站配置
    /// </summary>
    public class Sys_SiteConfig : BaseModel
    {
        /// <summary>
        /// 微信AccessToken
        /// </summary>
        [DataMember]
        public string WechatAccessToken { get; set; }

        /// <summary>
        /// 微信AccessToken刷新时间
        /// </summary>
        [DataMember]
        public DateTime? WechatAccessTokenRefreshTime { get; set; }
    }
}