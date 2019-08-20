using Base.Model;
using Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model.Sys.Model;

namespace Base.IService
{
    public interface IReportService : IBaseService<Sys_Report>
    {
        string GetReportColumns(int id);
        string GetReportData(Pagination page, AdminCredential User);
        List<SearchField> GetReportParameters(int id);
        DataTable GetExportReportData(Pagination page, AdminCredential User);

    }
}
