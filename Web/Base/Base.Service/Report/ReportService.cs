using Base.IService;
using Base.Model;
using Base.Model.Sys;
using Base.Model.Sys.Model;
using ORM;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using Newtonsoft.Json;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model.Enum;
using Base.Model.Base.Model;

namespace Base.Service
{
    public class ReportService : BaseService<Sys_Report>, IReportService
    {
        IFieldService fieldService;
        public ReportService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_Report> GetPagingList(Sys_Report request, Pagination page)
        {
            return base.GetPagingList(page);
        }


        /// <summary>
        /// 获取报表显示列
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetReportColumns(int id)
        {
            DataSet ds = new DataSet();
            ItemResult<ReportModel> result_report = new ItemResult<ReportModel>();
            //ReportModel item = new ReportModel();
            var db = CreateDao();
            var sql = new Sql();
            sql.Select("*").From("Report").Where("ID=@0", id);
            Sys_Report report = base.Get<Sys_Report>(id).Data;
            var result = new PageOfDaTaSet();
            result = db.DataSet(report.ColumnsSql);
            db.CloseSharedConnection();
            List<ViewFieldModel> columns = new List<ViewFieldModel>();
            foreach (DataColumn column in result.Data.Tables[0].Columns)
            {

                ViewFieldModel field = new ViewFieldModel();
                field.Field = column.ColumnName;
                field.Title = column.ColumnName;
                if (column.ColumnName.IndexOf("__") != -1)
                {
                    field.Field = column.ColumnName.Substring(0, column.ColumnName.IndexOf("__"));
                    field.Title = column.ColumnName.Substring(column.ColumnName.IndexOf("__") + 2);
                    //  field.C = column.ColumnName.Substring(column.ColumnName.IndexOf("__") + 2);
                }
                field.Width = 100;
                var length = field.Title.Length;
                if (length > 5) { field.Width = length * 20 - 20; }
                if (column.ColumnName == "peta_rn")
                {
                    field.Title = "序号";
                    field.Width = 50;
                }
                columns.Add(field);
            }
            return JsonConvert.SerializeObject(columns);
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public string GetReportData(Pagination page, AdminCredential User)
        {
            DataSet ds = new DataSet();
            var result = GetExportDataDataTable(page, User);
            if (result.Data != null && result.Data.Tables.Count > 0)
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

        private PageOfDaTaSet GetExportDataDataTable(Pagination page, AdminCredential User)
        {
            Sys_Report report = base.Get<Sys_Report>(page.vid).Data;
            if (page.IsSearch)
            {
                page.Page = 1;
            }
            var _sql = report.DataSource;
            var db = CreateDao();
            if (db.ExecuteScalar<int>("SELECT COUNT(0) FROM Sys_ReportParameters WHERE ReportID=@0", report.ID) > 0 && string.IsNullOrEmpty(page.Filter))
            {
                return new PageOfDaTaSet();
            }
            if (_sql.IndexOf("@auth") != -1)
            {
                _sql = _sql.Replace("@auth", GetAuthSql(db, User, report.EntityID));
            }
            List<object> objs = new List<object>();
            if (!string.IsNullOrEmpty(page.Filter))
            {
                List<KendoUIFilter> filters = JsonConvert.DeserializeObject<List<KendoUIFilter>>(page.Filter);
                foreach (var filter in filters.OrderBy(e => e.sort).ToList())
                {
                    var value = filter.value;
                    switch (Convert.ToInt32(filter.type))
                    {
                        case (int)ParameterTypeEnum.文本:
                            value = "%" + filter.value + "%";
                            break;
                        case (int)ParameterTypeEnum.数字:
                            value = filter.value;
                            break;
                        case (int)ParameterTypeEnum.日期:
                            value = value.Replace("00:00:00", "");
                            if (filter.opera == "<=")
                            {
                                value = filter.value + " 23:59:59";
                            }
                            break;
                        case (int)ParameterTypeEnum.时间:
                            if (filter.opera == "<=")
                            {
                                if (filter.value.IndexOf("00:00:00") != -1)
                                {
                                    value = filter.value.Replace("00:00:00", "");
                                    value = filter.value + " 23:59:59";
                                }
                            }
                            break;
                    }
                    objs.Add(value);
                }
            }
            var sql = new Sql(_sql, objs.ToArray());
            var result = new PageOfDaTaSet();
            if (report.IsPage == 1)
            {
                result = db.DataSetPage(page.Page, page.PageSize, sql);
            }
            else
            {
                result = db.DataSet(_sql, objs.ToArray());
            }
            ApplicationContext.Log.Info("报表 " + report.Name, "Sql：" + _sql + "/ 参数：" + JsonConvert.SerializeObject(objs));
            db.CloseSharedConnection();
            return result;
        }

        public string GetAuthSql(DBDatabase db, AdminCredential User, int EntityID)
        {
            if (EntityID == 0) return string.Empty;
            string _sql = string.Empty;
            var _EntityName = db.ExecuteScalar<string>(new Sql("SELECT Name from Sys_Entity WHERE ID=" + EntityID));
            if (User.ID != 999)
            {
                //查看权限控制
                //配置了权限
                if (User.DataConfig.Any(e => e.EntityID == EntityID))
                {
                    var ViewRight = User.DataConfig.Where(e => e.EntityID == EntityID).SingleOrDefault().ViewRight;
                    if (ViewRight < 4)
                    {
                        _sql += " AND (";
                        switch (ViewRight)
                        {
                            //个人级别
                            case 0:
                            case 1:
                                _sql += _EntityName + ".OwnerID=" + User.ID;
                                break;
                            //部门
                            case 2:
                                _sql += _EntityName + ".DepartmentID=" + User.DepartmentID;
                                break;
                            //上下级部门
                            case 3:
                                _sql += "charindex(','+rtrim(" + _EntityName + ".DepartmentID)+',' ," + "," + string.Join(",", User.ChildDepartmentID) + "," + ")>0";
                                break;
                        }
                        //共享数据
                        _sql += " OR ','+" + _EntityName + ".ShareList+',' LIKE '%," + User.ID + ",%'";//
                        _sql += ")";
                    }
                }
                //没有配置权限，默认个人级别
                else
                {
                    _sql += " AND (";
                    _sql += _EntityName + ".OwnerID=" + User.ID;
                    //共享数据
                    _sql += " OR ','+" + _EntityName + ".ShareList+',' LIKE '%," + User.ID + ",%'";//
                    _sql += ")";
                }
            }
            return _sql;
        }

        /// <summary>
        /// 获取报表参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SearchField> GetReportParameters(int id)
        {
            List<SearchField> SearchFieldList = new List<SearchField>();
            List<Sys_ReportParameters> paramlist = base.GetList<Sys_ReportParameters>(new Sql("SELECT * FROM Sys_ReportParameters WHERE ReportID=@0 Order by Sort", id));
            foreach (var param in paramlist)
            {
                var s = new SearchField()
                {
                    ID = param.FieldID,
                    FieldType = param.ParameterType.ToString(),
                    Name = param.Name,
                    Field = param.ParameterName,
                    Sort = param.Sort,
                    IsMultiple = param.IsMultiple == 1,
                    Title = param.Title
                };
                if (param.ParameterType == (int)ParameterTypeEnum.选项集)
                {
                    s.DictionaryList = base.GetList<Sys_Dictionary>(new Sql("SELECT * FROM Sys_Dictionary WHERE FieldID=@0", param.FieldID) { });
                }
                SearchFieldList.Add(s);
            }
            return SearchFieldList;
        }

        public DataTable GetExportReportData(Pagination page, AdminCredential User)
        {
            page.PageSize = 999999;
            var result = GetExportDataDataTable(page, User);
            if (result.Data.Tables.Count > 0)
            {
                return result.Data.Tables[0];
            }
            else
            {
                return new DataTable();
            }

        }
    }
}
