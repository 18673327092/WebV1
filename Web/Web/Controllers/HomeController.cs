using Base.IService;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility.Container;
using Web.Attribute;

namespace Web.Controllers
{
    [AdminLogin]
    public class HomeController : Controller
    {
        IOperationConfigService operationconfigService;
        IMenuAreaService menuareaService;
        //
        // GET: /Home/
        public HomeController(IMenuAreaService _menuareaService, IOperationConfigService _operationconfigService)
        {
            operationconfigService = _operationconfigService;
            menuareaService = ObjectContainer.Current.Resolve<IMenuAreaService>();
        }
        public ActionResult Index(AdminCredential User, int UserID, int id = 0)
        {

            var MenusAreas = menuareaService.GetAll().ToList().Where(e => e.SiteID == 1000 && e.StateCode == 0).ToList();
            if (MenusAreas != null && MenusAreas.Count > 0)
            {
                //if (id == 0) { id = MenusAreas.FirstOrDefault().ID; }
            }
            ViewBag.AreaID = id;
            ViewBag.MenusAreas = MenusAreas;
            ViewBag.Menus = operationconfigService.GetAccessMenus(User.Roles, UserID).Data.Where(e => e.IsHide == false && e.SiteID == 1000).ToList();
            return View();
        }
    }
}
