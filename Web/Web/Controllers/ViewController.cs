//using Base.IService;
//using Base.Model;
//using Base.Model.Sys.Model;
//using Base.Service.SystemSet;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using Utility;
//using Utility.Components;
//using Web.Attribute;

//namespace Web.Controllers
//{
//    [LoginAuth]
//    public class ViewController : Controller
//    {
//        //
//        // GET: /Customer/
//        IFieldService fieldService;
//        public ViewController(IFieldService _fieldService)
//        {
//            fieldService = _fieldService;
//        }

//        public ActionResult Index(int eid, AdminCredential User, int v = 0)
//        {
//            var entity =SystemSetService.Entity.GetEntityItem(eid).Item;
//            ViewBag.EntityId = eid;
//            ViewBag.AreaName = entity.AreaName;
//            ViewBag.ControllerName = entity.ControllerName;
//            ViewBag.ViewId = v;
//            ViewBag.UserID = User.ID;
//            ViewBag.Entity = JsonConvert.SerializeObject(entity);
//            ViewBag.EntityName = entity.Name;
//            ViewBag.FieldList = JsonConvert.SerializeObject(SystemSetService.Field.GeViewFieldList(new Sys_Field() { EntityID = eid }).List);
//            ViewBag.FKEntityList = JsonConvert.SerializeObject(SystemSetService.Entity.GeFKEntityAndFieldsListByEntityID(eid).List);
//            ViewBag.Express = string.Empty;
//            if (v > 0)
//            {
//                var view = fieldService.GetViewItem(v).Item;
//                ViewBag.View = view;
//                ViewBag.ViewName = view.Title;
//                ViewBag.ViewRemark = view.Remark;
//                ViewBag.Express = view.Express;
//                ViewBag.ViewType = view.Type;
//                ViewBag.ViewSort = view.Sort;
//                ViewBag.IsMenu = view.IsMenu.HasValue && view.IsMenu.Value ? 1 : 0;
//                ViewBag.ViewFieldList = fieldService.GetGridFieldsByViewID(v);
//            }
//            else
//            {
//                ViewBag.ViewFieldList = fieldService.GetDefaultViewByEntityID(eid).FieldList;
//            }

//            return View();
//        }

//        public JsonResult _List(Pagination rq)
//        {
//            KendoFilterHelper.Instance.SetPagination(Request.Form, rq);
//            return new ListJsonResult { Data = fieldService.GetPagingViewList(new Sys_View() { }, rq), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
//        }

//        public string GetFieldList(int entityId)
//        {
//            return JsonConvert.SerializeObject(fieldService.GeViewFieldList(new Sys_Field() { EntityID = entityId }).List);
//        }

//        public PartialViewResult DictionarySelectPanel(int fieldid, string value = "")
//        {
//            List<string> values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            ViewBag.Value = value;
//            List<Sys_Dictionary> list = fieldService.GetAllDictionaryList(fieldid).List;
//            ViewBag.LeftDictionary = list.Where(e => !values.Contains(e.Value.ToString())).ToList();
//            ViewBag.RightDictionary = list.Where(e => values.Contains(e.Value.ToString())).ToList();
//            return PartialView();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="fieldid">字段ID</param>
//        /// <param name="value">已选值</param>
//        /// <param name="id">原字段自定义ID（用于取得返回值）</param>
//        /// <param name="filter">筛选条件</param>
//        /// <returns></returns>
//        public PartialViewResult RelationFieldSelectPanel(int fieldid, string value = "", int id = 0, string filter = "", int height = 310)
//        {
//            ViewBag.ID = id;
//            ViewBag.Value = value;
//            ViewBag.WhereSql = filter;
//            ViewBag.SelectValue = new List<RelationEntityField>();
//            CRMRepository<RelationEntityField> crmRepository = new CRMRepository<RelationEntityField>();
//            var entity = fieldService.Get(fieldid).Item;
//            ViewBag.Field = entity.Name;
//            var RelationEntityID = fieldService.GetEntityID(entity.RelationEntity);
//            ViewBag.EntityID = RelationEntityID;
//            ViewBag.RelationEntity = entity.RelationEntity;
//            var ViewObj = fieldService.GetViewByTypeEntityID(RelationEntityID, ViewTypeEnum.弹框视图.ToString());
//            ViewBag.Columns = ViewObj.FieldList;
//            ViewBag.ViewID = ViewObj.ID;
//            ViewBag.SearchField = fieldService.GetDialogSearchFields(RelationEntityID);
//            if (!string.IsNullOrEmpty(value))
//            {

