using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    /// <summary>
    /// 菜单区域
    /// </summary>
    [TableName("Sys_MenuArea")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_MenuArea : BaseModel
    {
        [DataMember]
        public string Icon { get; set; }

        [DataMember]
        public string BJClass { get; set; }

        [DataMember]
        public int SiteID { get; set; }
    }
}
