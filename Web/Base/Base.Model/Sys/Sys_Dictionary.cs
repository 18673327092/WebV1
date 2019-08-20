using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    /// <summary>
    /// 数据库字典实体
    /// </summary>
    [TableName("Sys_Dictionary")]
    [PrimaryKey("ID")]
    public class Sys_Dictionary
    {
        public int ID { get; set; }
        public int Sort { get; set; }
        public string Name { get; set; }

        public int Value { get; set; }
        public int StateCode { get; set; }
        public int IsSystem { get; set; }

        

        public int? FieldID { get; set; }
    }
}
