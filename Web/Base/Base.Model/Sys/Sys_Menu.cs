using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    [TableName("Sys_Menu")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    /// <summary>
    /// 菜单实体
    /// </summary>
    public class Sys_Menu
    {
        [DataMember]
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        [DataMember]
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        [DataMember]
        [ResultColumn]
        /// <summary>
        /// 菜单父级
        /// </summary>
        public string ParenName { get; set; }

        [DataMember]
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string MenuUrl { get; set; }

        [DataMember]
        /// <summary>
        /// 父级id
        /// </summary>
        public int ParentID { get; set; }

        [DataMember]
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }

        [DataMember]
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        bool _IsHide = false;
        [DataMember]
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHide { get { return _IsHide; } set { _IsHide = value; } }


        bool _IsNoMenu = false;

        /// <summary>
        /// 不作为菜单，但可以授权
        /// </summary>
        [DataMember]
        public bool IsNoMenu { get { return _IsNoMenu; } set { _IsNoMenu = value; } }

        bool _IsDug = false;
        [DataMember]
        /// <summary>
        /// 是否属性系统内置开发菜单
        /// </summary>
        public bool IsDug { get { return _IsDug; } set { _IsDug = value; } }

        [DataMember]
        public string Icon { get; set; }

        [ResultColumn]
        public int RoleID { get; set; }

        [Ignore]
        public List<Sys_Menu> ChildMenuList { get; set; }

        [DataMember]
        public string AccessOperation { get; set; }

        [DataMember]
        public int ViewID { get; set; }

        [DataMember]
        public string MenuArea { get; set; }

        [DataMember]
        public string MenuAreaName { get; set; }

        public int SiteID { get; set; }

    }
}
