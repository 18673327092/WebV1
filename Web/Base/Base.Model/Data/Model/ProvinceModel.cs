using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Model.Data.Model
{
    /// <summary>
    /// 省/直辖市
    /// </summary>
    [Serializable]
    [DataContract]
    public class ProvinceModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 省份编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }
    }
}
