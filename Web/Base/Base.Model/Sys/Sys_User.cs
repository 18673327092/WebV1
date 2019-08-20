using Base.Model;
using System;
using System.Runtime.Serialization;
namespace Base.Model
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class Sys_User : BaseModel
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string LoginAccount { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [DataMember]
        public string LoginPassword { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 所属岗位
        /// </summary>
        [DataMember]
        public string Jobs { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [DataMember]
        public int ForDepartment { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [DataMember]
        public string RoleList { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public int Sex { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public int DpMentID { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        [DataMember]
        public int AccountType { get; set; }
    }
}