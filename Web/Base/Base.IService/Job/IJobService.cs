using Base.Model;
using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface IJobService : IBaseService<Sys_Job>
    {
        string GetPagingList(Pagination page);

    }
}
