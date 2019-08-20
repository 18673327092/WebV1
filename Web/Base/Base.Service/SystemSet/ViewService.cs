using Base.Model;
using Base.Model.Enum;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using ORM;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utility;
using Utility.Components;
using Utility.ResultModel;

namespace Base.Service.SystemSet
{
    public class ViewService : BaseService<Sys_View>
    {
        private static ViewService viewService = null;
        public static ViewService Single
        {
            get
            {
                if (viewService == null)
                {
                    viewService = new ViewService();
                }
                return viewService;
            }
        }
        public ItemResult<Sys_View> GetViewItem(int viewId)
        {
            return base.Get<Sys_View>(viewId);
        }

        public ListResult<Sys_View> GetPagingViewList(Sys_View request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_View");
            if (request.EntityID.HasValue) _sql.Where("EntityID=@0", request.EntityID.Value);
            if (!string.IsNullOrEmpty(request.Type)) _sql.Where("Type=@0", request.Type);
            if (request.OwnerID != 999 && request.OwnerID > 0) _sql.Where("OwnerID=@0", request.OwnerID);

            return base.GetPagingList<Sys_View>(_sql, page);
        }

        public ListResult<Sys_View> GetViewTitleList(Sys_View request)
        {
            Sql _sql = new Sql();
            _sql.Select("ID,Title").From("Sys_View");
            _sql.Where("EntityID=@0 AND Type='系统视图'", request.EntityID.Value);
            return base.GetPagingList<Sys_View>(_sql, new Pagination() { Page = 1, PageSize = 999 });
        }

        public ListResult<Sys_View> GetAllViewList(Sys_View request)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_View");
            if (request.EntityID.HasValue) _sql.Where("EntityID=@0", request.EntityID.Value);
            return base.GetPagingList<Sys_View>(_sql, new Pagination() { Page = 1, PageSize = 999 });
        }

