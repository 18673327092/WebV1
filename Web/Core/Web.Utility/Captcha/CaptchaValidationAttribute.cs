using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utility.Extension;
namespace Web.Utility.Captcha
{
    /// <summary>
    ///验证码判断
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CaptchaValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aid">常量</param>
        public CaptchaValidationAttribute(string aid) : this(aid, "code") { }
        public CaptchaValidationAttribute(string aid, string fileid)
        {
            this.guid = aid;
            this.fileid = fileid;
        }
        public string guid { get; set; }
        public string fileid { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.ActionParameters["captchaValid"] = false;

            bool checkOnly = false;
            if (filterContext.ActionParameters.ContainsKey("checkOnly")) bool.TryParse(filterContext.ActionParameters["checkOnly"].ToString(), out checkOnly);

            var code = filterContext.HttpContext.Request.Form[fileid];
            var _data = filterContext.HttpContext.Request.Cookies.Get("verifycode");

            var text = string.Format("{0}{1}", guid, (code == "" || code == null) ? "" : code.Trim());

            string actualValue = text.ToLower().EncryptOneWay<System.Security.Cryptography.SHA256CryptoServiceProvider>().ToLower();
            string expectedValue = _data == null ? String.Empty : _data.Value;
            if (!checkOnly) filterContext.HttpContext.Response.Cookies.Remove("verifycode");

            if (String.IsNullOrEmpty(actualValue) || String.IsNullOrEmpty(expectedValue) || !String.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.ActionParameters["captchaValid"] = false;
                return;
            }

            filterContext.ActionParameters["captchaValid"] = true;
        }
    }
}
