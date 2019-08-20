using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Base.Model
{
    [Serializable]
    public class CategoryTree
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int pId { get; set; }

        [DataMember]
        public string name { get; set; }

        private bool _open = true;
        [DataMember]
        public bool open { get { return _open; } set { _open = value; } }
        //public string file { get; set; }
    }
}
