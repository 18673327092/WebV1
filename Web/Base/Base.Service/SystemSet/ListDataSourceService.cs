using Base.Model;
using Base.Model.Sys.Model;
using ORM;
using PetaPoco;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utility;
using Utility.Components;

namespace Base.Service.SystemSet
{
    /// <summary>
    /// 列表数据源
    /// </summary>
    public class ListDataSourceService : DBRepository
    {
        private static ListDataSourceService listDataSourceService = null;
        public static ListDataSourceService Single
        {
            get
            {
                if (listDataSourceService == null)
                {
                    listDataSourceService = new ListDataSourceService();
                }
                return listDataSourceService;
            }
        }

        /// <summary>
        /// 获取列表数据源
        /// </summary>
        /// <param name="page"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public string GetListDataSource(Pagination page, AdminCredential User)
        {
            PageOfDaTaSet result = GetSourceDataSet(page, User);
            if (result.Data.Tables.Count > 0)
            {
                var sumdataset = GetSumDataSet(page, User);
                return JsonHelper.ToListResultJson(result.Data.Tables[0], sumdataset.Data, result.PageCount, result.PageSize, result.Total);
            }
            else
            {
                StringBuilder Json = new StringBuilder();
                Json.Append("{\"Data\":[");
                Json.Append("],\"PagesCount\": " + 0 + ",\"PageSize\": " + 0 + ",\"Total\": " + 0 + "}");
                return Json.ToString();
            }
        }

        /// <summary>
        /// 获取列表数据源
        /// </summary>
        /// <param name="page"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public string GetListDataSource(string sql, Pagination page, AdminCredential User)
        {
            PageOfDaTaSet result = GetSourceDataSet(sql, page, User);
            if (result.Data.Tables.Count > 0)
            {
                var sumdataset = GetSumDataSet(page, User);
                return JsonHelper.ToListResultJson(result.Data.Tables[0], sumdataset.Data, result.PageCount, result.PageSize, result.Total);
            }
            else
            {
                StringBuilder Json = new StringBuilder();
                Json.Append("{\"Data\":[");
                Json.Append("],\"PagesCount\": " + 0 + ",\"PageSize\": " + 0 + ",\"Total\": " + 0 + "}");
                return Json.ToString();
            }
        }

        /// <summary>
        /// 获取列表数据源
        /// </summary>
        /// <param name="page"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public DataSet GetListDataSourceDataSet(Pagination page, AdminCredential admin)
        {
            return GetSourceDataSet(page, admin).Data;
        }

        private PageOfDaTaSet GetSourceDataSet(string sql, Pagination page, AdminCredential admin)
        {
            var db = CreateDao();
            //var _EntityName = db.ExecuteScalar<string>(new Sql("SELECT Name from Sys_entity WHERE ID=" + page.eid));
            //if (sql.IndexOf(_EntityName + ".ID") == -1)
            //{
            //    sql = sql.Replace("SELECT", "SELECT " + _EntityName + ".ID AS " + _EntityName + "$ID,");
            //}
            //if (sql.IndexOf(_EntityName + ".Name") == -1)
            //{
            //    sql = sql.Replace("SELECT", "SELECT " + _EntityName + ".Name AS " + _EntityName + "$Name,");
            //}
            ////关键字
            //sql = AppendSqlForKeyWord(page.KeyWord, page.eid.Value, db, sql);
            //if (page.IsSearch)
            //    page.Page = 1;
            //if (!string.IsNullOrEmpty(page.SortField))
            //{
            //    sql = sql + " Order By " + page.SortField + " " + page.SortType;
            //}
            string sqlStr = AppendListSql(page, admin, db, sql);
            var result = db.DataSetPage(page.Page, page.PageSize, new Sql(sql));
            return result;
        }

        private PageOfDaTaSet GetSourceDataSet(Pagination page, AdminCredential admin)
        {
            using (var db = CreateDao())
            {
                var sql = new Sql();
                sql = sql.Select("[Sql]").From("Sys_view").Where("ID=@0", page.vid);
                var sqlStr = db.ExecuteScalar<string>(sql);
                sqlStr = AppendListSql(page, admin, db, sqlStr);
                PageOfDaTaSet result = db.DataSetPage(page.Page, page.PageSize, new Sql(sqlStr));
                return result;
            }
        }

