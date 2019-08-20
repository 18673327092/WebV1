using Base.Model;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using ORM;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Components;

namespace Base.Service.SystemSet
{
    public class CommonService : DBRepository
    {

        private static CommonService commonService = null;
        public static CommonService Single
        {
            get
            {
                if (commonService == null)
                {
                    commonService = new CommonService();
                }
                return commonService;
            }
        }

        #region 数据验证
        public bool ExistsValueByFieldName(string value, string entityName, string fieldName = "Name")
        {
            using (var db = CreateDao())
            {
                return (db.ExecuteScalar<int>("SELECT COUNT(0) FROM " + entityName + "WHERE " + fieldName + "=@0", value.Trim())) > 0;
            };
        }

        public bool ExistsValue(string name, int entityName)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<int>("SELECT COUNT(0) FROM " + entityName + "WHERE Name=@0", name.Trim()) > 0;
            };
        }

        /// <summary>
        /// 检测Name值是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public int ExistsDictionName(string name, int fieldid)
        {
            var db = CreateDao();
            var list = db.Fetch<int>("SELECT Value FROM Sys_dictionary WHERE FieldID=@0 AND Name=@1", fieldid, name.Trim());
            if (list.Count > 0) { return list.FirstOrDefault(); } else { return -1; }
        }
        #endregion

        #region 其他方法

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetImgSrcByID(int id)
        {
            using (var db = CreateDao())
            {
                return ApplicationContext.Cache.TryGet(string.Format("GetImgSrcByID{0}", id), 0, () =>
                {

                    return db.ExecuteScalar<string>(new Sql(string.Format("SELECT Src FROM Base_Images WHERE ID={0}", id)));
                });
            }
        }

        /// <summary>
        /// 同步图片的使用者字段
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public void AsyncImagesUserDataID(int userId, string ids)
        {
            using (var db = CreateDao())
            {
                if (string.IsNullOrEmpty(ids)) return;
                var list = ids.Split(new char[] { ',' }).ToList();
                list.ForEach(id =>
                {
                    db.Execute(new Sql(string.Format("Update Base_Images SET UserDataID={0} WHERE ID={1}", userId, id)));
                });
            }
        }

        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <returns></returns>
        public string GetFKField_Names(string entityName, string values)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql("SELECT dbo.Get" + entityName + "Names('" + values + "')"));
            }
        }

        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetPageFormData(string entityname, int id)
        {
            var db = CreateDao();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var result = db.DataSetPage(1, 1, new Sql(string.Format("SELECT * FROM {0} WHERE ID={1}", entityname, id)));
            if (result.Data.Tables.Count > 0)
            {
                DataTable dt = result.Data.Tables[0];
                DataRow row = dt.Rows[0];
                foreach (DataColumn column in dt.Columns)
                {
                    dic.Add(column.ColumnName, row[column.ColumnName].ToString());
                }
            }

            db.CloseSharedConnection();
            return dic;
        }




        /// <summary>
        /// 获取表单外键弹框列表要显示的列
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public string GetDialogColumns(string entityName)
        {
            string CacheKey = "DialogColumns-" + entityName;
            return CacheHelper.Single.TryGet(CacheKey, 0, () =>
            {
                var db = CreateDao();
                List<string> list = db.Fetch<string>(new Sql(string.Format("SELECT Name+'$'+Title+'$'+cast(isnull(width,0) as varchar(50)) FROM dbo.Sys_field WHERE EntityName='" + entityName + "' and IsDialogField=1")));
                db.CloseSharedConnection();
                return string.Join(",", list);
            }).ToString();
        }

        public bool UpdateEntityFieldById(string entityname, string fieldname, string value, int id)
        {
            using (var db = CreateDao())
            {
                return db.Execute(new Sql("UPDATE " + entityname + " SET " + fieldname + "=@0 WHERE ID=@1", value, id)) > 0;
            }
        }


        #endregion

        public List<ViewFieldModel> GetExportListFileds(int viewid = 0)
        {
            List<ViewFieldModel> field_list = new List<ViewFieldModel>();
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            if (viewid != 0)
            {
                _sql.Select("FieldList").From("Sys_view").Where("ID=@0", viewid);
                List<Sys_View> list = base.GetList<Sys_View>(_sql);
                var fieldlist = list.FirstOrDefault().FieldList;
                field_list = JsonConvert.DeserializeObject<List<ViewFieldModel>>(fieldlist);
            }
            return field_list;
        }


        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetFKField_Name(string entityname, int id)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql(string.Format("SELECT Name FROM {0} WHERE ID={1}", entityname, id)));
            }
        }

        /// <summary>
        /// 修改某个字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateValue(string entityname, string field, object value, int id)
        {
            using (var db = CreateDao())
            {
                return db.Execute(new Sql("UPDATE " + entityname + " SET " + field + "=@0  WHERE ID=@1", value, id));
            }
        }
    }
}
