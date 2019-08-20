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
    /// 部门实体
    /// </summary>
    [TableName("Sys_Department")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Department : BaseModel
    {
        /// <summary>
        /// 上级部门
        /// </summary>
        [DataMember]
        public int? ParentOrgID { get; set; }
    }
}
