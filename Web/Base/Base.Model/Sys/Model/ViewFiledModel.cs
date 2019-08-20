using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    /// <summary>
    /// 视图列
    /// </summary>
    [Serializable]
    [DataContract]
    public class ViewFieldModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "fieldtype")]
        public string FieldType { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; }

        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "format")]
        public string Format { get; set; }

        [DataMember(Name = "headertemplate")]
        public string HeaderTemplate { get; set; }

        [DataMember(Name = "headerattributes")]
        public string HeaderAttributes { get; set; }

        [DataMember(Name = "attributes")]
        public string Attributes { get; set; }

        [DataMember(Name = "template")]
        public string Template { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "sort")]
        public int Sort { get; set; }

        [DataMember(Name = "fieldsql")]
        public string FieldSql { get; set; }

        [DataMember(Name = "jointablename")]
        public string JoinTableName { get; set; }

        [DataMember(Name = "onsql")]
        public string OnSql { get; set; }

        [DataMember(Name = "filterable")]
        public Filterable Filterable { get; set; }
        [DataMember(Name = "openlink")]
        public string OpenLink { get; set; }

        [DataMember(Name = "templatesql")]
        public string TemplateSql { get; set; }

        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DataMember(Name = "RelationEntity")]
        public string RelationEntity { get; set; }

        [DataMember(Name = "type")]
        public string Columns_Type { get; set; }

        [DataMember(Name = "Valid")]
        /// <summary>
        /// 表单验证规则
        /// </summary>
        public string Valid { get; set; }


    }
    public class Filterable
    {
        public string ui { get; set; }
    }
}
