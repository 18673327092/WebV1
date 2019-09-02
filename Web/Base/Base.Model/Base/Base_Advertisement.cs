using Base.Model;
using System;
using System.Runtime.Serialization;
namespace Base.Model
{
    /// <summary>
    /// 广告
    /// </summary>
    public class Base_Advertisement : BaseModel
    {

        /// <summary>
        /// 位置
        /// </summary>
        [DataMember]
        public int Position { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string Link { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [DataMember]
        public string Image { get; set; }
    }
}