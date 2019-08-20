using Web.Attribute;
using Base.IService;
using Base.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Model.Sys.Model;
using Utility;
using Utility.Components;
using Web.Utility;

namespace Web.Controllers
{
    public class ScheduleController : AdminBaseController
    {
        public IScheduleService service { get; set; }
        public ScheduleController()
        {
            EntityName = "Sys_Schedule";
        }

        public ActionResult Index()
        {
            return View();
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
        public JsonResult _Save(Sys_Schedule entity, AdminCredential User)
        {
            if (entity.ID == 0)
            {
                entity.OwnerID = User.ID;
                entity.CreateUserID = User.ID;
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = DateTime.Now;
                entity.DepartmentID = CurrentLoginAdmin.DepartmentID;
                entity.StateCode = 0;
                return Json(service.Insert(entity), JsonRequestBehavior.DenyGet);
            }
            else
            {
                var dbEntity = service.Get(entity.ID).Data;
                dbEntity.starttime = entity.starttime;
                dbEntity.endtime = entity.endtime;
                dbEntity.Remark = entity.Remark;
                dbEntity.UpdateTime = DateTime.Now;
                dbEntity.UpdateUserID = User.ID;
                return Json(service.Update(dbEntity), JsonRequestBehavior.DenyGet);
            }
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

        public JsonResult _AjaxList(Sys_Schedule request, AdminCredential User)
        {
            request.OwnerID = User.ID;
            return new ListJsonResult
            {
                Data = service.GetPagingList(request, new Pagination() { Page = 1, PageSize = 999 }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
