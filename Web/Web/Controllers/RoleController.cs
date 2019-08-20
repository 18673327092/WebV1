using Base.IService;
using Base.Model;
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
    public class RoleController : AdminBaseController
    {
        public IRoleService service { get; set; }
        public RoleController()
        {
            EntityName = "Sys_Role";
        }

        [PageAuth]
        public ActionResult Form(int id = 0)
        {

            base.BaseForm(id);
            if (id == 0)
            {
                return View(new Sys_Role()
                {
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            else
            {
                Sys_Role entity = service.Get(id).Data;
                ViewBag.Data = ToJson(entity);
                return View();
            }
        }

        [HttpPost]
        public JsonResult _Save(Sys_Role entity)
        {
            if (entity.ID == 0)
            {
                 List<Sys_Role> rtlist = service.GetAll().Where(c => c.Name == entity.Name).ToList();
                 if (rtlist.Count > 0)
                 {
                     ItemResult<int> item = new ItemResult<int>();
                     item.Success = false;
                     item.Message = "角色名称不能重复，请修改名称后，重新保存。";
                     return Json(item, JsonRequestBehavior.DenyGet);
                 }
                 else
                 {
                     return Json(service.Insert(entity), JsonRequestBehavior.DenyGet);
                 }
            }
            else
            {
                List<Sys_Role> Irtlist = service.GetAll().Where(c => c.Name == entity.Name && c.ID != entity.ID).ToList();
                if (Irtlist.Count > 0)
                {
                    ItemResult<int> item = new ItemResult<int>();
                    item.Success = false;
                    item.Message = "角色名称不能重复，请修改名称后，重新保存。";
                    return Json(item, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    ApplicationContext.Cache.Remove(EntityName + entity.ID);
                    entity.UpdateTime = DateTime.Now;
                    return Json(service.Update(entity), JsonRequestBehavior.DenyGet);
                }
            }
        }

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

       
    }
}
