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
    public class DataConfigService : BaseService<Sys_DataConfig>, IDataConfigService
    {
        public ListResult<Sys_DataConfig> GetPagingList(Sys_DataConfig request, Pagination page)
        {
            Sql _sql = new Sql();
            _sql.Select("ViewRight,EditRight,DeleteRight,Sys_entity.ID 'EntityID',RoleID,Sys_entity.ShowName").From("Sys_entity");
            _sql.LeftJoin("Sys_dataconfig").On("Sys_entity.ID=Sys_dataconfig.EntityID AND RoleID="+request.RoleID+"");
            _sql.Where("Sys_entity.IsSystem!=1 and IsHide!=1");
            if (!string.IsNullOrEmpty(page.WhereSql))
            {
                _sql.Where(page.WhereSql);
            }
            return base.GetPagingList<Sys_DataConfig>(_sql, new Pagination() { PageSize = 999, Page = 1 });
        }

        public ItemResult<int> InsertDataConfig(List<Sys_DataConfig> list)
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
                db.Execute(new Sql(("DELETE FROM Sys_dataconfig WHERE RoleID=" + list.FirstOrDefault().RoleID)));
                foreach (Sys_DataConfig entity in list)
                {
                    db.Insert(entity);
                }

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
    }
}
