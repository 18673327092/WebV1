using Base.IService;
using Base.Model;
using Base.Model.Sys.Model;
using System.Web.Mvc;
using Web.Attribute;
using Web.Utility;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Business/

        //
        // GET: /Menu/
        ICategoryService service;
        IFieldService fieldService;
        public CategoryController(ICategoryService _service, IFieldService _fieldService)
        {
            service = _service;
            fieldService = _fieldService;

        }


        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            return View();
        }


        #region Ajax
        [HttpPost]
        public JsonResult _Add(int pId, string name)
        {
            return Json(service.Insert(new Base_Category()
            {
                Name = name,
                ParentID = pId
            }));
        }

        public JsonResult _Delete(int id)
        {
            return Json(service.Delete(id));
        }

        [HttpPost]
        public JsonResult Move(int id, int newpId, int sibId, int dir)
        {
            return Json(service.Move(id, newpId, sibId, dir));
        }

        [HttpPost]
        public JsonResult _Update(int id, string name)
        {
            var entity = service.Get(id).Data;
            entity.Name = name;
            return Json(service.Update(entity));
        }

        public JsonResult _Item(int id)
        {
            return Json(service.Get(id));
        }

        public JsonResult _DropDownSource(AdminCredential User)
        {
            return new ListJsonResult
            {
                Data = service.GetTreeList(new Base_Category() { }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult _List(AdminCredential User)
        {
            return new ListJsonResult
            {
                Data = service.GetTreeList(new Base_Category() { }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        #endregion

    }
}
