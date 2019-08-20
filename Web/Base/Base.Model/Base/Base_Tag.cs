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
    /// 标签实体
    /// </summary>
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_Tag : BaseModel
    {
        /// <summary>
        /// 标签类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 父级标签
        /// </summary>
        public int ParentTag { get; set; }
    }
}
