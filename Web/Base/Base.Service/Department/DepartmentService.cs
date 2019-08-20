using Base.IService;
using Base.Model;
using PetaPoco;
using System.Collections.Generic;
using System.Linq;
using Utility;
using Utility.ResultModel;

namespace Base.Service
{
    public class DepartmentService : BaseService<Sys_Department>, IDepartmentService
    {
        IFieldService fieldService;
        public DepartmentService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_Department> GetPagingList(Sys_Department request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public Sys_Department GetDefaultByBusinessID(int businessId)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_department").Where("ID=@0", businessId);
            List<Sys_Department> list = base.GetList<Sys_Department>(_sql);
            return list.FirstOrDefault();
        }
    }
}
