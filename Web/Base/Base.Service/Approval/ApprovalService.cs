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
    public class ApprovalService : BaseService<Base_Approval>, IApprovalService
    {
        public ListResult<Base_Approval> GetPagingList(Base_Approval request, Pagination page)
        {
            return base.GetPagingList(page);
        }
    }
}
