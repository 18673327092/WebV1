using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    public class JoinEntity
    {
        [DataMember(Name = "jointablename")]
        public string JoinTableName { get; set; }

        [DataMember(Name = "onsql")]
        public string OnSql { get; set; }
    }
}
