using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility.Components
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        public static CookieHelper Single
        {
            get
            {
                return new CookieHelper() { };
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minute">超时时间分钟</param>
        /// <param name="path"></param>
        /// <param name="domain">二级域名时定位</param>
        public void Save(string key, string value, int minute = 0, string domain = "", string path = "/")
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
            }
            cookie.Value = HttpUtility.UrlEncode(value);
            if (minute != 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(minute);
            }
            cookie.Path = path;
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        public string Get(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            return cookie != null ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <param name="domain"></param>
        public void Remove(string key, string path = "/", string domain = "")
        {
            var hc = new HttpCookie(key);
            hc.Path = path;
            if (domain != "")
            {
                hc.Domain = domain;
            }
            hc.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(hc);
            //HttpContext.Current.Response.Cookies.Remove(key);
        }
    }
}
