using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model;

namespace Base.IService
{
    public interface IUserAndRoleService : IBaseService<Sys_User_Role>
    {
        //List<Sys_UserAndRole> GetPagingList(Pagination page);

        ItemResult<int> SetRoles(int Uid, string Roles, string rolesname);

        /// <summary>
        /// 查询用户下的所有角色
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        List<Sys_User_Role> SelectAllRole(int Uid);
    }
}
