using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    /// <summary>
    /// 数据持久基类
    /// </summary>
    /// <typeparam name="TEntity">持久对象类型</typeparam>
    public class Repository<TEntity> : DBRepository, IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">持久对象</param>
        /// <returns></returns>
        public virtual object Insert(TEntity entity)
        {
            return this.CreateDao().Insert(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">持久对象</param>
        /// <returns></returns>
        public virtual int Update(TEntity entity)
        {
            return this.CreateDao().Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual int DeleteByEntityId(object primaryKey)
        {
            return this.CreateDao().Delete<TEntity>(primaryKey);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">持久对象</param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity)
        {
            return this.CreateDao().Delete<TEntity>(entity);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual bool Exists(object primaryKey)
        {
            return this.CreateDao().Exists<TEntity>(primaryKey);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual bool Exists(string tableName, string fieldName, string fieldValue)
        {
            var db = this.CreateDao();
          return db.ExecuteScalar<int>("SELECT count(0) FROM " + tableName + " WHERE " + fieldName + "=@0", fieldValue)>0;
           
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public virtual TEntity Get(object primaryKey)
        {
            return this.CreateDao().SingleOrDefault<TEntity>(primaryKey);
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="sql">sql对象</param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Sql sql)
        {
            using (var db = CreateDao())
            {
                return db.Query<TEntity>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAll()
        {
            return this.CreateDao().Fetch<TEntity>("");
        }

    }
}
