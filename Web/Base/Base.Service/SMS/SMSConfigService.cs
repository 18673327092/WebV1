using Base.IService;
using Base.Model;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Service
{
    public class SMSConfigService : BaseService<Sys_SMSConfig>, ISMSConfigService
    {
        IFieldService fieldService;
        public SMSConfigService(IFieldService _fieldService)
        {
            fieldService = _fieldService;
        }
        public ListResult<Sys_SMSConfig> GetPagingList(Sys_SMSConfig request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        public Sys_SMSConfig GetSMSAccount()
        {
            Sql sql = new Sql();
            sql.Select("TOP 1 *").From("SMSConfig").Where("statecode=0");
            return base.GetList<Sys_SMSConfig>(sql).FirstOrDefault();
        }
    }
}
