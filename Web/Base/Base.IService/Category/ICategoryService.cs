using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Model.Base.Model;
using Base.Model;
using Utility;
using Utility.ResultModel;

namespace Base.IService
{
    public interface ICategoryService : IBaseService<Base_Category>
    {
        ListResult<CategoryTree> GetTreeList(Base_Category request);
        ItemResult<int> Delete(int ID);
        ItemResult<int> Move(int id, int newpId, int sibId, int dir);
    }
}
