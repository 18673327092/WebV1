using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    /// <summary>
    /// 表单-字段
    /// </summary>
    [Serializable]
    [DataContract]
    public class FormFieldModel
    {

        /// <summary>
        /// 关联字段
        /// </summary>
        [DataMember]
        public int FieldID { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }

        /// <summary>
        ///类型
        /// </summary>
        [DataMember]
        public string FieldType { get; set; }


        /// <summary>
        ///表单验证规则
        /// </summary>
        [DataMember]
        public string Valid { get; set; }


        /// <summary>
        ///显示名称
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        ///备注说明
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        [DataMember]
        public bool? IsReadOnly { get; set; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        [DataMember]
        public bool? IsCustomizeControl { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [DataMember]
        public bool? IsMust { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        public int Y { get; set; }

        [DataMember]
        public string Placeholder { get; set; }

    }
}
