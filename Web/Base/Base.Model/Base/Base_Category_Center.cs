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
    public class Base_Category_Center
    {
        [DataMember]
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        [DataMember]
        /// <summary>
        /// 类别id
        /// </summary>
        public int C_ID { get; set; }

        [DataMember]
        /// <summary>
        /// 其他关联表id
        /// </summary>
        public int Q_ID { get; set; }

        [ResultColumn]
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CName { get; set; }
    }
}
