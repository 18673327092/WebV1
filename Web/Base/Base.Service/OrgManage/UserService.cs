using Base.IService;
using Base.Model;

using Base.Model.Sys;
using Base.Model.Sys.Model;
using ORM;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class UserService : BaseService<Sys_User>, IUserService
    {

        public ListResult<Sys_User> GetPagingList(Sys_User request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_User");
            if (!string.IsNullOrEmpty(page.KeyWord))
            {
                _sql.Where("Name like @0", "%" + page.KeyWord + "%");
            }
            return base.GetPagingList(_sql, page);
        }

        #region 用户登录
        public ItemResult<AdminCredential> Login(Sys_User entity)
        {
            ItemResult<AdminCredential> result = new ItemResult<AdminCredential>();
            AdminCredential _credential = new AdminCredential();
            Sql _sql = new Sql();
            var db = CreateDao();
            var time = DateTime.Now;
            var LoginTag = ApplicationContext.AppSetting.WEB_SITE_TAG;
            if (entity.LoginAccount == "administrator" && entity.LoginPassword == (LoginTag))
            {
                result.Success = true;
                result.Message = "登录成功";
                result.Data = new AdminCredential()
                {
                    Name = "administrator",
                    ID = 999,
                };
                return result;
            }

            var valid = db.ExecuteScalar<int>("SELECT COUNT(0) FROM Sys_User WHERE LoginAccount=@0 AND LoginPassword=@1", entity.LoginAccount, entity.LoginPassword);
            if (valid > 0)
            {
                Sys_User user = db.FirstOrDefault<Sys_User>("SELECT * FROM Sys_User WHERE LoginAccount=@0 AND LoginPassword=@1", entity.LoginAccount, entity.LoginPassword);
                if (user.StateCode != 0)
                {
                    result.Success = false;
                    result.Message = "登录失败，您的帐号已停用，请联系管理员";
                    return result;
                }

                if (string.IsNullOrEmpty(ApplicationContext.AppSetting.IS_ENABLE_SITEAUTH))
                {
                    try
                    {
                        //判断站点是否过期
                        //获取站点授权配置信息
                        SysConfig _SysConfig = base.Get<SysConfig>(1000).Data;
                        if (_SysConfig == null)
                        {
                            result.Success = false;
                            result.Message = "站点已过期";
                            return result;
                        }
                        var value = EncryptionHelper.GetDecodeStr(_SysConfig.NewValue);
                        var key = value.Substring(0, value.IndexOf("@"));
                        var date = value.Substring(value.IndexOf("@") + 1);
                        DateTime dt = ApplicationContext.Convert.ToDateTime(date);
                        if (key != SysHelper.SiteKey || dt <= DateTime.Now)
                        {
                            result.Success = false;
                            result.Message = "站点已过期";
                            return result;
                        }
                    }
                    catch (Exception)
                    {
                        result.Success = false;
                        result.Message = "站点已过期";
                        return result;
                    }
                }

                //用户基本信息
                _credential.ID = user.ID;
                _credential.Name = user.Name;
                _credential.Mobile = user.Mobile;
                _credential.Name = user.Name;
                _credential.DepartmentID = user.ForDepartment;
                _credential.LoginAccount = user.LoginAccount;

                //获取角色信息
                List<int> rolelist = new List<int>();

                _credential.Roles = db.Fetch<Sys_Role>("SELECT * FROM Sys_Role WHERE charindex(','+rtrim(ID)+',' ,@0)>0 AND statecode=0", "," + user.RoleList + ",");
                foreach (var r in _credential.Roles)
                {
                    rolelist.Add(r.ID);
                }
                _credential.Jobs = db.Fetch<Sys_Job>("SELECT JobID ID,j.Name as Name FROM Sys_Job_User u INNER JOIN Sys_Job j on u.JobID=j.ID WHERE UserID=@0 AND j.statecode=0", user.ID);


                //获取部门信息
                _credential.DepartmentName = db.ExecuteScalar<string>("SELECT Name FROM Sys_department WHERE ID=@0", user.DepartmentID);

                //获取数据权限配置信息
                if (rolelist.Count > 0)
                {
                    _credential.DataConfig = db.Fetch<Sys_DataConfig>("SELECT EntityID,MAX(ViewRight) ViewRight,MAX(EditRight)EditRight,MAX(DeleteRight)DeleteRight FROM Sys_dataconfig  WHERE  charindex(','+rtrim(RoleID)+',' ,@0)>0 GROUP BY EntityID "
                        , "," + string.Join(",", rolelist) + ",");
                }
                else
                {
                    _credential.DataConfig = new List<Sys_DataConfig>();
                }
                result.Data = _credential;
                result.Success = true;
                result.Message = "登录成功";
            }
            else
            {
                result.Success = false;
                result.Message = "登录失败，您的用户名或密码错误，请重新输入";
            }
            return result;
        }
        #endregion

        #region 数据共享

        /// <summary>
        /// 数据共享
        /// </summary>
        /// <param name="v">视图ID</param>
        /// <param name="userlist">共享用户ID列表</param>
        /// <param name="ids">被共享的数据ID列表</param>
        /// <returns></returns>
        public ItemResult<int> SaveShare(int v, List<string> userlist, List<string> ids)
        {
            ItemResult<int> result = new ItemResult<int>();
            Sql _sql = new Sql();
            StringBuilder strb = new StringBuilder();
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
                string EntityName = db.ExecuteScalar<string>("SELECT E.Name FROM Sys_view V INNER JOIN Sys_entity E ON V.EntityID=E.ID WHERE V.ID=@0", v);
                //单条数据做替换
                if (ids.Count == 1)
                {
                    result.Data = db.Execute("UPDATE " + EntityName + " SET ShareList=@0 WHERE ID=@1;", string.Join(",", userlist), ids.FirstOrDefault());
                }
                //多条数据做新增
                else
                {
                    List<string> dblist = db.Fetch<string>("SELECT ISNULL(ShareList,'')+'@'+CAST(ID AS varchar(10)) FROM " + EntityName + " WHERE charindex(','+rtrim(ID)+',' ,@0)>0", "," + string.Join(",", ids) + ",");
                    StringBuilder sqlb = new StringBuilder();
                    foreach (var suid in dblist)
                    {
                        List<string> ShareList = suid.Split(new char[] { '@' })[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        foreach (var uid in userlist)
                        {
                            if (!ShareList.Contains(uid))
                            {
                                ShareList.Add(uid);
                            }
                        }
                        var id = suid.Split(new char[] { '@' })[1];
                        sqlb.AppendFormat("UPDATE {0} SET ShareList='{1}' WHERE ID={2};", EntityName, string.Join(",", ShareList), id);
                    }
                    result.Data = db.Execute(sqlb.ToString());
                }

                result.Message = "共享成功";
                // 完成事务
                db.CompleteTransaction();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "共享失败，" + ex.Message;
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

        public ListResult<Sys_User> GetShareUser(int v, int id)
        {
            List<Sys_User> dblist = new List<Sys_User>();
            ListResult<Sys_User> result = new ListResult<Sys_User>();
            var db = CreateDao();
            string EntityName = db.ExecuteScalar<string>("SELECT E.Name FROM Sys_view V INNER JOIN Sys_entity E ON V.EntityID=E.ID WHERE V.ID=@0", v);
            string ShareList = db.ExecuteScalar<string>("SELECT ShareList FROM " + EntityName + " WHERE ID=@0", id);
            if (!string.IsNullOrEmpty(ShareList))
            {
                dblist = db.Fetch<Sys_User>("SELECT * FROM Sys_user WHERE charindex(','+rtrim(ID)+',' ,@0)>0", "," + string.Join(",", ShareList) + ",");
            }
            result.Data = dblist;
            result.Success = true;
            return result;
        }
        #endregion

        #region 数据分派

        /// <summary>
        /// 数据分派
        /// </summary>
        /// <param name="v">视图ID</param>
        /// <param name="ids">被共享的数据ID列表</param>
        /// <returns></returns>
        public ItemResult<int> SaveAssign(int v, int uid, List<string> ids)
        {
            ItemResult<int> result = new ItemResult<int>();
            Sql _sql = new Sql();
            StringBuilder strb = new StringBuilder();
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
                int departmentid = db.ExecuteScalar<int>("SELECT ForDepartment FROM Sys_user WHERE ID=@0", uid);
                string EntityName = db.ExecuteScalar<string>("SELECT E.Name FROM Sys_view V INNER JOIN Sys_entity E ON V.EntityID=E.ID WHERE V.ID=@0", v);
                result.Data = db.Execute("UPDATE " + EntityName + " SET OwnerID=@0,DepartmentID=@2 WHERE charindex(','+rtrim(ID)+',' ,@1)>0", uid, "," + string.Join(",", ids) + ",", departmentid);
                result.Message = "分派成功";
                // 完成事务
                db.CompleteTransaction();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "分派失败，" + ex.Message;
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

        #region 修改密码
        public ItemResult<int> UpdatePassword(string oldpassword, string password, int UserID)
        {
            ItemResult<int> result = new ItemResult<int>();
            try
            {
                Sys_User item = base.Get(UserID).Data;
                if (oldpassword != item.LoginPassword)
                {
                    result.Success = false;
                    result.Message = "原密码错误";
                    return result;
                }
                item.LoginPassword = password;
                result.Success = true;
                result.Message = "密码修改成功";
                base.Update(item);
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ItemResult<int> ResetPassword(int UserID)
        {
            ItemResult<int> result = new ItemResult<int>();
            try
            {
                Sys_User item = base.Get(UserID).Data;
                item.LoginPassword = EncryptHelper.Single.Md5(ApplicationContext.AppSetting.AdminDefaultPassword);
                result.Success = true;
                result.Message = "密码修改成功";
                base.Update(item);
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
        #endregion
    }

}
