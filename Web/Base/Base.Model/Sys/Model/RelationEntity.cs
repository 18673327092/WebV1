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
    /// 外键实体模型
    /// </summary>
    public class FKEntityModel
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ShowName { get; set; }

        [DataMember(Name = "jointablenme")]
        public string JoinTableName { get; set; }

        [DataMember(Name = "onsql")]
        public string OnSql { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public int RelationEntityID { get; set; }

        [DataMember(Name = "fileds")]
        public List<Sys_Field> Fileds { get; set; }
    }
}
