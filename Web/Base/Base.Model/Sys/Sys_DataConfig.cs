using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    /// <summary>
    /// 数据权限配置实体
    /// </summary>
    [TableName("Sys_DataConfig")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_DataConfig
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        /// <summary>
        /// 查看权限
        /// </summary>
        [DataMember]
        public int ViewRight { get; set; }

        /// <summary>
        /// 编辑权限
        /// </summary>

        [DataMember]
        public int EditRight { get; set; }

        /// <summary>
        /// 删除权限
        /// </summary>

        [DataMember]
        public int DeleteRight { get; set; }

        [DataMember]
        public int RoleID { get; set; }

        [ResultColumn]
        public string ShowName { get; set; }

    }
}
