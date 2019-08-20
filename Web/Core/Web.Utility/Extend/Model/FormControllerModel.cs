using Base.Model.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Utility.Extend.Model
{
    public class FormControllerModel
    {
        /// <summary>
        ///字段ID
        /// </summary>
        public int FieldID { get; set; }
        /// <summary>
        /// 当前数据ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 控件属性
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 控件Class
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        public FormFieldModel FormField { get; set; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool? IsCustomize { get; set; }

        /// <summary>
        /// 自定义控件名称
        /// </summary>
        public string CustomizeFieldName { get; set; }

        public bool? IsReadOnly { get; set; }
    }
}