using Base.IService;
using Base.Model;
using Base.Model.Sys.Model;
using Base.Service.SystemSet;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using Web.Attribute;
using Web.Models;

namespace Web.Controllers
{
    [AdminLogin]
    public class CommonController : Controller
    {
        //
        // GET: /User/
        //
        // GET: /Customer/
        IUserService userService;
        IFieldService fieldService;
        IReportService reportservice;
        public CommonController(IUserService _userService, IFieldService _fieldService, IReportService _reportservice)
        {
            userService = _userService;
            fieldService = _fieldService;
            reportservice = _reportservice;
        }
        //
        // GET: /Common/

        public ActionResult AttachmentUpload()
        {
            return View();
        }

        #region 数据共享
        public ActionResult Share(int v, int id = 0)
        {
            if (id != 0)
            {
                ViewBag.ShareUser = userService.GetShareUser(v, id).Data;
            }
            return View();
        }

        [HttpPost]
        public JsonResult _GetUserList(Pagination request)
        {
            return Json(userService.GetPagingList(new Sys_User() { }, request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult _SaveID(string ids)
        {
            try
            {
                List<string> list = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                Session["ids"] = list;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult _SaveShare(int v, string userids)
        {
            try
            {
                List<string> userlist = userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                List<string> ids = Session["ids"] as List<string>;
                return Json(userService.SaveShare(v, userlist, ids), JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return Json(new ItemResult<bool> { Success = false, Message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 数据分派

        public ActionResult Assign(int v)
        {
            return View();
        }

        [HttpPost]
        public JsonResult _SaveAssign(int v, int uid)
        {
            try
            {
                List<string> ids = Session["ids"] as List<string>;
                return Json(userService.SaveAssign(v, uid, ids), JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return Json(new ItemResult<bool> { Success = false, Message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImportFile()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "请求数据错误" }, "text/html");
                }
                string file = SaveFileFromClient("ImportFile", "上传文件");
                return Json(new { success = true, data = file }, "text/html");
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "系统错误" });
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="RequestTag">Request中的参数名</param>
        /// <param name="FilePath">上传文件夹下的子文件夹路径</param>
        /// <param name="deleteFilename">删除该文件夹下的某文件</param>
        /// <returns></returns>
        private string SaveFileFromClient(string RequestTag, string FilePath = "Temp", string deleteFilename = "")
        {
            Stream uploadStream = null;
            FileStream fs = null;
            try
            {
                //文件上传，一次上传1M的数据，防止出现大文件无法上传  
                HttpPostedFileBase postFileBase = Request.Files[RequestTag];
                uploadStream = postFileBase.InputStream;
                int bufferLen = 1024;
                byte[] buffer = new byte[bufferLen];
                int contentLen = 0;

                string fileName = Path.GetFileName(postFileBase.FileName);
                string fileExt = fileName.Substring(fileName.LastIndexOf("."));
                fileName = Guid.NewGuid() + fileExt;
                string baseUrl = Server.MapPath("/");
                string uploadPath = baseUrl + @"Upload\" + FilePath + @"\";
                string fileUrl = "../../../Upload/" + FilePath + "/" + fileName;
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                if (!string.IsNullOrWhiteSpace(deleteFilename) && System.IO.File.Exists(uploadPath + deleteFilename))
                {
                    System.IO.File.Delete(uploadPath + deleteFilename);
                }
                using (fs = new FileStream(uploadPath + fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    while ((contentLen = uploadStream.Read(buffer, 0, bufferLen)) != 0)
                    {
                        fs.Write(buffer, 0, bufferLen);
                        fs.Flush();
                    }
                }
                return fileUrl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region 数据导出
        public FileResult ExportList(AdminCredential User, int eid, int v)
        {
            var view = SystemSetService.View.GetViewItem(v).Data;
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = workbook.CreateSheet(view.Title);
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.Height = 430;

            ICellStyle style = workbook.CreateCellStyle();
            // style.FillForegroundColor = (short)24;// NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            // style.FillPattern = FillPattern.SolidForeground;
            //style.BorderTop = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            //style.BorderBottom = BorderStyle.Thin;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            // IFont font = workbook.CreateFont(); //创建一个字体样式对象
            // font.FontHeightInPoints = 10;//字体大小
            // font.FontName = "宋体"; //和excel里面的字体对应
            // font.Boldweight = short.MaxValue;//字体加粗
            // font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            //  style.SetFont(font); //将字体样式赋给样式对象

            var FieldList = JsonConvert.DeserializeObject<List<ViewFieldModel>>(view.FieldList);
            int i = 0;
            foreach (var item in FieldList)
            {
                ICell cell = row1.CreateCell(i);
                cell.SetCellValue(item.Title);
                cell.CellStyle = style;
                i++;
            }
            style.Alignment = HorizontalAlignment.Left;
            DataTable dt = SystemSetService.View.GetViewExportData(view.Sql, new Pagination() { Page = 1, PageSize = 999999, vid = v, eid = eid }, User);
            //将数据逐步写入sheet1各个行
            for (int z = 0; z < dt.Rows.Count; z++)
            {
                var row = dt.Rows[z];

                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(z + 1);
                int c = 0;
                foreach (var item in FieldList)
                {
                    var _value = row[item.Field].ToString();
                    //if (ConvertHelper.IsDateTime(_value))
                    //{
                    //    if (ConvertHelper.ToDouble(_value, 0) == 0)
                    //    {
                    //        _value = ConvertHelper.ToDateTime(_value).ToString("yyyy-MM-dd HH:mm:ss");
                    //    }
                    //}
                    var cell = rowtemp.CreateCell(c++);
                    cell.CellStyle = style;
                    cell.SetCellValue(_value);
                }
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", view.Title + ".xls");
        }
        #endregion

        [HttpPost]
        public JsonResult _UpdateValue(UpdateFiledValue input)
        {
            var res = new ItemResult<int> { Success = true, Message = "" };
            var entityname = input.field.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var field = input.field.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries)[1];
            try
            {
                res.Data = CommonService.Single.UpdateValue(entityname, field, input.value, input.id);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
