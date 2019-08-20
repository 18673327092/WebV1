using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Model.Sys.Model
{
    public class SearchField
    {
        public int ID { get; set; }
        public string Field { get; set; }
        public string FieldType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string RelationEntity { get; set; }
        public int Sort { get; set; }
        public int SearchControlWidth { get; set; }

        public bool? IsMultiple { get; set; }
        public bool? IsCustomizeSearchControl { get; set; }
        public bool SearchControIsForView { get; set; }
        
        public List<Sys_Dictionary> DictionaryList { get; set; }

        /// <summary>
        /// 控件Class
        /// </summary>
        public string ClassName { get; set; }
    }
}
