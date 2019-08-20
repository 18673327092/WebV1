using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM;
using Base.IService;
using Utility;
using PetaPoco;
using Base.Model;
using CRM.Model.Data.Model;
using Model.Data.Model;
using Base.Model.Data.Model;
namespace Base.Service
{
    /// <summary>
    /// 系统基础数据提供类
    /// </summary>
    public class DataService : BaseService<BaseModel>, IDataService
    {
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        public List<ProvinceModel> GetProvinceList()
        {
            using (var db = CreateDao())
            {
                Sql sql = new Sql();
                sql.Select("ID,Name,Code").From("Data_Province");
                return db.Fetch<ProvinceModel>(sql);
            }
        }

        /// <summary>
        /// 根据省份ID获取城市列表
        /// </summary>
        /// <returns></returns>
        public List<CityModel> GetCityListByProvinceID(int ProvinceID)
        {
            using (var db = CreateDao())
            {
                Sql sql = new Sql();
                sql.Select("ID,Name,Code,ProvinceCode,ProvinceID").From("Data_City").Where("ProvinceID=@0", ProvinceID);
                return db.Fetch<CityModel>(sql);
            }
        }

        /// <summary>
        /// 根据城市ID获取区域列表
        /// </summary>
        /// <returns></returns>
        public List<AreaModel> GetAreaListByCityID(int CityID)
        {
            using (var db = CreateDao())
            {
                Sql sql = new Sql();
                sql.Select("ID,Name,Code,CityCode,CityID").From("Data_Area").Where("CityID=@0", CityID);
                return db.Fetch<AreaModel>(sql);
            }
        }

        /// <summary>
        /// 作废，不可用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ListResult<BaseModel> GetPagingList(BaseModel request, Pagination page)
        {
            return new ListResult<BaseModel>();
        }
    }
}
