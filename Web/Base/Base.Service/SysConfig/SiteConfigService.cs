using Base.IService;
using Base.Model;
using Utility;
using Utility.ResultModel;

namespace Base.Service
{
    /// <summary>
    /// 网站配置
    /// </summary>
    public class SiteConfigService : BaseService<Sys_SiteConfig>, ISiteConfigService
        {
            public ListResult<Sys_SiteConfig> GetPagingList(Sys_SiteConfig request, Pagination page)
            {
                return base.GetPagingList(page);
            }
        }
    }