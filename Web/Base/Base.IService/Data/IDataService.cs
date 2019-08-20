using Base.Model;
using Base.Model.Data.Model;
using CRM.Model.Data.Model;
using Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.IService
{
    /// <summary>
    /// 系统基础数据接口
    /// </summary>
    public interface IDataService : IBaseService<BaseModel>
    {
        List<ProvinceModel> GetProvinceList();
        List<CityModel> GetCityListByProvinceID(int ProvinceID);
        List<AreaModel> GetAreaListByCityID(int CityID);
    }
}
