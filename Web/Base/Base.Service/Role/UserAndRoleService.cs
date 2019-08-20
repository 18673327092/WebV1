using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class UserAndRoleService : BaseService<Sys_User_Role>, IUserAndRoleService
    {
        public ListResult<Sys_User_Role> GetPagingList(Sys_User_Role request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        ///// <summary>
        ///// 根据Sql,获取角色列表
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //public List<Sys_User_Role> GetPagingList(Pagination page)
        //{
        //    List<string> fid = new List<string>();
        //    Sql _sql = new Sql();
        //    _sql.Select("*").From("Sys_User_Role");

        //    List<Sys_User_Role> list = base.GetList<Sys_User_Role>(_sql);
        //    return list;
        //}


        public ItemResult<int> SetRoles(int uid, string rolesid, string rolesname)
        {
            ItemResult<int> item = new ItemResult<int>();
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
                string sql = "delete from Sys_User_Role where U_ID=" + uid + "";
                if (db.Execute(sql) >= 0)
                {
                    if (!string.IsNullOrEmpty(rolesid))
                    {
                        string[] arrl = rolesid.Split(new char[] { ',' }).ToArray();
                        for (int i = 0; i < arrl.Length; i++)
                        {
                            Sys_User_Role Sys_role = new Sys_User_Role();
                            Sys_role.U_ID = uid;
                            Sys_role.R_ID = Convert.ToInt32(arrl[i]);
                            base.Insert(Sys_role);
                        }
                    }
                    db.Execute("UPDATE Sys_User SET Roles=@0 WHERE ID=@1", rolesname, uid);
                }
                // 完成事务
                db.CompleteTransaction();
                item.Success = true;
            }
            catch (Exception ex)
            {
                item.Message = ex.Message;
                item.Success = false;
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
            return item;
        }

        ///查询用户下的所有角色
        public List<Sys_User_Role> SelectAllRole(int Uid)
        {
            try
            {
                var db = CreateDao();
                Sql _sql = new Sql();
                _sql.Select("a.id, a.U_ID, a.R_ID,b.Name as RoName").From("dbo.Sys_User_Role as a inner join dbo.Sys_role as b on a.R_ID=b.ID");
                _sql.Where("a.U_ID=" + Uid + "");
                List<Sys_User_Role> list = base.GetList<Sys_User_Role>(_sql);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
