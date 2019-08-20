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
    ///操作权限配置
    /// </summary>
    [TableName("Sys_OperationConfig")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_OperationConfig
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int RoleID { get; set; }

        [DataMember]
        public int MenuID { get; set; }



        [ResultColumn]
        public string MenuName { get; set; }

        [DataMember]
        public string Operations { get; set; }
    }
}
