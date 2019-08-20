using Base.IService;
using Base.Model;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utility;
using Utility.ResultModel;
using Web.Attribute;

namespace Web.Controllers
{
    public class DepartmentController : AdminBaseController
    {
        public IDepartmentService service { get; set; }
        public DepartmentController()
        {
            EntityName = "Sys_Department";
        }
        #region 通用方法（除_Save方法的实体参数外，其他方法不用改）
        #region 表单页面
        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            base.BaseForm(id);
            return View();
        }
        #endregion

        #region 表单提交
        [HttpPost]
        public JsonResult _Save(Sys_Department entity, AdminCredential User)
        {
            if (entity.ID == 0)
            {
                List<Sys_Department> rtlist = service.GetAll().Where(c => c.Name == entity.Name).ToList();
                if (rtlist.Count > 0)
                {
                    ItemResult<int> item = new ItemResult<int>();
                    item.Success = false;
                    item.Message = "部门名称不能重复，请修改名称后，重新保存。";
                    return Json(item, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    ApplicationContext.Cache.Remove(EntityName + entity.ID);
                    return Json(service.Insert(entity), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                List<Sys_Department> Irtlist = service.GetAll().Where(c => c.Name == entity.Name && c.ID != entity.ID).ToList();
                if (Irtlist.Count > 0)
                {
                    ItemResult<int> item = new ItemResult<int>();
                    item.Success = false;
                    item.Message = "部门名称不能重复，请修改名称后，重新保存。";
                    return Json(item, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    entity.UpdateTime = DateTime.Now;
                    entity.UpdateUserID = User.ID;
                    return Json(service.Update(entity), JsonRequestBehavior.DenyGet);
                }
            }
        }
        #endregion

        #region 详细页

        [PageAuth]
        public ActionResult Detail(int id)
        {
            base.BaseDetail(id);
            return View();
        }
        #endregion

        #region 数据删除
        [HttpPost]
        public JsonResult _Delete(string ids)
        {
            return Json(service.Delete(JsonConvert.DeserializeObject<List<int>>(ids)));
        }
        #endregion

        #region 数据停用

        [HttpPost]
        public JsonResult _Disable(string ids, int statecode = 1)
        {
            return Json(service.Disable(EntityName, JsonConvert.DeserializeObject<List<int>>(ids), statecode), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult _IsUnDisable(int id)
        {
            return Json(service.IsUnDisable(EntityName, id), JsonRequestBehavior.DenyGet);
        }
        #endregion

        public JsonResult _Item(int id)
        {
            return Json(service.Get(id));
        }
        #endregion
    }
}
