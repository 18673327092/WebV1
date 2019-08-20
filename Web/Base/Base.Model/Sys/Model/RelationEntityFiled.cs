using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{

    /// <summary>
    /// 关联实体的字段列表
    /// </summary>
    public class RelationEntityField
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        public int FieldID { get; set; }
    }
}
