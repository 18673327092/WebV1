using Base.IService;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Model;

namespace Base.Service
{
    public class SysConfigService : BaseService<SysConfig>, ISysConfigService
    {
        IFieldService fieldService;
        public SysConfigService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<SysConfig> GetPagingList(SysConfig request, Pagination page)
        {
            return base.GetPagingList(page);
        }
    }
}