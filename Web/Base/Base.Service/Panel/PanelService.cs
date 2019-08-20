using Base.IService;
using Base.Model;
using Base.Model.Sys;
using Base.Model.Sys.Model;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public class PanelService : BaseService<Sys_Panel>, IPanelService
    {
        IFieldService fieldService;
        public PanelService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_Panel> GetPagingList(Sys_Panel request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        public ListResult<Sys_Panel> GetDataList(List<int> ids, AdminCredential User)
        {
            ListResult<Sys_Panel> result = new ListResult<Sys_Panel>();
            result.Data = new List<Sys_Panel>();
            var db = CreateDao();
            List<Sys_Panel> dblist = base.GetAll().Where(e => ids.Contains(e.ID)).ToList();
            foreach (var item in dblist)
            {
                if (item.Sql.IndexOf("@auth") != -1)
                {
                    item.Sql = item.Sql.Replace("@auth",  GetAuthSql(db, User, item.EntityID));
                }
                var panel = new Sys_Panel()
                {
                    ID = item.ID,
                    Num = db.ExecuteScalar<int>(item.Sql),
                    Name = item.Name,
                    Sort = item.Sort,
                    Link = item.Link
                };
                result.Data.Add(panel);
            }
            db.CloseSharedConnection();
            return result;
        }

        public string GetAuthSql(DBDatabase db, AdminCredential User, int EntityID)
        {
            if (EntityID == 0) return string.Empty;
            string _sql = string.Empty;
            var _EntityName = db.ExecuteScalar<string>(new Sql("SELECT Name from Sys_entity WHERE ID=" + EntityID));
            if (User.ID != 999)
            {
                //查看权限控制
                //配置了权限
                if (User.DataConfig.Any(e => e.EntityID == EntityID))
                {
                    var ViewRight = User.DataConfig.Where(e => e.EntityID == EntityID).SingleOrDefault().ViewRight;
                    if (ViewRight < 4)
                    {
                        _sql += " AND (";
                        switch (ViewRight)
                        {
                            //个人级别
                            case 0:
                            case 1:
                                _sql += _EntityName + ".OwnerID=" + User.ID;
                                break;
                            //部门
                            case 2:
                                _sql += _EntityName + ".DepartmentID=" + User.DepartmentID;
                                break;
                            //上下级部门
                            case 3:
                                _sql += "charindex(','+rtrim(" + _EntityName + ".DepartmentID)+',' ," + "," + string.Join(",", User.ChildDepartmentID) + "," + ")>0";
                                break;
                        }
                        //共享数据
                        _sql += " OR ','+" + _EntityName + ".ShareList+',' LIKE '%," + User.ID + ",%'";//
                        _sql += ")";
                    }
                }
                //没有配置权限，默认个人级别
                else
                {
                    _sql += " AND (";
                    _sql += _EntityName + ".OwnerID=" + User.ID;
                    //共享数据
                    _sql += " OR ','+" + _EntityName + ".ShareList+',' LIKE '%," + User.ID + ",%'";//
                    _sql += ")";
                }
            }
            return _sql;
        }

    }
}
