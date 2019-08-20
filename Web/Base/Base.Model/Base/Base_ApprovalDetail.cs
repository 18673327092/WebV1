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
    /// 审批流程明细
    /// </summary>
    [TableName("Base_ApprovalDetail")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_ApprovalDetail : BaseModel
    {
        [DataMember]
        public int approval { get; set; }

        [DataMember]
        public int step { get; set; }

        [DataMember]
        public int approvaljob { get; set; }
    }
}
