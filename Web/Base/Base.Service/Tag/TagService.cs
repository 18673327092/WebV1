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
    /// <summary>
    /// 标签业务类
    /// </summary>
    public class TagService : BaseService<Base_Tag>, ITagService
    {
        public ListResult<Base_Tag> GetPagingList(Base_Tag request, Pagination page)
        {
            return base.GetPagingList(page);
        }
    }
}
