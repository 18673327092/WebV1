using Base.Model;
using Base.Model.Sys.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Components;
using Utility.ResultModel;

namespace Base.Service.SystemSet
{
    public class LookUpService : BaseService<Sys_Field>
    {
        private static LookUpService lookUpService = null;
        public static LookUpService Single
        {
            get
            {
                if (lookUpService == null)
                {
                    lookUpService = new LookUpService();
                }
                return lookUpService;
            }
        }


        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="entityName">实体名</param>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        public string GetLookUpName(string entityName, int id)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql(string.Format("SELECT Name FROM {0} WHERE ID={1}", entityName, id)));
            }
        }

        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="entityName">实体名</param>
        /// <param name="values">多个数据ID</param>
        /// <returns></returns>
        public string GetLookUpsName(string entityName, string values)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql("SELECT dbo.Get" + entityName + "Names('" + values + "')"));
            }
        }

        /// <summary>
        /// 获取实体的数据源-DropDown
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetLookUpDataSourceForDropDown(string entityName)
        {
            using (var db = CreateDao())
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                var result = db.DataSetPage(1, 999, new Sql(string.Format("SELECT * FROM {0}", entityName)));
                if (result.Data.Tables.Count > 0)
                {
                    DataTable dt = result.Data.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        dic.Add(Convert.ToInt32(row["ID"]), row["Name"].ToString());
                    }
                }
                return dic;
            }
        }

        /// <summary>
        ///获取“关联其他表”数据集合
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetFKEntityFiledList(RelationEntityField relation, Pagination page)
        {
            ListResult<RelationEntityField> relactionListResult = new ListResult<RelationEntityField>();
            List<string> fid = new List<string>();
            Sys_Field field = base.Get(relation.FieldID).Data;
            DataSet ds = new DataSet();
            var db = CreateDao();
            var sql = new Sql();
            var fields = GetDialogFields(field.RelationEntity);
            string _sql = "SELECT " + fields + " FROM " + field.RelationEntity + " WHERE StateCode=0";
            if (!string.IsNullOrEmpty(page.KeyWord))
            {
                _sql += " AND (";
                List<Sys_Field> listfiled = base.GetList(new Sql("SELECT Name FROM Sys_field WHERE EntityName='" + field.RelationEntity + "' AND IsAllowSearch=1"));
                int i = 0;
                foreach (var item in listfiled)
                {
                    if (i++ > 0) _sql += " Or ";
                    _sql += string.Format("{0} LIKE '%{1}%'", item.Name, page.KeyWord.Trim());
                }
                _sql += ")";
            }

            if (!string.IsNullOrEmpty(page.SortField))
            {
                _sql = _sql + " Order By " + page.SortField + " " + page.SortType;
            }
            var result = db.DataSetPage(page.Page, page.PageSize, new Sql(_sql));
            db.CloseSharedConnection();
            if (result.Data.Tables.Count > 0)
            {
                return JsonHelper.ToListResultJson(result.Data.Tables[0], result.Page, result.PageSize, result.Total);
            }
            else
            {
                StringBuilder Json = new StringBuilder();
                Json.Append("{\"List\":[");
                Json.Append("],\"PagesCount\": " + 0 + ",\"PageSize\": " + 0 + ",\"Total\": " + 0 + "}");
                return Json.ToString();
            }

        }

        public ListResult<FKEntityModel> GeFKEntityAndFieldsListByEntityID(int entityId)
        {
            ListResult<FKEntityModel> result = new ListResult<FKEntityModel>();
            string CacheKey = "GeFKEntityAndFieldsListByEntityID-" + entityId;
            result.Data = CacheHelper.Single.TryGet<List<FKEntityModel>>(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("E.ShowName+'（'+F.Title+'）' [ShowName],E.Name+' '+E.Name+'_'+F.Name [JoinTableName],E.Name+'_'+F.Name+'.ID='+F.EntityName+'.'+F.Name [OnSql],E.Name+'_'+F.Name [Value],E.ID [RelationEntityID]");
                _sql.From("Sys_field F").InnerJoin("Sys_entity E").On("E.Name=F.RelationEntity");
                _sql.Where("EntityID=@0 AND FieldType='关联其他表'", entityId);
                var entitylist = base.GetList<FKEntityModel>(_sql);
                foreach (var item in entitylist)
                {
                    var fields = base.GetPagingList(new Sql("SELECT * FROM Sys_field WHERE StateCode=0 AND EntityID=@0", entityId), null).Data;
                    item.Fileds = fields;
                }
                return entitylist;
            });
            result.Success = true;
            return result;
        }

        /// <summary>
        /// 获取“关联其他表”实体列表
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ListResult<FKEntityModel> GetFKEntityListByEntityID(int entityId)
        {
            ListResult<FKEntityModel> result = new ListResult<FKEntityModel>();
            string CacheKey = "GetFKEntityListByEntityID-" + entityId;
            result.Data = CacheHelper.Single.TryGet<List<FKEntityModel>>(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("F.Title Title,E.Remark,E.ID,E.Name,E.ShowName+'（'+F.Title+'）' [ShowName],E.Name+' '+E.Name+'_'+F.Name [JoinTableName],E.Name+'_'+F.Name+'.ID='+F.EntityName+'.'+F.Name [OnSql],E.Name+'_'+F.Name [Value],E.ID [RelationEntityID]");
                _sql.From("Sys_field F").InnerJoin("Sys_entity E").On("E.Name=F.RelationEntity");
                _sql.Where("EntityID=@0 AND FieldType='关联其他表' ", entityId);
                return base.GetList<FKEntityModel>(_sql);
            });
            result.Success = true;
            return result;
        }

        /// <summary>
        /// 获取表单外键弹框列表要显示的字段
        /// </summary>
        /// <param name="entityname"></param>
        /// <returns></returns>
        public string GetDialogFields(string entityname)
        {
            string CacheKey = "DialogField-" + entityname;
            return CacheHelper.Single.TryGet(CacheKey, 0, () =>
            {
                var db = CreateDao();
                List<string> list = db.Fetch<string>(new Sql(string.Format("SELECT Name FROM dbo.Sys_field WHERE EntityName='" + entityname + "'")));
                db.CloseSharedConnection();
                return string.Join(",", list);
            }).ToString();
        }
    }
}
