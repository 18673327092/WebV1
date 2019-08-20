using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_User")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_User : BaseModel
    {

        [DataMember]
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginAccount { get; set; }

        [DataMember]
        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPassword { get; set; }

        [Ignore]
        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPasswordEdit { get; set; }

        [DataMember]
        /// <summary>
        /// 业务部门
        /// </summary>
        public int ForDepartment { get; set; }

        [DataMember]
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        [DataMember]
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public int Sex { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        [DataMember]
        public string RoleList { get; set; }

        [DataMember]
        public string Jobs { get; set; }


        /// <summary>
        /// 价格权限
        /// </summary>
        public string PriceAuthority { get; set; }

    }
}
