using Base.IService;
using Base.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utility;

namespace Web.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/
        IMenuService menuService;
        IFieldService fieldService;
        public MenuController(IMenuService _menuService, IFieldService _fieldService)
        {
            menuService = _menuService;
            fieldService = _fieldService;
        }

        public ActionResult List()
        {
            //var fileds = menuService.GetPagingList(new Sys_Menu() { }, new Pagination() { Page = 1, PageSize = 100 });
            return View();
        }

        public JsonResult _List(string callabck)
        {
            List<Sys_MenuModel> list = new List<Sys_MenuModel>();
            var fileds = menuService.GetPagingList(new Sys_Menu() { }, new Pagination() { Page = 1, PageSize = 100, WhereSql = "1=1", SortField = "Sort" });
            foreach (var item in fileds.Data)
            {
                //判断是否有子集
                //var IsParent = menuService.GetPagingList(new Sys_Menu() { }, new Pagination() { Page = 1, PageSize = 100, WhereSql = "ParentID='" + item.ID + "'" });
                list.Add(new Sys_MenuModel()
               {
                   EmployeeId = item.ID,
                   MenuName = item.MenuName,
                   MenuUrl = item.MenuUrl,
                   Sort = item.Sort,
                   Icon = item.Icon,
                   ReportsTo = item.ParentID == 0 ? null : item.ParentID.ToString()
               });
            }
            return Json(list.OrderByDescending(e=>e.Sort).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostSave(Sys_Menu entity)
        {
            if (entity.ID == 0)
            {
                return Json(menuService.Insert(entity), JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(menuService.Update(entity), JsonRequestBehavior.DenyGet);
            }
        }


        [HttpPost]
        public ActionResult _Delete(int Menulds)
        {
            Sys_Menu Sys_mu = new Sys_Menu();
            Sys_mu.ID = Menulds;
            return Json(menuService.Delete(Sys_mu), JsonRequestBehavior.DenyGet);
        }

        public ActionResult Form(int pid, int id = 0)
        {
            ViewBag.MenuList = menuService.GetAll();
            //新增
            if (id == 0)
            {
                Sys_Menu Sys_mu = new Sys_Menu();
                Sys_mu.ParentID = 0;
                Sys_mu.ParenName = "";
                if (pid != 0)
                {
                    var fileds = menuService.GetDefaultByMenuID(pid);
                    Sys_mu.ParentID = pid;
                    Sys_mu.ParenName = fileds.MenuName;
                }
                return PartialView(Sys_mu);
            }
            else
            {
                var Sys_mu = menuService.GetDefaultByMenuID(id);
                if (Sys_mu.ParentID != 0)
                {
                    var field = menuService.GetDefaultByMenuID(Sys_mu.ParentID);
                    Sys_mu.ParenName = field.MenuName;
                }
                return PartialView(Sys_mu);
            }
        }
    }

    public class Sys_MenuModel
    {
        //ID
        public int EmployeeId { get; set; }

        //菜单名称
        public string MenuName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        //菜单地址
        public string MenuUrl { get; set; }
        public string Icon { get; set; }

        //父级
        public string ReportsTo { get; set; }

    }
}