//                var sql = new PetaPoco.Sql("SELECT ID,Name FROM " + entity.RelationEntity);
//                sql.Where("ID IN(" + value + ")");
//                ViewBag.SelectValue = crmRepository.GetList<RelationEntityField>(sql);
//            }
//            ViewBag.height = height;
//            return PartialView();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="fieldid">字段ID</param>
//        /// <param name="value">已选值</param>
//        /// <param name="id">原字段自定义ID（用于取得返回值）</param>
//        /// <param name="filter">筛选条件</param>
//        /// <returns></returns>
//        public PartialViewResult RelationFieldSelectPanelMulti(int fieldid, string value = "", int id = 0, string filter = "", int height = 310, string page = "list")
//        {
//            ViewBag.ID = id;
//            ViewBag.Value = value;
//            ViewBag.WhereSql = filter;
//            ViewBag.SelectValue = new List<RelationEntityField>();
//            CRMRepository<RelationEntityField> crmRepository = new CRMRepository<RelationEntityField>();
//            var entity = fieldService.Get(fieldid).Item;
//            ViewBag.Field = entity.Name;
//            var RelationEntityID = fieldService.GetEntityID(entity.RelationEntity);
//            ViewBag.EntityID = RelationEntityID;
//            ViewBag.RelationEntity = entity.RelationEntity;
//            var ViewObj = fieldService.GetViewByTypeEntityID(RelationEntityID, ViewTypeEnum.弹框视图.ToString());
//            ViewBag.Columns = ViewObj.FieldList;
//            ViewBag.ViewID = ViewObj.ID;
//            ViewBag.page = page;
//            ViewBag.SearchField = fieldService.GetDialogSearchFields(RelationEntityID);
//            if (!string.IsNullOrEmpty(value))
//            {

//                var sql = new PetaPoco.Sql("SELECT ID,Name FROM " + entity.RelationEntity);
//                sql.Where("ID IN(" + value + ")");
//                ViewBag.SelectValue = crmRepository.GetList<RelationEntityField>(sql);
//            }
//            ViewBag.height = height;
//            return PartialView();
//        }

//        public JsonResult RelationAjaxList(Pagination rq, AdminCredential User)
//        {
//            KendoFilterHelper.Instance.SetPagination(Request.Form, rq);
//            rq.WhereSql = rq.WhereSql.Replace("$", ".").Replace("__", "=");
//            if (!string.IsNullOrEmpty(rq.KeyWord)) rq.Page = 1;
//            return new ListJsonResult
//            {
//                Data = JsonConvert.DeserializeObject(fieldService.GetViewDataListResult(rq, User)),
//                JsonRequestBehavior = JsonRequestBehavior.AllowGet
//            };
//            //return Json(fieldService.GetFKEntityFiledList(request, rq));
//        }

//        public JsonResult Submit(string JsonViewSaveEntity, string JsonView, string JsonField, AdminCredential User)
//        {
//            ViewSaveEntity entity = JsonConvert.DeserializeObject<ViewSaveEntity>(JsonViewSaveEntity);
//            Sys_View view = JsonConvert.DeserializeObject<Sys_View>(JsonView);
//            List<ViewFieldModel> field_list = new List<ViewFieldModel>();
//            if (!string.IsNullOrEmpty(JsonField))
//            {
//                field_list = JsonConvert.DeserializeObject<List<ViewFieldModel>>(JsonField);
//            }
//            view.Express = HttpUtility.UrlDecode(view.Express);
//            foreach (var itemq in field_list)
//            {
//                if (!string.IsNullOrEmpty(itemq.Template))
//                {
//                    itemq.Template = HttpUtility.UrlDecode(itemq.Template);
//                    itemq.Template = HttpUtility.UrlDecode(itemq.Template);
//                }

//            }
//            view.OwnerID = User.ID;
//            return Json(fieldService.SaveView(entity, view, field_list));
//        }

//        public JsonResult GetViewList(Pagination rq, AdminCredential User)
//        {
//            return new ListJsonResult() { Data = fieldService.GetPagingViewList(new Sys_View() { EntityID = rq.eid, OwnerID = User.ID }, rq) };
//        }

//        #region 预览

//        public ActionResult Preview(int v = 0, string key = "")
//        {
//            if (!string.IsNullOrEmpty(key))
//            {
//                List<Sys_Field> field_list = (List<Sys_Field>)Session["Columns" + key];
//                ViewBag.GridFields = JsonConvert.SerializeObject(field_list);
//            }
//            else
//            {

//            }
//            return View();
//        }

