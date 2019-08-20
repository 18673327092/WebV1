using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Data.Model
{
    /// <summary>
    /// 市
    /// </summary>
    [Serializable]
    [DataContract]
    public class CityModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 省份编码
        /// </summary>
        [DataMember]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [DataMember]
        public int ProvinceID { get; set; }
    }
}