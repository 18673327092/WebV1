using Base.Model;
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
    /// 分类
    /// </summary>
    [TableName("Base_Category")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_Category : BaseModel
    {
        /// <summary>
        /// 父级ID
        /// </summary>
        [DataMember]
        public int ParentID { get; set; }

        [DataMember]
        /// <summary>
        ///层级
        /// </summary>
        public int Level { get; set; }

        [DataMember]
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        [DataMember]
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

    }
}
