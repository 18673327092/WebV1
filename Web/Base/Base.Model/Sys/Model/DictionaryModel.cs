using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Model.Sys.Model
{
    public class DictionaryModel
    {
        public int FieldID { get; set; }
        public string FieldTitle { get; set; }
        public string EntityName { get; set; }
        public string ValueList { get; set; }
    }
}
