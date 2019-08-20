using Base.Model;
using System;
using System.Runtime.Serialization;
namespace JDYD.Model
{
    /// <summary>
    /// 部门
    /// </summary>
    public class New_DpMent : BaseModel
    {

        /// <summary>
        /// 上级部门
        /// </summary>
        [DataMember]
        public int ParentID { get; set; }
    }
}