
using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model.Sys.Model;
using Base.Model;

namespace Base.IService
{
    public interface IDictionaryService : IBaseService<DictionaryModel>
    {
        ItemResult<bool> Save(DictionaryModel entity);
        Task<ListResult<Sys_Dictionary>> GetAllListAsync(int fieldId);
    }
}
