using Base.Model;
using System;
using System.Runtime.Serialization;
namespace JDYD.Model
{
    /// <summary>
    /// 客户
    /// </summary>
    public class New_Customer : BaseModel
    {

        /// <summary>
        /// 关联操作员
        /// </summary>
        [DataMember]
        public int AdminUserID { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
    }
}