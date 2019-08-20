using ORM;
using PetaPoco;
using System;
using System.Collections.Generic;
using Utility;
using Utility.ResultModel;

namespace Base.Service
{
    /// <summary>
    ///数据库操作类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TEntity> : Repository<TEntity> where TEntity : class
    {
        #region 数据库底层相关方法

        /// <summary>
        /// 根据传递过来的实体和Sql获得列表信息
        /// </summary>
        /// <typeparam name="TModel">实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="queryParams">分页参数</param>
        /// <returns>列表信息</returns>
        protected ListResult<TEntity> GetPagingList(Sql sql, Pagination page)
        {
            var db = CreateDao();
            var list = new ListResult<TEntity>();
            var _sqlcount = "SELECT COUNT(0) FROM (" + sql.SQL + ") t";
            list.Total = db.ExecuteScalar<int>(new Sql(_sqlcount, sql.Arguments));
            list.PageSize = page.PageSize;
            var _sql = new Sql(sql.SQL, sql.Arguments);
            if (!string.IsNullOrEmpty(page.SortField))
            {
                _sql.OrderBy(page.SortField + " " + page.SortType);
            }
            else
            {
                //CreateTime
            }
            var result = (List<TEntity>)db.Page<TEntity>(page.Page, page.PageSize, _sql);
            list.Data = result;
            list.Success = true;
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 根据传递过来的实体和Sql获得列表信息
        /// </summary>
        /// <typeparam name="TModel">实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="queryParams">分页参数</param>
        /// <returns>列表信息</returns>
        protected ListResult<TEntity> GetPagingList(string sql, Pagination page, params object[] args)
        {
            var db = CreateDao();
            var list = new ListResult<TEntity>();
            var _sqlcount = "SELECT COUNT(0) FROM (" + sql + ") t";
            list.Total = db.ExecuteScalar<int>(new Sql(_sqlcount, args));
            list.PageSize = page.PageSize;
            var _sql = new Sql(sql, args);
            if (!string.IsNullOrEmpty(page.SortField))
            {
                _sql.OrderBy(page.SortField + " " + page.SortType);
            }
            var result = (List<TEntity>)db.Page<TEntity>(page.Page, page.PageSize, _sql);
            list.Data = result;
            list.Success = true;
            db.Dispose();
            return list;
        }


        /// <summary>
        /// 根据传递过来的实体和Sql获得列表信息
        /// </summary>
        /// <typeparam name="TModel">实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="queryParams">分页参数</param>
        /// <returns>列表信息</returns>
        public ListResult<TEntity> GetPagingList(Pagination page)
        {
            var db = CreateDao();

            var viewSql = "";
            if (!page.vid.HasValue && page.eid.HasValue)
            {
                viewSql = db.ExecuteScalar<string>(new Sql("SELECT Sql FROM Sys_view Where EntityID=@0 AND type='系统视图' AND Title like '可用的%'", page.eid.Value));
            }
            else
            {
                viewSql = db.ExecuteScalar<string>(new Sql("SELECT Sql FROM Sys_view Where ID=@0", page.vid.Value));
            }
            var sql = new Sql(viewSql);
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                sql.Where(page.WhereSql);
            }
            var list = new ListResult<TEntity>();
            var _sqlcount = "SELECT COUNT(0) FROM (" + sql.SQL + ") t";
            list.Total = db.ExecuteScalar<int>(new Sql(_sqlcount, sql.Arguments));
            list.PageSize = page.PageSize;
            var _sql = new Sql(sql.SQL, sql.Arguments);
            if (!string.IsNullOrEmpty(page.SortField))
                _sql.OrderBy(page.SortField);
            var result = (List<TEntity>)db.Page<TEntity>(page.Page, page.PageSize, _sql);
            list.Data = result;
            list.Success = true;
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 根据传递过来的实体和Sql获得列表信息
        /// </summary>
        /// <typeparam name="TModel">实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="queryParams">分页参数</param>
        /// <returns>列表信息</returns>
        protected ListResult<Model> GetPagingList<Model>(Sql sql, Pagination page)
        {
            var db = CreateDao();
            var list = new ListResult<Model>();
            var _sqlcount = "SELECT COUNT(0) FROM (" + sql.SQL + ") t";
            list.Total = db.ExecuteScalar<int>(new Sql(_sqlcount, sql.Arguments));
            list.PageSize = page.PageSize;
            var _sql = new Sql(sql.SQL, sql.Arguments);
            if (!string.IsNullOrEmpty(page.SortField))
                _sql.OrderBy(page.SortField);
            var result = (List<Model>)db.Page<Model>(page.Page, page.PageSize, _sql);
            list.Data = result;
            list.Success = true;
            db.Dispose();
            return list;
        }

        protected BaseResult Execute(Sql sql)
        {
            BaseResult result = new BaseResult();
            var db = CreateDao();
            bool isKeepConnectionAlive = db.KeepConnectionAlive;
            try
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则进行设置
                if (!isKeepConnectionAlive)
                    // 保持连接存活状态
                    db.KeepConnectionAlive = true;
                // 开始事务
                db.BeginTransaction();
                db.Execute(sql);
                // 完成事务
                db.CompleteTransaction();
                result.Success = true;
            }
            catch (Exception)
            {
                result.Success = false;
                // 中断事务
                db.AbortTransaction();
            }
            finally
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则关闭数据库连接
                if (!isKeepConnectionAlive)
                {
                    // 关闭连接存活状态
                    db.KeepConnectionAlive = false;
                    // 关闭数据库连接
                    db.CloseSharedConnection();
                }
            }
            return result;
        }

