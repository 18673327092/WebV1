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
    /// 基础实体
    /// </summary>
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class BaseModel
    {
        #region 基本字段

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [DataMember]
        public int OwnerID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public int CreateUserID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [DataMember]
        public int UpdateUserID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int StateCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        DateTime _UpdateTime = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get { return _UpdateTime; } set { value = _UpdateTime; } }

        /// <summary>
        /// 数据所属部门
        /// </summary>
        [DataMember]
        public int DepartmentID { get; set; }

        /// <summary>
        /// 数据共享列表
        /// </summary>
        [DataMember]
        public string ShareList { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }


        #endregion
    }
}
