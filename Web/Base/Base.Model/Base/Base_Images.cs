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
    /// 图片
    /// </summary>
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Base_Images : BaseModel
    {
        /// <summary>
        ///图片地址
        /// </summary>
        [DataMember]
        public string Src { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        [DataMember]
        public string Size { get; set; }

        /// <summary>
        /// 原图地址
        /// </summary>
        [DataMember]
        public string OriginalSrc { get; set; }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        [DataMember]
        public string ThumbnailSrc { get; set; }

        [DataMember]
        public string UserAreaName { get; set; }
        
        /// <summary>
        /// 图片使用者实体名
        /// </summary>
        [DataMember]
        public string UserEntityName { get; set; }

        /// <summary>
        /// 图片使用者字段名	
        /// </summary>
        [DataMember]
        public string UserFieldName { get; set; }

        /// <summary>
        /// 图片使用者数据ID
        /// </summary>
        [DataMember]
        public int UserDataID { get; set; }

        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        [Ignore]
        [DataMember]
        public bool IsCreateThumbnail { get; set; }

        /// <summary>
        /// 是否保留原图
        /// </summary>
        [Ignore]
        [DataMember]
        public bool IsSaveOriginalGraph { get; set; }
    }
}
