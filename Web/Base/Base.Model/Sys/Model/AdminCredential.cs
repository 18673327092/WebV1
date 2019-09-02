using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    /// <summary>
    ///管理员凭证
    /// </summary>
    public class AdminCredential
    {
        public int ID { get; set; }
        public string LoginAccount { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// 上下级部门ID
        /// </summary>
        public int ChildDepartmentID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 登录站点
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// 登录站点首页链接
        /// </summary>
        public string SiteHomePageLink { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<Sys_Role> Roles { get; set; }

        /// <summary>
        /// 岗位列表
        /// </summary>
        public List<Sys_Job> Jobs { get; set; }

        /// <summary>
        /// 数据权限列表
        /// </summary>
        public List<Sys_DataConfig> DataConfig { get; set; }
    }
}
