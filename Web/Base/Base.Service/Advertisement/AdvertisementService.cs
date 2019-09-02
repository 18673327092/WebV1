using Base.Service;
using Base.IService;
using Base.Model;
using Utility;
using Utility.ResultModel;

namespace Base.Service
    {
        /// <summary>
        /// 广告
        /// </summary>
        public class AdvertisementService : BaseService<Base_Advertisement>, IAdvertisementService
        {
            public ListResult<Base_Advertisement> GetPagingList(Base_Advertisement request, Pagination page)
            {
                return base.GetPagingList(page);
            }
        }
    }