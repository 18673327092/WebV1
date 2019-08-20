using Base.IService;
using Base.Model.Enum;
using Base.Model.Sys.Model;
using Base.Service.SystemSet;
using Newtonsoft.Json;
using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;
using Utility.Components;
using Web.Attribute;
using Web.Utility;

namespace Web.Controllers
{
    [AdminLogin]
    public class LookUpController : Controller
    {
        IFieldService fieldService;
        public LookUpController(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }

        ///<summary>
        /// 
        /// </summary>
        /// <param name="fieldid">字段ID</param>
        /// <param name="value">已选值</param>
        /// <param name="id">原字段自定义ID（用于取得返回值）</param>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public PartialViewResult RelationFieldSelectPanel(int fieldid, string value = "", int id = 0, string filter = "", int height = 310)
        {
            ViewBag.ID = id;
            ViewBag.Value = value;
            ViewBag.WhereSql = filter;
            ViewBag.SelectValue = new List<RelationEntityField>();
            Repository<RelationEntityField> crmRepository = new Repository<RelationEntityField>();
            var entity = fieldService.Get(fieldid).Data;
            ViewBag.Field = entity.Name;
            var RelationEntityID = SystemSetService.Entity.GetEntityID(entity.RelationEntity);
            ViewBag.EntityID = RelationEntityID;
            ViewBag.RelationEntity = entity.RelationEntity.Replace("Sys_user","Sys_User");
            var ViewObj = SystemSetService.View.GetViewByTypeEntityID(RelationEntityID, ViewTypeEnum.弹框视图.ToString());
            ViewBag.Columns = ViewObj.FieldList;
            ViewBag.ViewID = ViewObj.ID;
            ViewBag.SearchField = SystemSetService.Search.GetDialogSearchFields(RelationEntityID);
            if (!string.IsNullOrEmpty(value))
            {
                var sql = new PetaPoco.Sql("SELECT ID,Name FROM " + entity.RelationEntity);
                sql.Where("ID IN(" + value + ")");
                ViewBag.SelectValue = crmRepository.GetList<RelationEntityField>(sql);
            }
            ViewBag.height = height;
            return PartialView();
        }

        public JsonResult RelationAjaxList(Pagination rq, AdminCredential User)
        {
            KendoFilterHelper.Single.SetPagination(Request.Form, rq);
            rq.WhereSql = rq.WhereSql.Replace("$", ".").Replace("__", "=");
            if (!string.IsNullOrEmpty(rq.KeyWord)) rq.Page = 1;
            return new ListJsonResult
            {
                Data = JsonConvert.DeserializeObject(SystemSetService.ListDataSource.GetListDataSource(rq, User)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public PartialViewResult RelationFieldSelectPanelMulti(int fieldid, string value = "", int id = 0, string filter = "", int height = 310, string page = "list")
        {
            ViewBag.ID = id;
            ViewBag.Value = value;
            ViewBag.WhereSql = filter;
            ViewBag.SelectValue = new List<RelationEntityField>();
            Repository<RelationEntityField> crmRepository = new Repository<RelationEntityField>();
            var entity = fieldService.Get(fieldid).Data;
            ViewBag.Field = entity.Name;
            var RelationEntityID = SystemSetService.Entity.GetEntityID(entity.RelationEntity);
            ViewBag.EntityID = RelationEntityID;
            ViewBag.RelationEntity = entity.RelationEntity;
            var ViewObj = SystemSetService.View.GetViewByTypeEntityID(RelationEntityID, ViewTypeEnum.弹框视图.ToString());
            ViewBag.Columns = ViewObj.FieldList;
            ViewBag.ViewID = ViewObj.ID;
            ViewBag.page = page;
            ViewBag.SearchField = SystemSetService.Search.GetDialogSearchFields(RelationEntityID);
            if (!string.IsNullOrEmpty(value))
            {

                var sql = new PetaPoco.Sql("SELECT ID,Name FROM " + entity.RelationEntity);
                sql.Where("ID IN(" + value + ")");
                ViewBag.SelectValue = crmRepository.GetList<RelationEntityField>(sql);
            }
            ViewBag.height = height;
            return PartialView();
        }
    }
}