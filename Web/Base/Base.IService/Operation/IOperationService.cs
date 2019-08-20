using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model;

namespace Base.IService
{
    public interface IOperationService : IBaseService<Sys_Operation>
    {
         ListResult<Sys_Operation> GetList(Sys_Operation request);
        /// <summary>
        /// 获取权限按钮第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        Sys_Operation GetDefaultByOperationID(int operationId);
    }
}
