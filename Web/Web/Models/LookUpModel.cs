using Base.Model.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LookUpModel
    {
        /// <summary>
        /// 参数
        /// </summary>
        public LookUpParameter Parameter { get; set; }

        /// <summary>
        /// 实体ID
        /// </summary>
        public int EntityID { get; set; }

        /// <summary>
        ///实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 视图ID
        /// </summary>
        public int ViewID { get; set; }

        /// <summary>
        /// 列表筛选控件集合
        /// </summary>
        public List<SearchField> SearchFieldList { get; set; }

        /// <summary>
        /// 列表显示字段
        /// </summary>
        public string ListFieldsJSON { get; set; }

        /// <summary>
        ///已选中值
        /// </summary>
        public List<RelationEntityField> SelectValue { get; set; }

    }
}