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
    /// 视图实体
    /// </summary>
    [TableName("Sys_View")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_View
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int? EntityID { get; set; }
        [DataMember]
        public string Sql { get; set; }
        [DataMember]
        public string FieldList { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public int OwnerID { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string Express { get; set; }
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public bool? IsMenu { get; set; }

    }
}
