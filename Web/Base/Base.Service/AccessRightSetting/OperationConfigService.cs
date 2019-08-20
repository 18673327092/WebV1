using Base.IService;
using Base.Model;
using PetaPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using Utility.Components;
using Utility.ResultModel;

namespace Base.Service
{
    public class OperationConfigService : BaseService<Sys_OperationConfig>, IOperationConfigService
    {
        private static Hashtable objCache = new Hashtable();
        private static OperationConfigService _operationConfigService;
        public static OperationConfigService Instance
        {
            get
            {
                if (_operationConfigService == null) { _operationConfigService = new OperationConfigService() { }; }
                return _operationConfigService;
            }
        }

        public ListResult<Sys_OperationConfig> GetPagingList(Sys_OperationConfig request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("Sys_operationconfig.*,Sys_menu.MenuName").From("Sys_operationconfig");
            _sql.LeftJoin("Sys_menu").On("Sys_menu.ID=Sys_operationconfig.MenuID");
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                _sql.Where(page.WhereSql);
            }
            return base.GetPagingList<Sys_OperationConfig>(_sql, new Pagination() { PageSize = 999, Page = 1 });
        }

        public ItemResult<int> InsertOperationConfig(List<Sys_OperationConfig> listSys_OperationConfig)
        {
            var db = CreateDao();
            ItemResult<int> item = new ItemResult<int>();
            Sql _sql = new Sql();
            StringBuilder strb = new StringBuilder();
            bool isKeepConnectionAlive = db.KeepConnectionAlive;
            try
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则进行设置
                if (!isKeepConnectionAlive)
                    // 保持连接存活状态
                    db.KeepConnectionAlive = true;
                // 开始事务
                db.BeginTransaction();
                db.Execute(new Sql(("DELETE FROM Sys_operationconfig WHERE RoleID=" + listSys_OperationConfig.FirstOrDefault().RoleID)));
                foreach (Sys_OperationConfig entity in listSys_OperationConfig)
                {
                    entity.Operations = "," + entity.Operations + ",";
                    db.Insert(entity);
                }
                CacheHelper.Single.RemoveAll();
                // 完成事务
                db.CompleteTransaction();
                item.Success = true;
            }
            catch (Exception ex)
            {
                item.Message = ex.Message;
                item.Success = false;
                // 中断事务
                db.AbortTransaction();
            }
            finally
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则关闭数据库连接
                if (!isKeepConnectionAlive)
                {
                    // 关闭连接存活状态
                    db.KeepConnectionAlive = false;
                    // 关闭数据库连接
                    db.CloseSharedConnection();
                }
            }
            return item;
        }

        #region 菜单
        /// <summary>
        /// 获取权限范围内的菜单
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public ListResult<Sys_Menu> GetAccessMenus(List<Sys_Role> roles, int UserID = 0)
        {
            if (UserID == 999)
            {
                return GetAllMenus();
            }
            List<int> ids = new List<int>();
            foreach (var item in roles)
            {
                ids.Add(item.ID);
            }
            return CacheHelper.Single.TryGet<ListResult<Sys_Menu>>("GetAccessMenus-" + string.Join("_", ids), 0, () =>
               {
                   List<int> menuidlist = new List<int>();
                   ListResult<Sys_Menu> result = new ListResult<Sys_Menu>();
                   if (roles == null || roles.Count == 0) { return result; }
                   List<Sys_Menu> menus = new List<Sys_Menu>();
                   var db = CreateDao();
                   Sql _menusql = new Sql();
                   _menusql.Select("*").From("Sys_Menu").Where("ParentID=0");
                   var pmenulist = base.GetList<Sys_Menu>(_menusql);
                   Sql _sql = new Sql();
                   _sql.Append("SELECT m.ID,MenuName,MenuUrl,Icon,RoleID,ParentID,Sort,IsNoMenu FROM Sys_operationconfig oc INNER JOIN [dbo].[Sys_menu] m on oc.MenuID=m.ID WHERE  Operations LIKE '%,0,%'");
                   var list = base.GetList<Sys_Menu>(_sql);
                   List<Sys_Menu> menusauth = new List<Sys_Menu>();
                   foreach (var role in roles)
                   {
                       foreach (var item in list)
                       {
                           if (item.RoleID == role.ID)
                           {
                               if (!menuidlist.Contains(item.ID))
                               {
                                   menuidlist.Add(item.ID);
                                   menusauth.Add(item);
                               }
                           }
                       }
                   }
                   foreach (var pmenu in pmenulist)
                   {
                       //父菜单包含在授权菜单中
                       if (menusauth.Any(e => e.ID == pmenu.ID))
                       {
                           if (!menus.Contains(pmenu))
                           {
                               if (pmenu.ChildMenuList != null)
                               {
                                   foreach (var cm in pmenu.ChildMenuList)
                                   {
                                       cm.ChildMenuList = menusauth.Where(e => e.ParentID == cm.ID).OrderByDescending(e => e.Sort).ToList();
                                       foreach (var zm in cm.ChildMenuList)
                                       {
                                           zm.ChildMenuList = menusauth.Where(e => e.ParentID == zm.ID).OrderByDescending(e => e.Sort).ToList();
                                       }
                                   }
                               }
                               menus.Add(pmenu);
                           }
                       } //父菜单的子集菜单包含在授权菜单中
                       else if (menusauth.Any(e => e.ParentID == pmenu.ID))
                       {
                           pmenu.ChildMenuList = menusauth.Where(e => e.ParentID == pmenu.ID).OrderByDescending(e => e.Sort).ToList();
                           if (!menus.Contains(pmenu))
                           {
                               if (pmenu.ChildMenuList != null)
                               {
                                   foreach (var cm in pmenu.ChildMenuList)
                                   {
                                       cm.ChildMenuList = menusauth.Where(e => e.ParentID == cm.ID).OrderByDescending(e => e.Sort).ToList();
                                   }
                               }
                               menus.Add(pmenu);
                           }
                       }
                   }
                   result.Data = menus.OrderByDescending(e => e.Sort).ToList().Where(e => e.IsDug == false && e.IsHide == false).ToList();
                   return result;
               });
        }

        public bool ValidControllerNameEqMenuID(int menuid, string controllername, string actionName, out int _menuid)
        {
            _menuid = menuid;
            var db = CreateDao();
            if (menuid == 0)
            {
                var isexists = db.ExecuteScalar<int>(new Sql("SELECT COUNT(0) FROM Sys_Menu WHERE MenuUrl LIKE '%/" + controllername + "/%'")) > 0;
                if (isexists)
                {
                    _menuid = db.ExecuteScalar<int>(new Sql("SELECT ID FROM Sys_Menu WHERE MenuUrl LIKE '%/" + controllername + "/%'"));
                }
                else
                {
                    _menuid = -1;
                    return true;
                }
            }
            return db.ExecuteScalar<int>(new Sql("SELECT COUNT(0) FROM Sys_Menu WHERE MenuUrl LIKE '%" + controllername + "%' AND ID=" + _menuid)) > 0;
        }

        /// <summary>
        /// 获取菜单所属区域
        /// </summary>
        /// <returns></returns>
        public string GetAreaIdByUrl(string menuUrl)
        {
            string result = string.Empty;
            using (var db = CreateDao())
            {
                Sql _menusql = new Sql();
                menuUrl = menuUrl.Replace("seller/", "mall/");
                var sql = _menusql.Select("*").From("Sys_Menu").Where("MenuUrl LIKE @0", "%" + menuUrl + "%");
                var menuEntity = db.FirstOrDefault<Sys_Menu>(_menusql);
                if (menuEntity != null)
                {
                    result = menuEntity.MenuArea;
                    if (string.IsNullOrEmpty(menuEntity.MenuArea) || menuEntity.MenuArea == ",")
                    {
                        _menusql = new Sql();
                        sql = _menusql.Select("MenuArea").From("Sys_Menu").Where("id=@0", menuEntity.ParentID);
                        result = db.FirstOrDefault<string>(sql);
                    }

                }
                return result;
            }
        }
        #endregion

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public ListResult<Sys_Menu> GetAllMenus()
        {
            List<int> menuidlist = new List<int>();
            ListResult<Sys_Menu> result = new ListResult<Sys_Menu>();
            List<Sys_Menu> menus = new List<Sys_Menu>();
            var db = CreateDao();
            Sql _menusql = new Sql();
            _menusql.Select("*").From("Sys_Menu").Where("IsHide=0");
            var menulist = base.GetList<Sys_Menu>(_menusql);
            foreach (var menu in menulist.Where(e => e.ParentID == 0))
            {
                if (menulist.Any(e => e.ParentID == menu.ID))
                {
                    menu.ChildMenuList = menulist.Where(e => e.ParentID == menu.ID).OrderByDescending(e => e.Sort).ToList();
                    foreach (var cm in menu.ChildMenuList)
                    {
                        cm.ChildMenuList = menulist.Where(e => e.ParentID == cm.ID).OrderByDescending(e => e.Sort).ToList();
                    }
                }
                if (!menus.Contains(menu))
                {
                    menus.Add(menu);
                }
            }
            result.Data = menus.OrderByDescending(e => e.Sort).OrderByDescending(e => e.Sort).ToList();
            return result;
        }

        /// <summary>
        /// 获取所有菜单区域
        /// </summary>
        /// <returns></returns>
        public ListResult<Sys_MenuArea> GetMenuAreas()
        {
            ListResult<Sys_MenuArea> result = new ListResult<Sys_MenuArea>();
            using (var db = CreateDao())
            {
                Sql sql = new Sql();
                sql.Select("*").From("Sys_MenuArea");
                result.Data = base.GetList<Sys_MenuArea>(sql);
            }
            return result;
        }


        #region 操作按钮
        public ListResult<Sys_Operation> GetAccessOperations(List<Sys_Role> roles, int menuid, string controllername = "", int UserID = 0)
        {
            List<int> ids = new List<int>();
            if (roles != null)
            {
                foreach (var item in roles)
                {
                    ids.Add(item.ID);
                }
            }
            string CacheKey = "GetAccessOperations-" + string.Join("_", ids) + menuid + controllername;
            return CacheHelper.Single.TryGet<ListResult<Sys_Operation>>(CacheKey, 0, () =>
             {
                 ListResult<Sys_Operation> result = new ListResult<Sys_Operation>();
                 List<int> rolesid = new List<int>();
                 if (roles != null)
                 {
                     foreach (var role in roles)
                     {
                         rolesid.Add(role.ID);
                     }
                     List<int> menuidlist = new List<int>();
                     // if (roles == null || roles.Count == 0) { return result; }
                 }

                 List<Sys_Menu> menus = new List<Sys_Menu>();
                 var db = CreateDao();
                 if (menuid == 0 && !string.IsNullOrEmpty(controllername))
                 {
                     var ishavemenu = db.ExecuteScalar<int>("SELECT COUNT(0) FROM Sys_Menu WHERE MenuUrl LIKE '%/" + controllername + "/%'") > 0;
                     if (ishavemenu)
                         menuid = db.ExecuteScalar<int>("SELECT TOP 1 ID FROM Sys_Menu WHERE MenuUrl LIKE '%/" + controllername + "/%'");
                 }
                 List<int> operations = new List<int>();
                 Sql _sql = new Sql();
                 if (UserID == 999 && menuid == 0)
                 {
                     _sql.Append("SELECT * FROM Sys_operationconfig");
                 }
                 else if (UserID == 999)
                 {
                     _sql.Append("SELECT * FROM Sys_operationconfig WHERE  MenuID=@0", menuid);
                 }
                 else
                 {
                     _sql.Append("SELECT * FROM Sys_operationconfig WHERE charindex(','+rtrim(RoleID)+',' ,@0)>0 AND MenuID=@1", "," + string.Join(",", rolesid) + ",", menuid);
                 }
                 var list = base.GetList<Sys_OperationConfig>(_sql);
                 if (list != null)
                 {
                     foreach (var item in list)
                     {
                         var opts = item.Operations.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                         foreach (var opid in opts)
                         {
                             var id = Convert.ToInt32(opid);
                             if (!operations.Contains(id))
                             {
                                 operations.Add(id);
                             }
                         }
                     }
                     var operationobjs = base.GetAll<Sys_Operation>().ToList().Where(e => operations.Contains(e.ID) && e.StateCode == 0).ToList().OrderByDescending(e => e.Sort).OrderBy(e => e.Type).ToList();
                     result.Data = operationobjs;
                 }
                 db.Dispose();
                 return result;
             });
        }

        public ListResult<Sys_Operation> GetAllOperations()
        {
            var result = new ListResult<Sys_Operation>();
            var operationobjs = base.GetAll<Sys_Operation>().ToList().OrderByDescending(e => e.Sort).OrderBy(e => e.Type).ToList();
            result.Data = operationobjs;
            return result;
        }

        #endregion
    }
}
