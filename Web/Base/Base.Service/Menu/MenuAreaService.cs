using Base.IService;
using Base.Model;
using Base.Model.Sys;
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
    public class MenuAreaService : BaseService<Sys_MenuArea>, IMenuAreaService
    {
        public ListResult<Sys_MenuArea> GetPagingList(Sys_MenuArea request, Pagination page)
        {
            return base.GetPagingList(page);
        }
    }
}
