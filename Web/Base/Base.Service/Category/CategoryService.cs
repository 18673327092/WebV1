using Base.IService;
using Base.Model;
using Base.Model.Sys;
using ORM;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model.Base.Model;

namespace Base.Service
{
    /// <summary>
    /// 分类 业务逻辑类
    /// </summary>
    public class CategoryService : BaseService<Base_Category>, ICategoryService
    {
        public ListResult<Base_Category> GetPagingList(Base_Category request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Base_Category");
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                _sql.Where(page.WhereSql);
            }
            return base.GetPagingList<Base_Category>(_sql, page);
        }

        public ListResult<CategoryTree> GetTreeList(Base_Category request)
        {
            Sql _sql = new Sql();
            _sql.Select("ID AS id,ParentID as pId,Name as name,(case when Level=2 then 0 else 1 end) as 'open'").From("Base_Category");
            return base.GetPagingList<CategoryTree>(_sql, new Pagination() { Page = 1, PageSize = 999, SortField = "sort desc" });
        }

        public new ItemResult<int> Insert(Base_Category entity)
        {
            var result = new ItemResult<int>();
            using (var db = CreateDao())
            {
                db.Execute("exec proc_category_add @0,@1,@2,@3 OUTPUT", entity.Name, entity.ParentID, entity.Type, entity.ID);
                result.Success = true;
            }
            return result;
        }

        public ItemResult<int> Delete(int ID)
        {
            var result = new ItemResult<int>();
            using (var db = CreateDao())
            {
                db.Execute("exec proc_category_delete @0", ID);
                result.Success = true;
            }
            return result;
        }

        public ItemResult<int> Move(int id, int newpId, int sibId, int dir)
        {
            var result = new ItemResult<int>();
            using (var db = CreateDao())
            {
                db.Execute("exec proc_category_updateSort @0,@1,@2,@3", id, newpId, sibId, dir);
                result.Success = true;
            }
            return result;
        }
    }
}
