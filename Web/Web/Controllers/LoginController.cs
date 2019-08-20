using Base.IService;
using Base.Model;
using Base.Model.Sys.Model;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using Utility;
using Utility.ResultModel;
using Web.Utility.Captcha;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        IUserService service;
        ISysConfigService sysConfigService;
        public LoginController(IUserService _service, ISysConfigService _sysConfigService)
        {
            service = _service;
            sysConfigService = _sysConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("0925", "codetext")]
        public JsonResult _Login(LoginModelRequest entity, string codetext, bool captchaValid)
        {
            if (ModelState.IsValid)
            {
                ItemResult<AdminCredential> result = new ItemResult<AdminCredential>();
                result = service.Login(new Sys_User() { LoginAccount = entity.LoginAccount, LoginPassword = entity.LoginPassword });
                if (!captchaValid && !string.IsNullOrEmpty(codetext))
                {
                    result.Message = "验证码错误";
                    result.Success = false;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                if (result.Success)
                {
                    Session["AdminCredential"] = result.Data;
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            return Json(new { Message = "登录失败", Success = false }, JsonRequestBehavior.DenyGet);
        }

        //用户登出
        public ActionResult LoginOut()
        {
            Session["AdminCredential"] = null;
            return RedirectToAction("Index");
        }

        //判断登录状态
        public JsonResult IsLogin()
        {
            BaseResult result = new BaseResult();
            if (Session["AdminCredential"] == null)
            {
                if (!ApplicationContext.AppSetting.IS_NeedLogin)
                {
                    Session["AdminCredential"] = new AdminCredential()
                    {
                        Name = "开发帐号",
                        ID = 999,
                    };
                }
            }
            result.Success = Session["AdminCredential"] != null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AuthSubmit(string value)
        {
            SysConfig model = sysConfigService.Get(1000).Data;
            model.NewValue = value;
            return Json(sysConfigService.Update(model));
        }

        #region 验证码
        /// <summary>
        /// 验证码 GET: /User/ValidCode/1234
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public ActionResult ValidCode(string aid)
        {
            CaptchaImage ci = CaptchaImage.CreateCaptcha(aid);
            using (Bitmap b = ci.RenderImage())
            {
                b.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            Response.ContentType = "image/jpeg";
            Response.StatusCode = 200;
            Response.StatusDescription = "OK";
            return null;
        }
        #endregion
    }
}
