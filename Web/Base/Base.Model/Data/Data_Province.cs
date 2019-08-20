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
    /// 省/直辖市
    /// </summary>
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Data_Province : BaseModel
    {
        /// <summary>
        /// 省份编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }
    }
}
