using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class ReportParametersService : BaseService<Sys_ReportParameters>, IReportParametersService
    {
        IFieldService fieldService;
        public ReportParametersService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_ReportParameters> GetPagingList(Sys_ReportParameters request, Pagination page)
        {
            return base.GetPagingList(page);
        }

    }
}
