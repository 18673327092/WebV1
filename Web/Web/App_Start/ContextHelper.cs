using Base.Model.Sys.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Web.App_Start
{
    public class ContextHelper
    {
        /// <summary>
        ///登录Cookie值
        /// </summary>
        public static string IdentityName = "Mall.Web";

        /// <summary>
        /// 获取当前用户信息  
        /// </summary>
        public static AdminCredential _AdminCredential
        {
            get
            {
                try
                {
                    //if (System.Web.HttpContext.Current.Session[ContextHelper.IdentityName] == null)
                    //{
                    //    HttpCookie _cookie = System.Web.HttpContext.Current.Request.Cookies[ContextHelper.IdentityName];
                    //    AdminCredential _AdminCredential = JsonConvert.DeserializeObject<AdminCredential>(_cookie.Value);
                    //    System.Web.HttpContext.Current.Session[ContextHelper.IdentityName] = _AdminCredential;
                    //}
                    return HttpContext.Current.Session[IdentityName] as AdminCredential;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}