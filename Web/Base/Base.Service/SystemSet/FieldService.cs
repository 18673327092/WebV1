using Base.IService;
using Base.Model;
using PetaPoco;
using Utility;
using Utility.Components;
using Utility.ResultModel;

namespace Base.Service
{
    public class FieldService : BaseService<Sys_Field>, IFieldService
    {
        private static FieldService fieldService = null;
        public static FieldService Single
        {
            get
            {
                if (fieldService == null)
                {
                    fieldService = new FieldService();
                }
                return fieldService;
            }
        }

        #region 字段

        public ListResult<Sys_Field> GetPagingList(Sys_Field request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_field").Where("StateCode=0 AND EntityID=@0", request.EntityID);
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                _sql.Where(page.WhereSql);
            }
            return base.GetPagingList(_sql, page);
        }

        public ListResult<Sys_Field> GetAllFieldList(Sys_Field request)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_field").Where("StateCode=0 AND EntityID=@0", request.EntityID);
            return base.GetPagingList(_sql, new Pagination() { Page = 1, PageSize = 999 });
        }

        /// <summary>
        /// 获取视图所需的列
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ListResult<Sys_Field> GeViewFieldList(Sys_Field request)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_field").Where("StateCode=0 AND EntityID=@0 AND IsHide<>1", request.EntityID);
            return base.GetPagingList(_sql, new Pagination() { Page = 1, PageSize = 999 });
        }

        /// <summary>
        /// 列表显示列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public ListResult<Sys_Field> GetGridFields(int entityId, int currentityid, int UserID = 0)
        {
            ListResult<Sys_Field> result = new ListResult<Sys_Field>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_field").Where("EntityID=@0 AND IsColumnShow=1 AND IsHide<>1", entityId);//
            if (entityId != currentityid && UserID != 999)
            {
                _sql.Where("FieldType<>'关联其他表' AND FieldType<>'关联其他表多选' AND FieldType<>'选项集' AND FieldType<>'选项集多选' AND FieldType<>'两个选项'");
            }
            return base.GetPagingList(_sql, new Pagination() { Page = 1, PageSize = 999 });
        }

        /// <summary>
        /// 表单显示列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public ListResult<Sys_Field> GetFormFields(int entityId)
        {
            string CacheKey = "GetFormFields-" + entityId;
            return CacheHelper.Single.TryGet<ListResult<Sys_Field>>(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("*").From("Sys_field").Where("EntityID=@0 AND IsFormShow=1", entityId);
                return base.GetPagingList(_sql, new Pagination() { Page = 1, PageSize = 999, SortField = "Sort" });
            });
        }

        /// <summary>
        /// 表单隐藏列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public ListResult<Sys_Field> GetFormHideFields(int entityId)
        {
            string CacheKey = "GetFormHideFields-" + entityId;
            return CacheHelper.Single.TryGet<ListResult<Sys_Field>>(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("ID,Field,Title").From("Sys_field").Where("EntityID=@0 AND ISNULL(IsFormShow,0)=0 AND IsCustomizeControl<>1", entityId);
                return base.GetPagingList(_sql, new Pagination() { Page = 1, PageSize = 999, SortField = "Sort" });
            });
        }


        /// <summary>
        /// 获取字段标题
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public string GetTitle(int entityId, string fieldname)
        {
            string CacheKey = "GetTitle-" + entityId + fieldname;
            return CacheHelper.Single.TryGet(CacheKey, 0, () =>
            {
                Sql _sql = new Sql();
                _sql.Select("Title").From("Sys_field").Where("EntityID=@0 AND Name=@1", entityId, fieldname);
                return base.GetList(_sql)?[0].Title;
            });
        }

        #endregion



    }
}
