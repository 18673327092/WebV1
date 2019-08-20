using Web.Attribute;
using Base.IService;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utility.ResultModel;
using Utility;
using Utility.Components;
using Base.Model.Sys.Model;
using Base.Service.SystemSet;
using Web.Utility;

namespace Web.Controllers
{
    public class DictionaryController : Controller
    {
        IFieldService fieldService;
        IDictionaryService dictionaryService;
        public DictionaryController(IFieldService _fieldService, IDictionaryService _dictionaryService)
        {
            fieldService = _fieldService;
            dictionaryService = _dictionaryService;
        }

        public ActionResult Form(int id = 0)
        {
            ViewBag.FieldID = id;
            ViewBag.DictionaryList = JsonConvert.SerializeObject(SystemSetService.Dictionary.GetAllDictionaryList(id).Data);
            return View();
        }

        public ActionResult List(int menuid = 0)
        {
            return View();
        }

        public JsonResult _List(Pagination rq, string EntityName, string FieldTitle, string ValueList)
        {
            KendoFilterHelper.Single.SetPagination(Request.Form, rq);
            return new ListJsonResult { Data = dictionaryService.GetPagingList(new DictionaryModel() { EntityName = EntityName, FieldTitle = FieldTitle, ValueList = ValueList }, rq), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Save(DictionaryModel entity)
        {
            return Json(dictionaryService.Save(entity));
        }


      

    }
}
