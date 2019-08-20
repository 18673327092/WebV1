using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model
{
    /// <summary>
    /// 实体
    /// </summary>
    [TableName("Sys_Entity")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Entity
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShowName { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public string ControllerName { get; set; }

        public bool _IsSystem = false;
        [DataMember]
        public bool IsSystem { get { return _IsSystem; } set { _IsSystem = value; } }

        public bool _IsHide = false;
        [DataMember]
        public bool IsHide { get { return _IsHide; } set { _IsHide = value; } }

        public bool _IsDialog = false;
        /// <summary>
        /// 弹框形式展示表单
        /// </summary>
        [DataMember]
        public bool IsDialog { get { return _IsDialog; } set { _IsDialog = value; } }

        public bool _IsEnableSumData = false;
        /// <summary>
        /// 统计数据
        /// </summary>
        [DataMember]
        public bool IsEnableSumData { get { return _IsEnableSumData; } set { _IsEnableSumData = value; } }

        public bool _IsEnableFormPage = false;
        /// <summary>
        /// 表单分页
        /// </summary>
        [DataMember]
        public bool IsEnableFormPage { get { return _IsEnableFormPage; } set { _IsEnableFormPage = value; } }

        #region 弹框属性

        /// <summary>
        /// 弹框宽
        /// </summary>
        public string _DialogWidth = "85%";
        [DataMember]
        public string DialogWidth { get { return _DialogWidth; } set { _DialogWidth = value; } }

        /// <summary>
        /// 弹框高
        /// </summary>
        public string _DialogHeight = "90%";
        [DataMember]
        public string DialogHeight { get { return _DialogHeight; } set { _DialogHeight = value; } }


        #endregion

        #region 表单属性
        private int _OneRowFieldsNumber = 3;
        public int OneRowFieldsNumber { get { return _OneRowFieldsNumber; } set { _OneRowFieldsNumber = value; } }


        #endregion

        /// <summary>
        /// 启用数据授权
        /// </summary>
        [DataMember]
        public bool IsEnableDataAuthorize { get; set; }

        /// <summary>
        /// 启用页面授权
        /// </summary>
        [DataMember]
        public bool IsEnablePageAuthorize { get; set; }


    }
}
