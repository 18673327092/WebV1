using Utility.ResultModel;
using System;
using System.Collections.Generic;
using Base.Model;
using Base.Model.Sys.Model;

namespace Base.IService
{
    public interface IUserService : IBaseService<Sys_User>
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ItemResult<AdminCredential> Login(Sys_User entity);

        /// <summary>
        /// 数据共享
        /// </summary>
        /// <param name="v">视图ID</param>
        /// <param name="userlist">共享用户ID列表</param>
        /// <param name="ids">被共享的数据ID列表</param>
        /// <returns></returns>
        ItemResult<int> SaveShare(int v, List<string> userlist, List<string> ids);

        ListResult<Sys_User> GetShareUser(int v, int id);

        ItemResult<int> SaveAssign(int v, int uid, List<string> ids);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpassword"></param>
        /// <param name="password"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        ItemResult<int> UpdatePassword(string oldpassword, string password, int UserID);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        ItemResult<int> ResetPassword(int UserID);
    }
}
