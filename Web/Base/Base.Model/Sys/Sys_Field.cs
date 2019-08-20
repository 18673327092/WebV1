using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_Field")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    /// <summary>
    /// 属性（字段）类
    /// </summary>
    public class Sys_Field
    {
        #region 基本字段

        [DataMember]
        public int ID { get; set; }

        [DataMember(Name = "Name")]
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        [DataMember(Name = "fieldType")]
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        [DataMember(Name = "title")]
        /// <summary>
        /// 字段中文标题
        /// </summary>
        public string Title { get; set; }
        public int StateCode { get; set; }
        public string Remark { get; set; }

        public string Type { get; set; }
        [DataMember]
        public bool? IsSystem { get; set; }

        /// <summary>
        /// 选项集可自定义
        /// </summary>
        [DataMember]
        public bool? IsCustomizeDictionary { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [DataMember]
        public string DefaultValue { get; set; }


        #endregion

        #region 控制属性

        bool? _IsAllowSearch = false;
        /// <summary>
        ///允许查询
        /// </summary>
        [DataMember]
        public bool? IsAllowSearch { get { return _IsAllowSearch; } set { _IsAllowSearch = value; } }

        /// <summary>
        ///查询控件摆放顺序
        /// </summary>
        [DataMember]
        public int SearchControlSort { get; set; }

        /// <summary>
        ///查询控件宽度
        /// </summary>
        [DataMember]
        public int SearchControlWidth { get; set; }


        bool? _IsKeySearch = false;
        /// <summary>
        ///关键字查询
        /// </summary>
        [DataMember]
        public bool? IsKeySearch { get { return _IsKeySearch; } set { _IsKeySearch = value; } }

        bool? _IsCustomizeSearchControl = false;
        /// <summary>
        ///自定义查询控件
        /// </summary>
        [DataMember]
        public bool? IsCustomizeSearchControl { get { return _IsCustomizeSearchControl; } set { _IsCustomizeSearchControl = value; } }
        /// <summary>
        /// 查看控件是否属于该视图
        /// </summary>
        [ResultColumn]
        public int SearchControIsForView { get; set; }

        bool? _IsAllowDialogSearch = false;

        /// <summary>
        ///允许弹框查询
        /// </summary>
        [DataMember]
        public bool? IsAllowDialogSearch { get { return _IsAllowDialogSearch; } set { _IsAllowDialogSearch = value; } }

        bool? _IsFormShow = false;

        /// <summary>
        ///表单显示
        /// </summary>
        [DataMember]
        public bool? IsFormShow { get { return _IsFormShow; } set { _IsFormShow = value; } }

        bool? _IsColumnShow = false;

        /// <summary>
        ///列表显示
        /// </summary>
        [DataMember]
        public bool? IsColumnShow { get { return _IsColumnShow; } set { _IsColumnShow = value; } }

        bool? _IsImportField = false;
        /// <summary>
        ///是否允许导入
        /// </summary>
        [DataMember]
        public bool? IsImportField { get { return _IsImportField; } set { _IsImportField = value; } }

        [DataMember]
        /// <summary>
        /// 类型
        /// </summary>
        public string LabelType { get; set; }


        /// <summary>
        /// 跳转链接地址
        /// </summary>
        [DataMember]
        public string OpenLink { get; set; }

        /// <summary>
        /// 筛选条件所在的视图
        /// </summary>
        [DataMember]
        public string SearchForView { get; set; }


        #endregion

        #region Grid属性列

        /// <summary>
        /// 字段名
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "type")]
        public string Columns_Type { get; set; }

        [DataMember(Name = "format")]
        public string Columns_Format { get; set; }
        [DataMember]
        /// <summary>
        /// 列头模版（复选框、单选框）
        /// </summary>
        public string HeaderTemplate { get; set; }

        [DataMember]
        /// <summary>
        /// 列头属性
        /// </summary>
        public string HeaderAttributes { get; set; }

        [DataMember]
        /// <summary>
        /// 列属性
        /// </summary>
        public string Attributes { get; set; }

        [DataMember]
        /// <summary>
        /// 列自定义模版
        /// </summary>
        public string Template { get; set; }

        [DataMember]
        /// <summary>
        /// 列自定义模版-Sql
        /// </summary>
        public string TemplateSql { get; set; }

        [DataMember(Name = "width")]
        /// <summary>
        /// 列宽
        /// </summary>
        public int Width { get; set; }

        [DataMember]
        /// <summary>
        /// 列的排放顺序
        /// </summary>
        public int ColumnSort { get; set; }

        [DataMember]
        public string filterable { get; set; }

        bool? _EnableTips = false;
        /// <summary>
        ///是否启用鼠标悬浮显示全部
        /// </summary>
        [DataMember]
        public bool? EnableTips { get { return _EnableTips; } set { _EnableTips = value; } }

        #endregion

        #region 表单排版
        [DataMember]
        /// <summary>
        /// X 轴
        /// </summary>
        public int X { get; set; }

        [DataMember]
        /// <summary>
        /// Y 轴
        /// </summary>
        public int Y { get; set; }

        [DataMember]
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        [DataMember]
        public int Colspan { get; set; }
        #endregion

        #region 表单属性
        public string Reg { get; set; }
        /// <summary>
        /// 字段约束
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 最大字符数
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        public bool? IsMultiple { get; set; }

        bool? _IsMust = false;
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool? IsMust { get { return _IsMust; } set { _IsMust = value; } }

        bool? _IsCustomizeControl = false;
        /// <summary>
        /// 自定义控件
        /// </summary>
        public bool? IsCustomizeControl { get { return _IsCustomizeControl; } set { _IsCustomizeControl = value; } }

        bool? _IsHide = false;
        /// <summary>
        /// 是否隐藏列
        /// </summary>
        public bool? IsHide { get { return _IsHide; } set { _IsHide = value; } }

        /// <summary>
        /// 表单验证规则
        /// </summary>
        public string Valid { get; set; }

        /// <summary>
        /// 是否在弹框中显示出来了
        /// </summary>
        bool? _IsDialogField = false;
        public bool? IsDialogField { get { return _IsDialogField; } set { _IsDialogField = value; } }

        /// <summary>
        /// 是否只读
        /// </summary>
        bool? _IsReadOnly = false;
        public bool? IsReadOnly { get { return _IsReadOnly; } set { _IsReadOnly = value; } }

        [DataMember]
        /// <summary>
        /// 是否是Tab列
        /// </summary>
        bool? _IsTabField = false;
        public bool? IsTabField { get { return _IsTabField; } set { _IsTabField = value; } }

        /// <summary>
        /// 字段别名
        /// </summary>
        [DataMember]
        public string AnotherName { get; set; }

        /// <summary>
        /// 字段提示语
        /// </summary>
        [DataMember]
        public string Placeholder { get; set; }


        bool? _IsDropDownSource = false;
        /// <summary>
        /// 关联其他的表 数据源转为下拉框形式展示
        /// </summary>
        public bool? IsDropDownSource { get { return _IsDropDownSource; } set { _IsDropDownSource = value; } }

        #endregion

        #region 关联属性
        [DataMember]
        /// <summary>
        /// 所属实体表ID
        /// </summary>
        public int? EntityID { get; set; }

        [DataMember]
        /// <summary>
        /// 所属实体表
        /// </summary>
        public string EntityName { get; set; }

        [DataMember]
        /// <summary>
        /// 关联外键表
        /// </summary>
        public string RelationEntity { get; set; }

        [DataMember]
        /// <summary>
        /// 关联外键表的某个字段（已作废）
        /// </summary>
        public string RelationField { get; set; }

        [DataMember]
        /// <summary>
        /// 所属实体表
        /// </summary>
        public string FieldSql { get; set; }

        [Ignore]
        public string DictionaryList { get; set; }

        public string JoinTableName { get; set; }
        public string OnSql { get; set; }
        #endregion

        int _PointNumber = 2;
        /// <summary>
        /// 小数点格式
        /// </summary>
        public int PointNumber { get { return _PointNumber; } set { _PointNumber = value; } }

        public bool _IsEnableSumData = false;
        /// <summary>
        /// 统计数据
        /// </summary>
        [DataMember]
        public bool IsEnableSumData { get { return _IsEnableSumData; } set { _IsEnableSumData = value; } }

        #region 图片属性
        /// <summary>
        /// 图片宽度
        /// </summary>
        public double ImageWidth { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public double ImageHeight { get; set; }

        /// <summary>
        ///是否保存原图
        /// </summary>
        public bool? IsSaveOriginalGraph { get; set; }

        /// <summary>
        /// 是否创建缩略图
        /// </summary>
        public bool? IsCreateThumbnail { get; set; }

        #endregion
    }

    public class attributes
    {
        public string style { get; set; }
    }

    public class headerAttributes
    {
        public string style { get; set; }
    }

    public class EntityFilterable
    {
        public string ui { get; set; }
    }


}
