using Base.Service;
using JDYD.IService;
using JDYD.Model;
using Utility;
using Utility.ResultModel;

namespace JDYD.Service
    {
        /// <summary>
        /// 部门
        /// </summary>
        public class DpMentService : BaseService<New_DpMent>, IDpMentService
        {
            public ListResult<New_DpMent> GetPagingList(New_DpMent request, Pagination page)
            {
                return base.GetPagingList(page);
            }
        }
    }