        #endregion

        #region 提供给客户端用的方法

        public new virtual ItemResult<TEntity> Get(object primaryKey)
        {
            ItemResult<TEntity> item = new ItemResult<TEntity>();
            try
            {
                item.Data = base.Get<TEntity>(primaryKey);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        public new ItemResult<Model> Get<Model>(object primaryKey)
        {
            ItemResult<Model> item = new ItemResult<Model>();
            try
            {
                item.Data = base.Get<Model>(primaryKey);
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }
        public new virtual ItemResult<int> Insert(TEntity entity)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = Convert.ToInt32(base.Insert<TEntity>(entity));
                item.Success = true;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        #region 修改
        public new virtual ItemResult<int> Update(TEntity entity)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = base.Update<TEntity>(entity);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        public new virtual ItemResult<int> Update<Model>(Model entity)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = base.Update<Model>(entity);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }
        #endregion

        #region 删除
        public new ItemResult<int> Delete(TEntity entity)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = base.Delete<TEntity>(entity);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }
        public ItemResult<int> Delete(List<int> primaryKeyList)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                int successnum = 0;
                foreach (var id in primaryKeyList)
                {
                    var result = base.DeleteByEntityId(id);
                    if (result > 0) successnum += 1;
                }
                item.Success = true;
                item.Data = successnum;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }
        public new ItemResult<int> DeleteByEntityId(object primaryKey)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = base.DeleteByEntityId(primaryKey);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        public new ItemResult<int> DeleteByEntityId<Model>(object primaryKey)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                item.Data = base.DeleteByEntityId<Model>(primaryKey);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }
        #endregion



        #endregion

        #region 其他方法
        public int GetEntityID(string EntityName)
        {
            var db = CreateDao();
            var entityId = db.ExecuteScalar<int>(new Sql(string.Format("SELECT ID FROM Sys_entity WHERE Name='{0}'", EntityName)));
            db.Dispose();
            return entityId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Proname">存储过程名字</param>
        /// <param name="Parameter">@SerialNoType,@Year,@Month,@SerialNo OUTPUT </param>
        /// <param name="value">SerialNoType = "12", Year = iYear, Month = iMonth, SerialNo = param </param>
        /// <returns></returns>
        public int execpro(string Proname, string Parameter, params object[] args)
        {
            var db = CreateDao();
            int spResult = db.Execute("EXEC " + Proname + " " + Parameter + ""
          , args);
            return spResult;
        }

        /// <summary>
        /// 停用/可用
        /// </summary>
        /// <param name="primaryKeyList"></param>
        /// <returns></returns>
        public ItemResult<int> Disable(string entityname, List<int> primaryKeyList, int StateCode = 1)
        {
            ItemResult<int> item = new ItemResult<int>();
            try
            {
                var db = CreateDao();
                item.Data = db.Execute("UPDATE " + entityname + " SET StateCode=@0 WHERE charindex(','+rtrim(ID)+',' ,@1)>0", StateCode, "," + string.Join(",", primaryKeyList) + ",");
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="primaryKeyList"></param>
        /// <returns></returns>
        public ItemResult<bool> IsUnDisable(string entityname, int id)
        {
            ItemResult<bool> item = new ItemResult<bool>();
            try
            {
                var db = CreateDao();
                item.Data = db.ExecuteScalar<int>("SELECT COUNT(0) FROM " + entityname + " WHERE ID=@0 AND StateCode=0", id) > 0;
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        public ItemResult<string> GetName(string entityname, object primaryKey)
        {
            ItemResult<string> item = new ItemResult<string>();
            try
            {
                var db = CreateDao();
                item.Data = db.ExecuteScalar<string>("SELECT Name FROM " + entityname + " WHERE ID=@0", Convert.ToInt32(primaryKey));
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        public ItemResult<string> GetDictionaryName(int value, int fieldid)
        {
            ItemResult<string> item = new ItemResult<string>();
            try
            {
                var db = CreateDao();
                item.Data = db.ExecuteScalar<string>("SELECT Name FROM Sys_dictionary WHERE Value=@0 AND FieldID=@1", value, fieldid);
                item.Success = true;
                return item;
            }
            catch (Exception ex)
            {
                item.Success = false;
                item.Message = ex.Message;
            }
            return item;
        }

        #endregion
    }
}
