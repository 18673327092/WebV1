using CRM.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    /// <summary>
    /// 短信配置实体
    /// </summary>
    [TableName("SMSConfig")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_SMSConfig : BaseModel
    {
          [DataMember]
        /// <summary>
        /// 短信平台
        /// </summary>
        public int smsplatform { get; set; }

        /// <summary>
        /// 短信帐号
        /// </summary>
        [DataMember]
        public string smsaccount { get; set; }

        /// <summary>
        /// 短信密码
        /// </summary>
        [DataMember]
        public string smspassword { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public int isenable { get; set; }
    }

}
