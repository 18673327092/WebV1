using Base.IService;
using Base.Model;
using Base.Service.SystemSet;
using System.Collections.Generic;
using System.Web.Mvc;
using Utility;
using Web.Attribute;

namespace Web.Controllers
{

    /// <summary>
    /// 系统基础数据控制器
    /// </summary>
    [PageAuth]
    public class DataController : Controller
    {
        IDataService service;
        IFieldService fieldService;
        public DataController(IDataService _service, IFieldService _fieldService)
        {
            service = _service;
            fieldService = _fieldService;
        }

        #region 省、市、区
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProvinceList()
        {
            return Json(service.GetProvinceList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  根据省份ID获取城市列表
        /// </summary>
        /// <param name="id">省份ID</param>
        /// <returns></returns>
        public JsonResult GetCityListByProvinceID(int id)
        {
            return Json(service.GetCityListByProvinceID(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市ID获取区域列表
        /// </summary>
        /// <param name="id">城市ID</param>
        /// <returns></returns>
        public JsonResult GetAreaListByCityID(int id)
        {
            return Json(service.GetAreaListByCityID(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 选项集
        public JsonResult GetDictionaryListByFieldID(int FieldID)
        {
            List<Sys_Dictionary> list = SystemSetService.Dictionary.GetList(new Sys_Dictionary() { FieldID = FieldID });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
