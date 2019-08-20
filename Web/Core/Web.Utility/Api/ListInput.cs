using System.ComponentModel;

namespace Web.Utility.Api
{
    public class ListInput<T> : BaseInput<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Description("当前页")]
        public int PageIndx { get; set; } = 1;

        /// <summary>
        /// 每页显示数（默认：10）
        /// </summary>
        [Description("每页显示数（默认：10）")]
        public int PageSize { get; set; } = 10;
    }
}