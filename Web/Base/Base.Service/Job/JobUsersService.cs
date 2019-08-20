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
    public class JobUsersService : BaseService<Sys_Job_User>, IJobUsersService
    {
        IFieldService fieldService;
        public JobUsersService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        /// <summary>
        /// 根据Sql,获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Sys_Job_User> GetPagingList(Pagination page)
        {
            List<string> fid = new List<string>();
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_Job_User");

            List<Sys_Job_User> list = base.GetList<Sys_Job_User>(_sql);
            return list;
        }


        public ItemResult<int> SetJobs(int uid, string jobsid, string jobsname)
        {
            ItemResult<int> item = new ItemResult<int>();
            var db = CreateDao();
            bool isKeepConnectionAlive = db.KeepConnectionAlive;
            try
            {
                // 检查此前是否已经保持连接存活状态，如果没有，则进行设置
                if (!isKeepConnectionAlive)
                    // 保持连接存活状态
                    db.KeepConnectionAlive = true;
                // 开始事务
                db.BeginTransaction();
                string sql = "delete from Sys_Job_User where UserID=" + uid + "";
                if (db.Execute(sql) >= 0)
                {
                    if (!string.IsNullOrEmpty(jobsid))
                    {
                        string[] arrl = jobsid.Split(new char[] { ',' }).ToArray();
                        for (int i = 0; i < arrl.Length; i++)
                        {
                            Sys_Job_User Sys_job = new Sys_Job_User();
                            Sys_job.UserID = uid;
                            Sys_job.JobID = Convert.ToInt32(arrl[i]);
                            db.Insert(Sys_job);
                        }
                    }
                    db.Execute("UPDATE Sys_User SET Jobs=@0 WHERE ID=@1", jobsname, uid);
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

        ///查询用户下的所有岗位
        public List<Sys_Job_User> SelectAllJob(int Uid)
        {
            try
            {
                var db = CreateDao();
                Sql _sql = new Sql();
                _sql.Select("a.ID, a.UserID, a.JobID,b.Name as JobName").From("dbo.Sys_Job_User as a inner join dbo.Sys_Job as b on a.JobID=b.ID");
                _sql.Where("a.UserID=" + Uid + "");
                List<Sys_Job_User> list = base.GetList<Sys_Job_User>(_sql);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ListResult<Sys_Job_User> GetPagingList(Sys_Job_User request, Pagination page)
        {
            throw new NotImplementedException();
        }
    }
}
