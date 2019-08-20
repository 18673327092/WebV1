using Base.Model;
using ORM;
using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    /// <summary>
    /// 操作按钮权限配置
    /// </summary>
    public interface IOperationConfigService : IBaseService<Sys_OperationConfig>
    {
        ItemResult<int> InsertOperationConfig(List<Sys_OperationConfig> list);

        ListResult<Sys_Menu> GetAccessMenus(List<Sys_Role> roles, int UserID = 0);

        ListResult<Sys_Menu> GetAllMenus();

        ListResult<Sys_Operation> GetAccessOperations(List<Sys_Role> roles, int menuid, string controllername = "", int UserID = 0);

        ListResult<Sys_Operation> GetAllOperations();
    }
}
