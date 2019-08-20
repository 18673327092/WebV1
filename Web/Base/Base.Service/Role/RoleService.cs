using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class RoleService : BaseService<Sys_Role>, IRoleService
    {
        IFieldService fieldService;
        public RoleService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_Role> GetPagingList(Sys_Role request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        public new ItemResult<int> Delete(List<int> primaryKeyList)
        {
            ItemResult<int> result = new ItemResult<int>();
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
                foreach (var id in primaryKeyList)
                {
                    //删除操作权限配置
                    db.Execute("DELETE FROM Sys_operationconfig WHERE RoleID=@0", id);
                    //删除数据权限配置
                    db.Execute("DELETE FROM Sys_dataconfig WHERE RoleID=@0", id);
                    base.DeleteByEntityId(id);
                }
                // 完成事务
                db.CompleteTransaction();
                result.Message = "角色删除成功";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
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



        /// <summary>
        /// 根据Sql,获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public new string GetPagingList(Pagination page)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_Role");
            if (page.KeyWord != "")
            {
                _sql.Where("Name like '%" + page.KeyWord + "%'");
            }
            var db = CreateDao();
            var result = db.DataSetPage(page.Page, page.PageSize, _sql);
            return JsonHelper.ToListResultJson(result.Data.Tables[0], result.Page, result.PageSize, result.Total);
        }

        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public Sys_Role GetDefaultByRoleID(int RoleId)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_Role").Where("ID=@0", RoleId);
            List<Sys_Role> list = base.GetList<Sys_Role>(_sql);
            return list.FirstOrDefault();
        }
    }
}
