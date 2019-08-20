using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_ReportParameters")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_ReportParameters : BaseModel
    {
        [DataMember]
        public int FieldID { get; set; }

        [DataMember]
        public int ParameterType { get; set; }

        [DataMember]
        public string ParameterName { get; set; }

        [DataMember]
        public int ReportID { get; set; }

        [DataMember]
        public int Sort { get; set; }

        [DataMember]
        public int IsMultiple { get; set; }

        [DataMember]
        public string Title { get; set; }


    }
}
