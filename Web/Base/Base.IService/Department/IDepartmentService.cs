using Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface IDepartmentService : IBaseService<Sys_Department>
    {
        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        Sys_Department GetDefaultByBusinessID(int menuId);
    }
}
