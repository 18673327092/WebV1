using Base.Model;
using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface IRoleService : IBaseService<Sys_Role>
    {
        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        Sys_Role GetDefaultByRoleID(int menuId);

        string GetPagingList(Pagination page);

     
    }
}
