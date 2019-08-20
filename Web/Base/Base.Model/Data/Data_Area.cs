using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    /// <summary>
    /// 地区 区域
    /// </summary>
    public class Data_Area : BaseModel
    {
        /// <summary>
        /// 区域编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        [DataMember]
        public string CityCode { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [DataMember]
        public int CityID { get; set; }
    }
}