using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    [TableName("Sys_Config")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class SysConfig : BaseModel
    {
        public string NewKey { get; set; }
        public string NewValue { get; set; }
    }
}
