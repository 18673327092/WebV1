using Base.Model;
using Base.Model.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ListModel
    {
        /// <summary>
        /// 视图对象
        /// </summary>
        public Sys_View View { get; set; }
        /// <summary>
        /// 实体对象
        /// </summary>
        public Sys_Entity Entity { get; set; }
        /// <summary>
        /// 列表查询控件
        /// </summary>
        public List<SearchField> SearchFieldList { get; set; }
        /// <summary>
        /// 列表显示字段
        /// </summary>
        public string ListFieldsJSON { get; set; }
        /// <summary>
        /// 操作按钮
        /// </summary>
        public List<Sys_Operation> AccessOperations { get; set; }
        public string Path
        {
            get
            {
                return string.IsNullOrEmpty(Entity.AreaName) ? "/" + Entity.ControllerName + "/" : "/" + Entity.AreaName + "/" + Entity.ControllerName + "/";
            }
        }

    }
}