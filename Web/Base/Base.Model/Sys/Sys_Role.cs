using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model
{
    [TableName("Sys_Role")]
    [Serializable]
    [DataContract]
    [PrimaryKey("ID")]
    public class Sys_Role : BaseModel
    {
    }
}