        private PageOfDaTaSet GetSumDataSet(Pagination page, AdminCredential admin)
        {
            using (var db = CreateDao())
            {
                var ds = new PageOfDaTaSet();
                var sqlStr = CacheHelper.Single.TryGet($"{page.vid}-{admin.ID}-sumdata-sql", 0, () => { return ""; }).ToString();
                if (!string.IsNullOrEmpty(sqlStr))
                {
                    ds = db.DataSet(sqlStr);
                }
                return ds;
            }
        }

        private static string AppendListSql(Pagination page, AdminCredential admin, DBDatabase db, string sql)
        {
            Sys_Entity entity = db.FirstOrDefault<Sys_Entity>(new Sql("SELECT * FROM Sys_Entity WHERE ID=" + page.eid));
            if (sql.IndexOf(entity.Name + ".ID") == -1) sql = sql.Replace("SELECT", "SELECT " + entity.Name + ".ID AS " + entity.Name + "$ID,");
            if (sql.IndexOf(entity.Name + ".Name") == -1) sql = sql.Replace("SELECT", "SELECT " + entity.Name + ".Name AS " + entity.Name + "$Name,");
            //Where条件
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                if (sql.IndexOf("WHERE") != -1) sql += " AND ";
                else sql += " WHERE ";
                sql += page.WhereSql;
            }
            if (page.IsSearch) page.Page = 1;
            //关键字
            sql = AppendSqlForKeyWord(page.KeyWord, page.eid.Value, db, sql);
            //权限过滤
            if (entity.IsEnableDataAuthorize) sql = AppendSqlForAuth(page, admin, sql, entity.Name);
            //Tab
            if (page.pid.HasValue) sql = AppendSqlForTab(page, db, sql, entity.Name);
            //统计数据
            if (entity.IsEnableSumData) CacheHelper.Single.TrySave($"{page.vid}-{admin.ID}-sumdata-sql", GetSumDataSql(entity.ID, db, sql));
            //排序
            var orderSql = GetOrderSql(page, db, entity.Name);
            sql += orderSql;
            //表单分页
            CacheHelper.Single.TrySave($"{page.vid}-{admin.ID}-formpage-sql", GetFormPageSql(entity.Name, orderSql));
            CacheHelper.Single.TrySave($"{page.vid}-{admin.ID}-list-sql", sql);
            return sql;
        }

