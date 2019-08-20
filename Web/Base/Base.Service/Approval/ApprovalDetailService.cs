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
    public class ApprovalDetailService : BaseService<Base_ApprovalDetail>, IApprovalDetailService
    {
        public ListResult<Base_ApprovalDetail> GetPagingList(Base_ApprovalDetail request, Pagination page)
        {
            Sql sql = new Sql();
            sql.Select("*").From("Base_Approvaldetail");
            if (request.approval > 0)
            {
                sql.Where("approval=@0", request.approval);
            }
            return base.GetPagingList(sql,page);
        }
    }
}
