using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.ResultModel
{
    public class ListResult<T> : BaseResult
    {
        public List<T> Data { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PagesCount
        {
            get { return (int)Math.Ceiling((double)Total / PageSize); }
        }

        /// <summary>
        /// 每页显示数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; set; }
    }
}
