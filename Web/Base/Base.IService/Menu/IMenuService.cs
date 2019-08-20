using Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface IMenuService : IBaseService<Sys_Menu>
    {
        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        Sys_Menu GetDefaultByMenuID(int menuId);
    }
}
