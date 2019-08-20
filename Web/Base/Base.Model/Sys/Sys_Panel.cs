using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    /// <summary>
    /// 面板视图
    /// </summary>
    [TableName("Sys_Panel")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Panel : BaseModel
    {
        [DataMember]
        public string Sql { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int Sort { get; set; }

        [Ignore]
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

    }
}
