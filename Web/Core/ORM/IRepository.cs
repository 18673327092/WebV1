using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    /// <summary>
    /// 用于处理Entity持久化操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 把实体entity添加到数据库
        /// </summary>
        /// <param name="entity">实体</param>
        object Insert(TEntity entity);
        /// <summary>
        /// 把实体entiy更新到数据库
        /// </summary>
        /// <param name="entity">实体</param>
        int Update(TEntity entity);
        /// <summary>
        /// 从数据库删除实体(by 主键)
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        int DeleteByEntityId(object primaryKey);
        /// <summary>
        /// 从数据库删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        int Delete(TEntity entity);
        /// <summary>
        /// 依据主键检查实体是否存在于数据库
        /// </summary>
        /// <param name="primaryKey">主键</param>
        bool Exists(object primaryKey);
        bool Exists(string tableName, string fieldName, string fieldValue);
        /// <summary>
        /// 依据主键获取单个实体
        /// </summary>
        /// <remarks>
        /// 自动对实体进行缓存（除非实体配置为不允许缓存）
        /// </remarks>
        TEntity Get(object primaryKey);
        /// <summary>
        /// 获取所有实体（仅用于数据量少的情况）
        /// </summary>
        /// <remarks>
        /// 自动对进行缓存（缓存策略与实体配置的缓存策略相同）
        /// </remarks>
        List<TEntity> GetAll();
    }
}
