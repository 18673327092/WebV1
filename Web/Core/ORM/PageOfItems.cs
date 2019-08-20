using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    [Serializable]
    public class PageOfItems<T> : List<T>, IPageOfItems<T>
    {
        public PageOfItems()
        {
        }

        public PageOfItems(IEnumerable<T> items)
        {
            AddRange(items);
        }

        public PageOfItems(IEnumerable<T> items, int page, int pageSize, int total)
        {
            AddRange(items);
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = total;
        }

        #region IPageOfItems<T> Members

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageCount
        {
            get { return (int)Math.Ceiling((double)Total / PageSize); }
        }
        #endregion
    }

    [Serializable]
    public class PageOfDaTaSet
    {
        public PageOfDaTaSet()
        {
        }

        public PageOfDaTaSet(DataSet items, int page, int pageSize, int total)
        {
            this.Data = items;
            this.Page = page;
            this.PageSize = pageSize;
            this.Total = total;
        }

        #region IPageOfItems<T> Members

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public DataSet Data { get; set; }
        public int PageCount
        {
            get { return (int)Math.Ceiling((double)Total / PageSize); }
        }
        #endregion
    }
}