        /// <summary>
        /// 追加排序Sql
        /// </summary>
        private static string GetOrderSql(Pagination page, DBDatabase db, string entityName)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(page.SortField))
                sql = " Order By " + page.SortField + " " + page.SortType;
            else
            {
                var viewsort = db.ExecuteScalar<string>("SELECT Sort FROM Sys_View WHERE ID=@0", page.vid);
                if (!string.IsNullOrEmpty(viewsort))
                    sql = " Order By " + viewsort;
                else
                    sql = " Order By " + entityName + ".CreateTime desc";
            }
            return sql;
        }

        /// <summary>
        /// 当列表用于Tab时
        /// </summary>
        private static string AppendSqlForTab(Pagination page, DBDatabase db, string sqlStr, string entityName)
        {
            var pentityName = db.ExecuteScalar<string>(new Sql("SELECT Name from Sys_entity WHERE ID=" + page.peid));
            var _pfilename = db.ExecuteScalar<string>("SELECT Name FROM [dbo].[Sys_field] WHERE EntityID=" + page.eid + " AND FieldType='关联其他表' AND RelationEntity='" + pentityName + "'");
            var objModel = entityName + "." + _pfilename + "=" + page.pid.Value;
            if (sqlStr.IndexOf("WHERE") != -1) sqlStr += " AND (";
            else sqlStr += " WHERE (";
            sqlStr += objModel;
            sqlStr += ")";
            return sqlStr;
        }

        /// <summary>
        /// 追加数据权限过滤Sql
        /// </summary>
        private static string AppendSqlForAuth(Pagination page, AdminCredential admin, string sqlStr, string entityName)
        {
            //查看权限控制
            //配置了权限
            if (admin.DataConfig != null && admin.DataConfig.Any(e => e.EntityID == page.eid))
            {
                var ViewRight = admin.DataConfig.Where(e => e.EntityID == page.eid).SingleOrDefault().ViewRight;
                if (ViewRight < 4)
                {
                    if (sqlStr.IndexOf("WHERE") != -1) sqlStr += " AND (";
                    else sqlStr += " WHERE (";
                    switch (ViewRight)
                    {
                        //个人级别
                        case 0:
                        case 1:
                            sqlStr += entityName + ".OwnerID=" + admin.ID;
                            break;
                        //部门
                        case 2:
                            sqlStr += entityName + ".DepartmentID=" + admin.DepartmentID;
                            break;
                        //上下级部门
                        case 3:
                            sqlStr += "charindex(','+rtrim(" + entityName + ".DepartmentID)+',' ," + "," + string.Join(",", admin.ChildDepartmentID) + "," + ")>0";
                            break;
                    }
                    //共享数据
                    sqlStr += " OR ','+" + entityName + ".ShareList+',' LIKE '%," + admin.ID + ",%'";//
                    sqlStr += ")";
                }
            }
            //没有配置权限，默认个人级别
            else
            {
                if (sqlStr.IndexOf("WHERE") != -1) sqlStr += " AND (";
                else sqlStr += " WHERE (";
                sqlStr += entityName + ".OwnerID=" + admin.ID;
                //共享数据
                sqlStr += " OR ','+" + entityName + ".ShareList+',' LIKE '%," + admin.ID + ",%'";//
                sqlStr += ")";
            }
            return sqlStr;
        }

        /// <summary>
        /// 追加关键字筛选Sql
        /// </summary>
        private static string AppendSqlForKeyWord(string keyWord, int entityId, DBDatabase db, string sqlStr)
        {
            if (!string.IsNullOrEmpty(keyWord))
            {
                var sql = new Sql("SELECT Field FROM Sys_field WHERE entityid=" + entityId + " AND IsKeySearch=1");
                List<Sys_Field> listfiled = db.Fetch<Sys_Field>(sql);
                if (listfiled.Count == 0) return string.Empty;
                if (sqlStr.IndexOf("WHERE") != -1) sqlStr += " AND (";
                else sqlStr += " WHERE (";
                int i = 0;
                foreach (var field in listfiled)
                {
                    if (sqlStr.IndexOf(field.Field.Split(new char[] { '@' })[0]) != -1)
                    {
                        if (i++ > 0) sqlStr += " Or ";
                        sqlStr += string.Format("{0} LIKE '%{1}%'", field.Field.Replace("$", "."), keyWord.Trim());
                    }
                }
                sqlStr += ")";
            }
            return sqlStr;
        }

        /// <summary>
        /// 统计数据
        /// </summary>
        private static string GetSumDataSql(int entityId, DBDatabase db, string sqlStr)
        {
            var sql = new Sql("SELECT Field,EntityName,Name FROM Sys_field WHERE entityid=" + entityId + " AND IsEnableSumData=1");
            List<Sys_Field> listfiled = db.Fetch<Sys_Field>(sql);
            if (listfiled.Count == 0) return string.Empty;
            string sumSql = string.Empty;
            int i = 0;
            foreach (var field in listfiled)
            {
                if (i++ > 0) sumSql += ",";
                sumSql += $"SUM({field.EntityName}.{field.Name}) AS {field.Field}";
            }
            sqlStr = Regex.Replace(sqlStr, @"(SELECT[\s\S]*?FROM)", $"SELECT {sumSql} FROM");
            return sqlStr;
        }

        /// <summary>
        /// 表单分页
        /// </summary>
        private static string GetFormPageSql(string entityName, string orderSql)
        {
            return $@"SELECT ID,ROW FROM(SELECT ROW_NUMBER() OVER({orderSql}) ROW, ID FROM {entityName})Z";

        }

    }
}
