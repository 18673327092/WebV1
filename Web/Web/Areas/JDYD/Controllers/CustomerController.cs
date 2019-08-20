using Base.Model.Sys.Model;
using Newtonsoft.Json;
using JDYD.IService;
using JDYD.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utility;
using Web.Attribute;
using Web.Controllers;

namespace Web.Areas.JDYD.Controllers
{
    /// <summary>
    /// 客户控制器类
    /// </summary>
    public class CustomerController : AdminBaseController
    {
        public ICustomerService service { get; set; }
        public CustomerController()
        {
            EntityName = "New_Customer";
        }

        #region 通用方法（除_Save方法的实体参数外，其他方法不用改）
        #region 表单页面
        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            base.BaseForm(id);
            var model = new New_Customer();
            if (id > 0)
            {
                model = service.Get(id).Data;
            }
            return View(model);
        }

        #endregion

        #region 表单提交
        [HttpPost]
        public JsonResult _Save(New_Customer entity, AdminCredential User)
        {
            if (entity.ID == 0)
            {
                var result = service.Insert(entity);
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            else
            {
                ApplicationContext.Cache.Remove(EntityName + entity.ID);
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserID = User.ID;
                return Json(service.Update(entity), JsonRequestBehavior.DenyGet);
            }
        }
        #endregion

        #region 详细页

        [PageAuth]
        public ActionResult Detail(int id)
        {
            base.BaseDetail(id);
            var model = service.Get(id).Data;
            return View(model);
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
