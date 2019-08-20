using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public interface IPageOfItems<T> : IList<T>
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int Total { get; set; }
        int PageCount { get; }
    }
}
