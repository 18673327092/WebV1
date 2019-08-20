using Base.Service;
using Utility;
using Utility.ResultModel;
using JDYD.Model;
using JDYD.IService;

namespace JDYD.Service
    {
        /// <summary>
        /// 客户
        /// </summary>
        public class CustomerService : BaseService<New_Customer>, ICustomerService
        {
            public ListResult<New_Customer> GetPagingList(New_Customer request, Pagination page)
            {
                return base.GetPagingList(page);
            }
        }
    }