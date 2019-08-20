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
    ///报表实体
    /// </summary>
    [TableName("Sys_Report")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Report : BaseModel
    {
        [DataMember]
        public string DataSource { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int IsPage { get; set; }

        [DataMember]
        public string ColumnsSql { get; set; }
    }
}
