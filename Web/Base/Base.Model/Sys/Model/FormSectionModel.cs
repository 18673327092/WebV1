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
    /// 表单-节
    /// </summary>
    [Serializable]
    [DataContract]
    public class FormSectionModel
    {
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 列数量
        /// </summary>
        [DataMember]
        public int Colspan { get; set; }

        /// <summary>
        ///关联表单
        /// </summary>
        [DataMember]
        public int FormID { get; set; }

        [DataMember]
        [Ignore]
        public List<FormFieldModel> Fields { get; set; }
    }
}
