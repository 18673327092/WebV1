using Base.Model.Sys.Model;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace Web.Attribute
{
    /// <summary>
    /// 登录限制
    /// </summary>
    public class AdminLoginAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 是否跳过验证
        /// </summary>
        private bool isSkipAuthorization = false;
        public AdminLoginAttribute(bool isSkipAuthorization = false)
        {
            this.isSkipAuthorization = isSkipAuthorization;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
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

            if (isSkipAuthorization)
            {
                filterContext.HttpContext.Session["AdminCredential"] = new AdminCredential()
                {
                    Name = "",
                    ID = 999,
                };
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
                var LoginUser = (AdminCredential)filterContext.HttpContext.Session["AdminCredential"];
                filterContext.Controller.ViewBag.User = LoginUser;
                filterContext.Controller.ViewBag.UserID = LoginUser.ID;
                filterContext.Controller.ViewBag.UserName = LoginUser.Name;
                filterContext.Controller.ViewBag.Roles = LoginUser.Roles;
                filterContext.Controller.ViewBag.Jobs = LoginUser.Jobs;
                filterContext.Controller.ViewBag.DepartmentID = LoginUser.DepartmentID;
                filterContext.Controller.ViewBag.DepartmentName = LoginUser.DepartmentName;
                filterContext.Controller.ViewBag.DataConfig = LoginUser.DataConfig;
            }
            sw.Stop();
            ApplicationContext.Log.Debug("OnAuthorization", sw.ElapsedMilliseconds.ToString());
        }

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
                var LoginUser = (AdminCredential)filterContext.HttpContext.Session["AdminCredential"];
                filterContext.ActionParameters["UserID"] = LoginUser.ID;
                filterContext.ActionParameters["User"] = LoginUser;
                filterContext.ActionParameters["StoreID"] = 1000;
            }
            sw.Stop();
            ApplicationContext.Log.Debug("OnActionExecuting", sw.ElapsedMilliseconds.ToString());
        }
    }
}