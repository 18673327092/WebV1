using Base.Model.Sys.Model;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utility;
using Utility.Components;

namespace Web.Attribute
{
    /// <summary>
    /// 页面权限限制
    /// </summary>
    public class PageAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //跳过匿名访问
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (skipAuthorization)
            {
                return;
            }

            if (filterContext.HttpContext.Session["AdminCredential"] == null)
            {
                if (!ApplicationContext.AppSetting.IS_NeedLogin)
                {
                    filterContext.HttpContext.Session["AdminCredential"] = new AdminCredential()
                    {
                        Name = "开发帐号",
                        ID = 999,
                    };
                    return;
                }
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { Message = "登陆超时，请刷新页面重新登陆", Success = false },
                        ContentType = null,
                        ContentEncoding = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.FilePath))
                    {
                        var returnUrl = HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri);
                        filterContext.Result = new RedirectResult("/Login/Index?returnUrl=" + returnUrl);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Login/");
                    }
                }
            }
            else
            {
                var controllerName = (filterContext.RouteData.Values["controller"]).ToString().ToLower();
                var actionName = (filterContext.RouteData.Values["action"]).ToString().ToLower();

                var LoginUser = (AdminCredential)filterContext.HttpContext.Session["AdminCredential"];
                if (LoginUser.ID == 999)
                {
                    return;
                }
                string menuid = filterContext.HttpContext.Request.QueryString["menuid"];
                bool ispageauth = false;
                var mid = Convert.ToInt32(menuid);
                string CacheKey = controllerName + actionName + mid;
                ispageauth = (bool)CacheHelper.Single.Get(CacheKey, 0, () =>
                  {
                      if (OperationConfigService.Instance.ValidControllerNameEqMenuID(mid, controllerName, actionName, out mid))
                      {
                          var menulist = OperationConfigService.Instance.GetAccessMenus(LoginUser.Roles).Data;
                          foreach (var menu in menulist)
                          {
                              if ((menu.ChildMenuList != null && menu.ChildMenuList.Any(e => e.ID == mid)) || menu.ID == mid)
                              {
                                  ispageauth = true;
                              }
                          }
                          if (mid == -1)
                          {
                              ispageauth = true;
                          }
                      }
                      return ispageauth;
                  });
                if (!ispageauth)
                {
                    filterContext.Result = new RedirectResult("/PageAuth.html");
                }
            }
            sw.Stop();
            ApplicationContext.Log.Debug("PageAuthAttribute", sw.ElapsedMilliseconds.ToString());
        }
    }
}