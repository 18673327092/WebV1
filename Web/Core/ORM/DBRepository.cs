using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class DBRepository
    {
        /// <summary>
        /// 数据库DAO对象键名
        /// </summary>
        private static readonly string Key = "__sql_database__";

        /// <summary>
        /// 数据库DAO对象
        /// </summary>
        public DBDatabase Database
        {
            get
            {
                return (DBDatabase)CallContext.GetData(Key);
            }
            set
            {
                CallContext.SetData(Key, value);
            }
        }

        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        protected virtual DBDatabase CreateDao()
        {
            return this.Database ?? (this.Database = DBDatabase.CreateInstance("ConnectionString"));
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <param name="model">持久对象</param>
        /// <returns></returns>
        public virtual object Insert<TModel>(TModel model)
        {
            return this.CreateDao().Insert(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <param name="model">持久对象</param>
        /// <returns></returns>
        public virtual int Update<TModel>(TModel model)
        {
            return this.CreateDao().Update(model);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual int DeleteByEntityId<TModel>(object primaryKey)
        {
            return this.CreateDao().Delete<TModel>(primaryKey);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <param name="model">持久对象</param>
        /// <returns></returns>
        public virtual int Delete<TModel>(TModel model)
        {
            return this.CreateDao().Delete<TModel>(model);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual bool Exists<TModel>(object primaryKey)
        {
            return this.CreateDao().Exists<TModel>(primaryKey);
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual TModel Get<TModel>(object primaryKey)
        {
            return this.CreateDao().SingleOrDefault<TModel>(primaryKey);
        }

        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <typeparam name="TModel">持久对象类型</typeparam>
        /// <returns></returns>
        public virtual List<TModel> GetAll<TModel>()
        {
            return this.CreateDao().Fetch<TModel>("");
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="sql">sql对象</param>
        /// <returns></returns>
        public virtual List<TModel> GetList<TModel>(Sql sql)
        {
            using (var db = CreateDao())
            {
                return db.Query<TModel>(sql).ToList();
            }
        }
    }
}
