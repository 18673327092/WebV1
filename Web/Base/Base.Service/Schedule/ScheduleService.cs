using Base.IService;
using Base.Model;

using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class ScheduleService : BaseService<Sys_Schedule>, IScheduleService
    {
        public ListResult<Sys_Schedule> GetPagingList(Sys_Schedule request, Pagination page)
        {
            Sql sql = new Sql();
            sql.Select("*").From("Sys_Schedule").Where("OwnerID=@0", request.OwnerID);
            if (request.starttime.HasValue)
            {
                sql.Where("starttime>=@0", request.starttime.Value);
            }
            if (request.endtime.HasValue)
            {
                sql.Where("endtime<=@0", request.endtime.Value);
            }
            return base.GetPagingList(sql, page);
        }
    }
}
