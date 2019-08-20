using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    /// <summary>
    /// 视图保存实体
    /// </summary>
    public class ViewSaveEntity
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "entityid")]
        public int EntityID { get; set; }

        [DataMember(Name = "viewid")]
        public int? ViewID { get; set; }

        [DataMember(Name = "entityname")]
        public string EntityName { get; set; }

        [DataMember(Name = "filtersql")]
        public string FilterSql { get; set; }

        [DataMember(Name = "relationentity")]
        public List<JoinEntity> Relationentity { get; set; }

        [DataMember(Name = "express")]
        public string Express { get; set; }

    }
}
