using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Model;
using Base.Model.Sys.Model;

namespace Base.IService
{
    public interface IPanelService : IBaseService<Sys_Panel>
    {
        ListResult<Sys_Panel> GetDataList(List<int> ids, AdminCredential User);
    }
}
