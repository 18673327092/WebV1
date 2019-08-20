using Base.Model.Sys.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Base.Model
{
    /// <summary>
    /// 表单
    /// </summary>
    [TableName("Sys_Form")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Form
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 关联实体
        /// </summary>
        [DataMember]
        public int EntityID { get; set; }

        /// <summary>
        /// 是否主表单
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }

        [DataMember]
        public string FormShowFields { get; set; }

        [DataMember]
        public string FormHideFields { get; set; }

        [Ignore]
        public List<FormSectionModel> FormShowFieldsDB { get; set; }
        [Ignore]
        public List<int> FormHideFieldsDB { get; set; }

    }
}
