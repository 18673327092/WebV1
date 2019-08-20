using Base.Model;
using Utility;
using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface IJobUsersService : IBaseService<Sys_Job_User>
    {
        List<Sys_Job_User> GetPagingList(Pagination page);

        ItemResult<int> SetJobs(int Uid, string Roles, string rolesname);

        /// <summary>
        /// 查询用户下的所有角色
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        List<Sys_Job_User> SelectAllJob(int Uid);
    }
}
