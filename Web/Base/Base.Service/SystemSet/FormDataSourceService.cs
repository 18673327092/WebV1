using ORM;
using PetaPoco;
using System.Collections.Generic;
using System.Data;

namespace Base.Service.SystemSet
{
    public class FormDataSourceService : DBRepository
    {
        private static FormDataSourceService formDataSourceService = null;
        public static FormDataSourceService Single
        {
            get
            {
                if (formDataSourceService == null)
                {
                    formDataSourceService = new FormDataSourceService();
                }
                return formDataSourceService;
            }
        }
        public Dictionary<string, string> GetDataSource(string entityName, int id)
        {
            var db = CreateDao();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var result = db.DataSetPage(1, 1, new Sql(string.Format("SELECT * FROM {0} WHERE ID={1}", entityName, id)));
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
        /// 获取选项集的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetDictionaryNames(string values, int fieldid)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql("SELECT dbo.GetDictionaryNames('" + values + "'," + fieldid + ")"));
            }
        }
    }
}
