using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LookUpParameter
    {
        /// <summary>
        /// 字段FieldID
        /// </summary>
        public int FieldID { get; set; }
        /// <summary>
        /// 已选值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 原字段自定义ID（用于取得返回值）
        /// </summary>
        public int TagID { get; set; }
        /// <summary>
        /// 列表初始高度
        /// </summary>
        public int ListHeight { get; set; }
    }
}