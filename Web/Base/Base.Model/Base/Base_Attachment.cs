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
    /// 附件实体
    /// </summary>
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_Attachment : BaseModel
    {
        /// <summary>
        ///附件
        /// </summary>
        [DataMember]
        public string Attachment { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        [DataMember]
        public string Size { get; set; }
    }
}
