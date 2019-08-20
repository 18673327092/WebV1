using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_User_Role")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    /// <summary>
    /// 用户角色关联实体
    /// </summary>
    public class Sys_User_Role
    {
        [DataMember]
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        [DataMember]
        /// <summary>
        /// 用户ID
        /// </summary>
        public int U_ID { get; set; }

        [DataMember]
        /// <summary>
        /// 角色ID
        /// </summary>
        public int R_ID { get; set; }

        [ResultColumn]
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoName { get; set; }
    }
}