//        public JsonResult PreviewSubmit(string JsonViewSaveEntity, string JsonField = "")
//        {
//            StringBuilder sqlstrb = new StringBuilder();
//            List<string> field = new List<string>();
//            List<JoinEntity> joinEntity = new List<JoinEntity>();
//            ViewSaveEntity entity = JsonConvert.DeserializeObject<ViewSaveEntity>(JsonViewSaveEntity);
//            List<ViewFieldModel> field_list = new List<ViewFieldModel>();
//            if (!string.IsNullOrEmpty(JsonField))
//            {
//                field_list = JsonConvert.DeserializeObject<List<ViewFieldModel>>(JsonField);
//            }
//            else
//            {
//                field_list.Add(new ViewFieldModel()
//                {
//                    Field = "ID",
//                    Title = "ID",
//                    FieldSql = entity.EntityName + "." + "ID"
//                });
//                field_list.Add(new ViewFieldModel()
//                {
//                    Field = "Name",
//                    Title = "名称",
//                    FieldSql = entity.EntityName + "." + "Name"
//                });
//            }
//            foreach (var itemq in field_list)
//            {
//                if (!string.IsNullOrEmpty(itemq.FieldSql))
//                    field.Add(itemq.FieldSql);
//                if (itemq.FieldType == EnumFieldType.关联其他表.ToString() && !string.IsNullOrEmpty(itemq.TemplateSql))
//                {
//                    field.Add(itemq.TemplateSql);
//                }
//                if (joinEntity.Where(e => e.JoinTableName == itemq.JoinTableName).Count() == 0 && !string.IsNullOrEmpty(itemq.JoinTableName))
//                {
//                    joinEntity.Add(new JoinEntity() { JoinTableName = itemq.JoinTableName, OnSql = itemq.OnSql });
//                    if (!string.IsNullOrEmpty(itemq.Template))
//                        itemq.Template = HttpUtility.UrlDecode(itemq.Template);
//                }
//            }

//            foreach (var itemq in entity.Relationentity)
//            {
//                if (joinEntity.Where(e => e.JoinTableName == itemq.JoinTableName).Count() == 0
//                    && !string.IsNullOrEmpty(itemq.JoinTableName))
//                    joinEntity.Add(new JoinEntity() { JoinTableName = itemq.JoinTableName, OnSql = itemq.OnSql });
//            }
//            sqlstrb.AppendFormat("SELECT {0}", string.Join(",", field));
//            sqlstrb.AppendFormat(" FROM {0}", entity.EntityName);

//            foreach (var e in joinEntity)
//            {
//                sqlstrb.AppendFormat(" LEFT JOIN {0}", e.JoinTableName);
//                sqlstrb.AppendFormat(" ON {0}", e.OnSql);
//            }
//            sqlstrb.AppendFormat(" WHERE 1=1 {0}", string.IsNullOrEmpty(entity.FilterSql) ? "" : " AND " + entity.FilterSql);
//            var sessionKey = DateTime.Now.ToString("yyyyMMddhhmmssffff");
//            Session["Sql" + sessionKey] = sqlstrb;
//            Session["Columns" + sessionKey] = field_list;
//            return Json(new { Success = true, data = sessionKey, sql = sqlstrb.ToString() });
//        }

//        public JsonResult _PreviewList(string key, Pagination rq, AdminCredential User)
//        {
//            KendoFilterHelper.Instance.SetPagination(Request.Form, rq);
//            var data =SystemSetService.ListDataSource.GetViewDataListResult(Session["Sql" + key].ToString(), rq, User);
//            return new ListJsonResult
//            {
//                Data = JsonConvert.DeserializeObject(data),
//                JsonRequestBehavior = JsonRequestBehavior.AllowGet
//            };
//        }
//        #endregion


//        public JsonResult SendViewFields(string viewfields, string viewsort)
//        {
//            var sessionKey = DateTime.Now.ToString("yyyyMMddhhmmssffff");
//            Session[sessionKey] = JsonConvert.DeserializeObject<List<ViewFieldModel>>(viewfields);
//            Session[sessionKey + "viewsort"] = viewsort;
//            return Json(new { Success = true, data = sessionKey });
//        }
//        public ActionResult EditColumns(int eid, string key, AdminCredential User)
//        {
//            var entity = SystemSetService.Entity.GetEntityItem(eid).Item;
//            ViewBag.EntityName = entity.Name;
//            ViewBag.FieldList = JsonConvert.SerializeObject(Session[key]);
//            ViewBag.viewsort = Session[key + "viewsort"];
//            ViewBag.UserID = User.ID;
//            //if (v == 0)
//            //{
//            //    ViewBag.FieldList = fieldService.GetDefaultViewByEntityID(eid).FieldList;
//            //}
//            //else
//            //{
//            //    ViewBag.FieldList = fieldService.GetGridFieldsByViewID(v);
//            //}
//            // ViewBag.FkEntityList = fieldService.GeFKEntityAndFieldsListByEntityID(eid);
//            return View();
//        }

//        public ActionResult AddColumns(int eid)
//        {
//            var entity = SystemSetService.Entity.GetEntityItem(eid).Item;
//            ViewBag.EntityID = eid;
//            ViewBag.Entity = entity;
//            ViewBag.FkEntityList = fieldService.GetFKEntityListByEntityID(eid).List;
//            return View();
//        }

//        /// <summary>
//        /// 删除视图
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public JsonResult _DeleView(int id)
//        {
//            return Json(fieldService.DeleteView(id), JsonRequestBehavior.AllowGet);
//        }

//        /// <summary>
//        /// 获取外键表的字段
//        /// </summary>
//        /// <param name="eid"></param>
//        /// <param name="ceid"></param>
//        /// <returns></returns>
//        public JsonResult _FieldListByEntity(int eid, int ceid,int UserID=0)
//        {
//            return Json(fieldService.GetGridFields(eid, ceid, UserID), JsonRequestBehavior.AllowGet);
//        }


//    }
//}
