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
    public class MenuService : BaseService<Sys_Menu>, IMenuService
    {
        IFieldService fieldService;
        public MenuService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_Menu> GetPagingList(Sys_Menu request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_menu");
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                _sql.Where(page.WhereSql);
            }
            return base.GetPagingList<Sys_Menu>(_sql, page);
        }

        /// <summary>
        /// 获取菜单第一条数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        public Sys_Menu GetDefaultByMenuID(int menuId)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_Menu").Where("ID=@0", menuId);
            List<Sys_Menu> list = base.GetList<Sys_Menu>(_sql);
            return list.FirstOrDefault();
        }


    }
}
