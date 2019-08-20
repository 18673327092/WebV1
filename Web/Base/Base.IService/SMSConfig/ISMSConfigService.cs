using Base.Model;
using ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    public interface ISMSConfigService : IBaseService<Sys_SMSConfig>
    {
        Sys_SMSConfig GetSMSAccount();
    }
}
