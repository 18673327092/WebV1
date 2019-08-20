using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    /// <summary>
    /// 岗位实体
    /// </summary>
    [TableName("Sys_Job")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Job : BaseModel
    {
    
    }
}
