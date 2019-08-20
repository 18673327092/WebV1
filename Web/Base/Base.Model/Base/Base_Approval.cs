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
    /// 审批流程
    /// </summary>
    [TableName("base_approval")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_Approval : BaseModel
    {
        [DataMember]
        public int approvaltype { get; set; }
    }
}
