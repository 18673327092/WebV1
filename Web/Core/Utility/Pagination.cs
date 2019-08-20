namespace Utility
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class Pagination
    {
        public Pagination()
        {
            Page = 1;
            PageSize = 10;
        }
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }
        public string SortType { get; set; }

        public bool IsPreview { get; set; }
        public string KeyWord { get; set; }
        public string Filter { get; set; }
        public string WhereSql { get; set; }
        public bool IsSearch { get; set; }
        public string sql { get; set; }

        /// <summary>
        /// 实体ID
        /// </summary>
        public int? eid { get; set; }
        /// <summary>
        /// 视图ID
        /// </summary>
        public int? vid { get; set; }

        /// <summary>
        /// 外键ID
        /// </summary>
        public int? pid { get; set; }
        public int? peid { get; set; }

    }
}
