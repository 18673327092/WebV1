


using Base.Model;
using Utility;
using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model.Sys.Model;

namespace Base.IService
{
    public interface IFieldService : IBaseService<Sys_Field>
    {

        /// <summary>
        /// 列表显示列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        ListResult<Sys_Field> GetGridFields(int entityId, int currentityid, int UserID = 0);

        /// <summary>
        /// 表单显示列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        ListResult<Sys_Field> GetFormFields(int entityId);

        /// <summary>
        /// 表单隐藏列
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        ListResult<Sys_Field> GetFormHideFields(int entityId);

        /// <summary>
        /// 分页获取字段集合
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        ListResult<Sys_Field> GetPagingList(Sys_Field request, Pagination page);

        /// <summary>
        /// 获取所有字段集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ListResult<Sys_Field> GetAllFieldList(Sys_Field request);

    }
}
