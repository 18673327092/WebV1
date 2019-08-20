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
    /// 日程
    /// </summary>
    [TableName("Sys_Schedule")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Schedule : BaseModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime? starttime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime? endtime { get; set; }
    }
}
