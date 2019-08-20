using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_Job_User")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Job_User
    {
        public int ID { get; set; }
        public int JobID { get; set; }
        public int UserID { get; set; }

        [ResultColumn]
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string JobName { get; set; }
    }
}