        /// <summary>
        /// 视图显示列
        /// </summary>
        /// <param name="entityId">视图ID</param>
        /// <returns></returns>
        public string GetGridFieldsByViewID(int viewId)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("FieldList").From("Sys_View").Where("ID=@0", viewId);
            List<Sys_View> list = base.GetList<Sys_View>(_sql);
            return list.FirstOrDefault().FieldList;
        }

        /// <summary>
        /// 获取实体表的默认视图
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public int GetDefaultViewIDByEntityID(int entityId)
        {
            var CacheKey = "GetDefaultViewByEntityID" + entityId;
            return ApplicationContext.Convert.ToInt32(CacheHelper.Single.Get(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("top 1 ID").From("Sys_view").Where("EntityID=@0 AND type='系统视图'", entityId);
                var db = CreateDao();
                return db.ExecuteScalar<int>(_sql);
            }));
        }

        /// <summary>
        /// 获取实体表的视图
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public Sys_View GetViewByTypeEntityID(int entityId, string type)
        {
            var CacheKey = "GetViewByTypeEntityID" + entityId + type;
            return CacheHelper.Single.TryGet<Sys_View>(CacheKey, 0, () =>
            {
                List<string> fid = new List<string>();
                Sql _sql = new Sql();
                _sql.Select("top 1 *").From("Sys_view").Where("EntityID=@0 AND type=@1", entityId, type);
                List<Sys_View> list = base.GetList<Sys_View>(_sql);
                return list.FirstOrDefault();
            });
        }

        /// <summary>
        /// 获取视图导出数据
        /// </summary>
        /// <param name="_sql"></param>
        /// <param name="page"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public DataTable GetViewExportData(string _sql, Pagination page, AdminCredential admin)
        {
            DBDatabase db = CreateDao();
            PageOfDaTaSet result;
            var sql = CacheHelper.Single.TryGet($"{page.vid}-{admin.ID}-list-sql", 0, () => { return _sql; }).ToString();
            result = db.DataSetPage(page.Page, page.PageSize, new Sql(sql));
            db.Dispose();
            if (result.Data.Tables.Count > 0)
            {
                return result.Data.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        #region 自定义视图

        /// <summary>
        /// 保存视图
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="view_request"></param>
        /// <returns></returns>
        public ItemResult<int> SaveView(ViewSaveEntity entity, Sys_View view_request, List<ViewFieldModel> field_list)
        {
            int ViewID = 0;
            ItemResult<int> item = new ItemResult<int>();
            Sql _sql = new Sql();
            StringBuilder strb = new StringBuilder();
            var db = CreateDao();
            bool isKeepConnectionAlive = db.KeepConnectionAlive;
            try
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则进行设置
                if (!isKeepConnectionAlive)
                    // 保持连接存活状态
                    db.KeepConnectionAlive = true;
                // 开始事务
                db.BeginTransaction();

                //要显示的列
                var _fieldsql = new Sql();
                _fieldsql.Select("RelationEntity,EntityName,Name,FieldSql,JoinTableName,OnSql,Field,Title,Template,HeaderAttributes,Attributes,HeaderTemplate,Width,Columns_Type,Columns_Format")
                    .From("Sys_Field").Where("EntityID=" + entity.EntityID + " AND IsColumnShow=1");

                var sql = new Sql();
                StringBuilder strb_sql = new StringBuilder();
                #region 构建视图

                List<string> field = new List<string>();
                List<JoinEntity> joinEntity = new List<JoinEntity>();
                foreach (var itemq in field_list)
                {
                    if (!string.IsNullOrEmpty(itemq.FieldSql))
                    {
                        field.Add(itemq.FieldSql);
                    }

                    //列表外键弹框模版对应的Sql语句
                    if (itemq.FieldType == EnumFieldType.关联其他表.ToString() && !string.IsNullOrEmpty(itemq.TemplateSql))
                    {
                        field.Add(itemq.TemplateSql);
                    }

                    //列表选项集显示值对应的Value值
                    if (itemq.FieldType == EnumFieldType.两个选项.ToString() || itemq.FieldType == EnumFieldType.选项集.ToString())
                    {
                        field.Add(itemq.FieldSql.Replace(".Name", ".Value").Replace("$Name", "$Value"));
                    }

                    //列表外键显示值对应的ID值
                    if (itemq.FieldType == EnumFieldType.关联其他表.ToString())
                    {
                        field.Add(itemq.FieldSql.Replace(".Name", ".ID").Replace("$Name", "$ID"));
                    }

                    //外键表
                    if (joinEntity.Where(e => e.JoinTableName == itemq.JoinTableName).Count() == 0 && !string.IsNullOrEmpty(itemq.JoinTableName))
                    {
                        joinEntity.Add(new JoinEntity() { JoinTableName = itemq.JoinTableName, OnSql = itemq.OnSql });
                    }

                    //格式处理
                    if (itemq.FieldType == "时间")
                    {
                        itemq.Filterable = new Filterable() { ui = "datetimepicker" };
                        itemq.Format = "{0:yyyy-MM-dd HH:mm}";
                        itemq.Columns_Type = "date";
                    }
                }
                foreach (var itemq in entity.Relationentity)
                {
                    if (joinEntity.Where(e => e.JoinTableName == itemq.JoinTableName).Count() == 0 && !string.IsNullOrEmpty(itemq.JoinTableName))
                    {
                        joinEntity.Add(new JoinEntity() { JoinTableName = itemq.JoinTableName, OnSql = itemq.OnSql });
                    }
                }
                strb_sql.AppendFormat("SELECT {0} FROM {1}", string.Join(",", field), entity.EntityName);
                foreach (var e in joinEntity)
                {
                    strb_sql.AppendFormat(" LEFT JOIN {0} ON {1}", e.JoinTableName, e.OnSql);
                }
                if (!string.IsNullOrEmpty(entity.FilterSql))
                {
                    strb_sql.AppendFormat(" WHERE {0}", entity.FilterSql);
                }
                string viewname = string.Format("View_{0}_{1}_{2}", entity.EntityName, view_request.OwnerID, DateTime.Now.ToString("yyyyMMddhhmmssffff"));
                #endregion

                #region 编辑视图表
                if (view_request.ID == 0)
                {
                    Sys_View view = new Sys_View()
                    {
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        EntityID = entity.EntityID,
                        Name = viewname,
                        OwnerID = view_request.OwnerID,
                        FieldList = JsonConvert.SerializeObject(field_list),//EntityFieldsToViewFields(field_list)
                        Sql = strb_sql.ToString(),
                        Title = view_request.Title,
                        Remark = view_request.Remark,
                        Express = entity.Express,
                        IsMenu = view_request.IsMenu,
                        Sort = view_request.Sort.Replace("$", "."),
                        Type = string.IsNullOrEmpty(view_request.Type) ? "自定义视图" : view_request.Type
                    };
                    ViewID = Convert.ToInt32(db.Insert(view));
                }
                else
                {
                    var view_list = db.Query<Sys_View>(new Sql(string.Format("SELECT * FROM Sys_View WHERE ID={0}", view_request.ID))).ToList();
                    Sys_View view = view_list.FirstOrDefault();
                    view.Sql = strb_sql.ToString();
                    view.UpdateTime = DateTime.Now;
                    view.Title = view_request.Title;
                    view.Remark = view_request.Remark;
                    view.IsMenu = view_request.IsMenu;
                    view.Express = entity.Express;
                    view.Sort = view_request.Sort.Replace("$", ".");
                    view.FieldList = JsonConvert.SerializeObject(field_list);
                    if (!string.IsNullOrEmpty(view_request.Type))
                    {
                        view.Type = view_request.Type;
                    }
                    db.Update(view);
                    ViewID = view_request.ID;
                }
                #endregion

                db.Execute("DELETE FROM Sys_menu WHERE ViewID=@0", ViewID);
                if (view_request.IsMenu.HasValue && view_request.IsMenu.Value)
                {
                    Sys_Entity ventity = db.FirstOrDefault<Sys_Entity>("SELECT top 1 AreaName,ControllerName FROM dbo.Sys_entity WHERE ID=@0", entity.EntityID);
                    var menuid = base.Insert<Sys_Menu>(new Sys_Menu()
                    {
                        Icon = "icon-glass",
                        MenuName = view_request.Title,
                        ViewID = ViewID,
                        AccessOperation = "0,1,2,3,4,5,6,8,9,",
                        ParentID = 11029,
                        MenuUrl = string.Format("/{0}/{1}/List?v={2}", ventity.AreaName, ventity.ControllerName, ViewID),
                    });
                }

                // 完成事务
                db.CompleteTransaction();
                item.Success = true;
                item.Data = ViewID;
            }
            catch (Exception)
            {
                item.Success = false;
                // 中断事务
                db.AbortTransaction();
            }
            finally
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则关闭数据库连接
                if (!isKeepConnectionAlive)
                {
                    // 关闭连接存活状态
                    db.KeepConnectionAlive = false;
                    // 关闭数据库连接
                    db.CloseSharedConnection();
                }
            }
            return item;
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ItemResult<int> EditView(Sys_View entity)
        {
            ItemResult<int> result = new ItemResult<int>();
            using (var db = CreateDao())
            {
                Sys_View view = db.First<Sys_View>("SELECT * FROM Sys_View WHERE ID=@0", entity.ID);
                if (!string.IsNullOrEmpty(entity.Sql))
                {
                    view.Sql = entity.Sql;
                }
                result.Success = true;
                db.Update(view);
            }
            return result;
        }

        public ItemResult<int> DeleteView(int id)
        {
            return base.DeleteByEntityId<Sys_View>(id);
        }

        /// <summary>
        /// 获取自己的视图
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ListResult<Sys_View> GeViewListByEntityID(Pagination page, int userId)
        {
            ListResult<Sys_View> result = new ListResult<Sys_View>();
            Sql _sql = new Sql();
            _sql.Select("ID,Title,Type,UpdateTime,FieldList,Express,Title,EntityID,Remark").From("Sys_view").Where("EntityID=@0 AND OwnerID=@1", page.eid, userId);
            result.Data = base.GetList<Sys_View>(_sql);
            result.Success = true;
            return result;
        }

        #endregion
    }
}
