using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class OperationService : BaseService<Sys_Operation>, IOperationService
    {
        public ListResult<Sys_Operation> GetPagingList(Sys_Operation request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_operation").Where(page.WhereSql);
            return base.GetPagingList<Sys_Operation>(_sql, new Pagination() { PageSize = 999, Page = 1 });
        }

        public ListResult<Sys_Operation> GetList(Sys_Operation request)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_operation").Where("StateCode!=1");
            if (request.MenuID.HasValue)
            {
                _sql.Where("MenuID=@0", request.MenuID.Value);
            }
            return base.GetPagingList<Sys_Operation>(_sql, new Pagination() { PageSize = 999, Page = 1 });
        }

        /// <summary>
        /// 获取权限按钮第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public Sys_Operation GetDefaultByOperationID(int operationId)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_operation").Where("ID=@0", operationId);
            List<Sys_Operation> list = base.GetList<Sys_Operation>(_sql);
            return list.FirstOrDefault();
        }
    }
}
