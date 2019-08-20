using Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class FormModel
    {
        public int ID { get; set; }
        public Sys_Entity Entity { get; set; }
        public Sys_Form Form { get; set; }
        public List<Sys_Field> HideFields { get; set; }
        public List<Sys_Field> ShowFields { get; set; }
        public Dictionary<string, string> Data { get; set; }
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