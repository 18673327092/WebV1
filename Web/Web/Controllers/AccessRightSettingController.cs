using Base.IService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utility;
using Utility.ResultModel;
using Utility.Components;
using Base.Model;
using Base.Model.Sys.Model;

namespace Web.Controllers
{
    public class AccessRightSettingController : Controller
    {
        //
        // GET: /Menu/
        IMenuService menuService;
        IFieldService fieldService;
        IOperationService operationService;
        IRoleService roleService;
        IOperationConfigService operationconfigService;
        IDataConfigService dataconfigService;
        public AccessRightSettingController(IMenuService _menuService, IFieldService _fieldService
            , IOperationService _operationService
            , IRoleService _roleService
            , IOperationConfigService _operationconfigService
            , IDataConfigService _dataconfigService
            )
        {
            menuService = _menuService;
            fieldService = _fieldService;
            operationService = _operationService;
            roleService = _roleService;
            operationconfigService = _operationconfigService;
            dataconfigService = _dataconfigService;
        }

        public ActionResult Index()
        {
            List<Sys_Role> rolelist = roleService.GetAll().Where(e => e.StateCode == 0).ToList();
            List<Tree> treelist = new List<Tree>();

            foreach (var item in rolelist)
            {
                treelist.Add(new Tree()
                {
                    id = item.ID,
                    pId = 1,
                    name = item.Name,
                    open = true,
                    code = item.ID.ToString(),
                });
            }
            ViewBag.FirstRoleID = rolelist.FirstOrDefault().ID;
            ViewBag.RoleTreelist = JsonConvert.SerializeObject(treelist);
            return View();
        }

        #region 操作按钮权限配置
        public ActionResult PageConfig(int id, string RoleName)
        {
            ViewBag.RoleID = id;
            ViewBag.RoleName = RoleName;
            return View();
        }

        public JsonResult _PageConfigList(int roleid)
        {
            List<PageConfigModel> list = new List<PageConfigModel>();
            var menulist = menuService.GetPagingList(new Sys_Menu() { }, new Pagination() { Page = 1, PageSize = 100 }).Data.Where(e => e.IsDug == false && e.IsHide == false).ToList();
            var operationMenulist = operationconfigService.GetPagingList(new Sys_OperationConfig() { }, new Pagination() { Page = 1, PageSize = 100 });
            foreach (var item in menulist)
            {
                list.Add(new PageConfigModel()
               {
                   MenuID = item.ID,
                   MenuName = item.MenuName,
                   AccessOperation = item.AccessOperation,
                   IsHaveChildMemu = menulist.Any(e => e.ParentID == item.ID),
                   ReportsTo = item.ParentID == 0 ? null : item.ParentID.ToString(),
                   OperationJSON = GetOperatoinJSON(item.ID),
                   OperationCheck = operationMenulist.Data.Any(e => e.RoleID == roleid && e.MenuID == item.ID) ?
operationMenulist.Data.SingleOrDefault(e => e.RoleID == roleid && e.MenuID == item.ID).Operations + "," : "",//item.Operations + ","
               });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private string GetOperatoinJSON(int menuId)
        {
            string CacheKey = "OperatoinJSON";
            ListResult<Sys_Operation> Operationlist = CacheHelper.Single.TryGet<ListResult<Sys_Operation>>(CacheKey, 0, () =>
            {
                return operationService.GetList(new Sys_Operation() { });
            });
            return JsonConvert.SerializeObject(Operationlist.Data.Where(e => e.MenuID == menuId));
        }

        [HttpPost]
        public JsonResult PostOperationConfig(string value)
        {
            List<Sys_OperationConfig> list = JsonConvert.DeserializeObject<List<Sys_OperationConfig>>(value);
            return Json(operationconfigService.InsertOperationConfig(list), JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 数据权限配置
        public ActionResult DataConfig(int id, string RoleName)
        {
            ViewBag.RoleID = id;
            ViewBag.RoleName = RoleName;
            return View();
        }

        public JsonResult _DataConfigList(int roleid)
        {
            var result = dataconfigService.GetPagingList(new Sys_DataConfig() { RoleID = roleid }, new Pagination() { Page = 1, PageSize = 100 });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostDataConfig(string value)
        {
            List<Sys_DataConfig> list = JsonConvert.DeserializeObject<List<Sys_DataConfig>>(value);
            return Json(dataconfigService.InsertDataConfig(list), JsonRequestBehavior.DenyGet);
        }
        #endregion

    }


}
