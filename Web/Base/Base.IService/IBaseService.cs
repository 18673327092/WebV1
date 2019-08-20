using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.ResultModel;

namespace Base.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns>实体列表</returns>
        List<TEntity> GetAll();

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>实体对象</returns>
        ItemResult<TEntity> Get(object primaryKey);

        /// <summary>
        /// 获取Name
        /// </summary>
        /// <param name="entityname">表名</param>
        /// <param name="primaryKey">主键</param>
        /// <returns>Name</returns>
        ItemResult<string> GetName(string entityname, object primaryKey);

        /// <summary>
        /// 根据FieldID和Value值获取选项卡的显示值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldid"></param>
        /// <returns></returns>
        ItemResult<string> GetDictionaryName(int value, int fieldid);


        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="request">实体对象</param>
        /// <param name="page">分页对象</param>
        /// <returns>封装后的实体列表</returns>
        ListResult<TEntity> GetPagingList(TEntity request, Pagination page);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>新增ID</returns>
        ItemResult<int> Insert(TEntity entity);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响数据条数</returns>
        ItemResult<int> Update(TEntity entity);

        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>影响数据条数</returns>
        ItemResult<int> DeleteByEntityId(object primaryKey);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        ItemResult<int> Delete(TEntity entity);

        ItemResult<int> Delete(List<int> primaryKeyList);

        /// <summary>
        ///停用
        /// </summary>
        /// <param name="primaryKeyList"></param>
        /// <returns></returns>
        ItemResult<int> Disable(string entityname, List<int> primaryKeyList, int StateCode = 1);

        /// <summary>
        /// 判断数据是否可用
        /// </summary>
        /// <param name="entityname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ItemResult<bool> IsUnDisable(string entityname, int id);
     
    }
}
