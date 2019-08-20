using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class DialogController : Controller
    {
        /// <summary>
        /// 关联其他表
        /// </summary>
        /// <returns></returns>
        public ActionResult LookUp(LookUpParameter parameter)
        {
            LookUpModel model = new LookUpModel();
            model.Parameter = parameter;

            return View();
        }
    }
}