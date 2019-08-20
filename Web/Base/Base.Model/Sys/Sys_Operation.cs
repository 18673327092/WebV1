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
    /// 操作按钮实体
    /// </summary>
    [TableName("Sys_Operation")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Operation
    {

        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public int ID { get; set; }


        /// <summary>
        /// 权限名称
        /// </summary>
        [DataMember]
        public string OperateName { get; set; }


        /// <summary>
        /// ParentID
        /// </summary>
        [DataMember]
        public int ParentID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [ResultColumn]
        public string MenuName { get; set; }


        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public int? MenuID { get; set; }


        /// <summary>
        /// 权限标示
        /// </summary>
        [DataMember]
        public string Mark { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }


        /// <summary>
        ///按钮ID
        /// </summary>
        [DataMember]
        public string BtnID { get; set; }


        /// <summary>
        /// 0表示基础按钮，每一个菜单都共有的，1表示自定义按钮
        /// </summary>
        [DataMember]
        public int Type { get; set; }


        ///创建时间
        [DataMember]
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public string Icon { get; set; }

        [DataMember]
        public int StateCode { get; set; }


        /// <summary>
        /// 图标样式
        /// </summary>
        [DataMember]
        public string Style { get; set; }

        /// <summary>
        /// 脚本方法
        /// </summary>
        [DataMember]
        public string Fun { get; set; }

        bool _Ischeck = false;
        /// <summary>
        /// 是否必须选中后才显示按钮
        /// </summary>
        [DataMember]
        public bool Ischeck { get { return _Ischeck; } set { _Ischeck = value; } }

        bool _IsListHeadShow = false;
        /// <summary>
        /// 是否在列表头部显示
        /// </summary>
        [DataMember]
        public bool IsListHeadShow { get { return _IsListHeadShow; } set { _IsListHeadShow = value; } }

        bool _IsListShow = false;
        /// <summary>
        /// 是否在列表中显示
        /// </summary>
        [DataMember]
        public bool IsListShow { get { return _IsListShow; } set { _IsListShow = value; } }

        bool _Isonlyform = false;
        /// <summary>
        /// 是否表单中显示
        /// </summary>
        [DataMember]
        public bool IsFormShow { get { return _Isonlyform; } set { _Isonlyform = value; } }

        bool _IsTabListShow = false;
        /// <summary>
        /// 在Tab列表显示
        /// </summary>
        [DataMember]
        public bool IsTabListShow { get { return _IsTabListShow; } set { _IsTabListShow = value; } }

    }
}
