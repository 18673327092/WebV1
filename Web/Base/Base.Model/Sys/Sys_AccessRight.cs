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
    /// 权限实体
    /// </summary>
    [TableName("Sys_AccessRight")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_AccessRight
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int OperationID { get; set; }
    }
}
