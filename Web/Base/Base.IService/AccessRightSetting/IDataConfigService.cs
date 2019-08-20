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
    /// 数据权限配置
    /// </summary>
    public interface IDataConfigService : IBaseService<Sys_DataConfig>
    {
        ItemResult<int> InsertDataConfig(List<Sys_DataConfig> list);
    }
}
