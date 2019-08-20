using Base.Model;
using Base.Model.Enum;
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
    public class EntityService : BaseService<Sys_Entity>
    {
        private static EntityService entityService = null;
        public static EntityService Single
        {
            get
            {
                if (entityService == null)
                {
                    entityService = new EntityService();
                }
                return entityService;
            }
        }
        /// <summary>
        /// 根据实体名称获取实体ID
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public new int GetEntityID(string entityName)
        {
            string CacheKey = "GetEntityID-" + entityName;
            return Convert.ToInt32(CacheHelper.Single.Get(CacheKey, 0, () =>
            {
                using (var db = CreateDao())
                {
                    return db.ExecuteScalar<int>(new Sql(string.Format("SELECT ID FROM Sys_Entity WHERE Name='{0}'", entityName)));
                }
            }));
        }

        public ListResult<Sys_Entity> GetPagingList(Sys_Entity request, Pagination page)
        {
            string CacheKey = "EntityService-GetPagingList";
            return CacheHelper.Single.TryGet<ListResult<Sys_Entity>>(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("*").From("Sys_entity").Where("IsSystem!=1"); ;
                return base.GetPagingList<Sys_Entity>(_sql, page);
            });
        }

        public ItemResult<Sys_Entity> GetEntityItem(int id)
        {
            var CacheKey = "Sys_Entity" + id;
            return CacheHelper.Single.TryGet(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("*").From("Sys_entity").Where("ID=@0", id);
                return base.Get<Sys_Entity>(id);
            });
        }

        public Sys_Entity GetEntityItem(string name)
        {
            var CacheKey = "GetEntityItem" + name;
            return CacheHelper.Single.TryGet(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("*").From("Sys_entity").Where("Name=@0", name);
                return base.GetList(_sql)?.FirstOrDefault();
            });
        }

        public ListResult<Sys_Entity> GetLookUpEntityList(int id)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_entity").Where("IsSystem!=1");//.Where("ID!=@0", id);
            return base.GetPagingList<Sys_Entity>(_sql, new Pagination() { PageSize = 999, Page = 1 });
        }


        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetFKDropDownSourceData(string entityname)
        {
            var db = CreateDao();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var result = db.DataSetPage(1, 100, new Sql(string.Format("SELECT * FROM {0}", entityname)));
            if (result.Data.Tables.Count > 0)
            {
                DataTable dt = result.Data.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    dic.Add(Convert.ToInt32(row["ID"]), row["Name"].ToString());
                }
            }

            db.CloseSharedConnection();
            return dic;
        }

        /// <summary>
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetFKField_Names(string entityname, string values)
        {
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>(new Sql("SELECT dbo.Get" + entityname + "Names('" + values + "')"));
            }
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
        /// 获取关联字段值的显示名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetAreaName(string entityname)
        {
            using (var db = CreateDao())
            {
                return ApplicationContext.Cache.TryGet("GetAreaName" + entityname, 0, () =>
                 {
                     return db.ExecuteScalar<string>(new Sql(string.Format("SELECT AreaName FROM Sys_entity WHERE Name={0}", entityname)));
                 });
            }
        }
    }
}
