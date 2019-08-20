using Base.IService;
using Base.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Utility.ResultModel;
using Utility;
using Utility.Components;
using System.Drawing;
using System.Web.Mvc;
using Web.Attribute;
using Base.Model;
using Web.Utility;
using Base.Service.SystemSet;
using Base.Model.Sys.Model;
using Base.Model.Enum;
using System.Diagnostics;

namespace Web.Controllers
{

    public class BaseController : Controller
    {
        public BaseController() { }
        public string EntityName { get; set; }
        public IFieldService fieldService { get; set; }

        #region 公用方法
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new ListJsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding };
        }

        public new JsonResult Json(object data, JsonRequestBehavior jsonRequest)
        {
            return new ListJsonResult { Data = data, JsonRequestBehavior = jsonRequest };
        }

        public new JsonResult Json(object data)
        {
            return new ListJsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public string ToJson(object data)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };//这里使用自定义日期格式，默认是ISO8601格式        
            return JsonConvert.SerializeObject(data, Formatting.Indented, timeConverter);
        }
        #endregion

        #region 页面公用

        /// <summary>
        /// 实体ID
        /// </summary>
        public int EntityID
        {
            get
            {
                return SystemSetService.Entity.GetEntityID(EntityName);
            }
        }

        [PageAuth]
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult List(int menuid = 0, int v = 0)
        {
            ViewBag.AccessOperations = OperationConfigService.Instance.GetAccessOperations(CurrentLoginAdmin.Roles, menuid, "", CurrentLoginAdmin.ID).Data;
            var Entity = SystemSetService.Entity.GetEntityItem(EntityID).Data;

            ViewBag.EntityID = Entity.ID;
            ViewBag.EntityName = Entity.Name;
            ViewBag.AreaName = Entity.AreaName;
            ViewBag.Controller = Entity.ControllerName;
            ViewBag.EntityName = Entity.Name;
            ViewBag.Title = Entity.ShowName;
            ViewBag.DialogHeight = Entity.DialogHeight;
            ViewBag.DialogWidth = Entity.DialogWidth;
            ViewBag.IsDialog = Entity.IsDialog;

            var viewid = v;// !string.IsNullOrEmpty(Request.QueryString["v"]) ? Convert.ToInt32(Request.QueryString["v"]) : 0;
            if (viewid > 0)
            {
                var ViewModel = SystemSetService.View.GetViewItem(Convert.ToInt32(viewid)).Data;
                ViewBag.ViewModel = ViewModel;
                ViewBag.ViewType = ViewModel.Type;
                ViewBag.ViewTitle = ViewModel.Title;
            }
            else
            {
                var ViewModel = SystemSetService.View.GetViewByTypeEntityID(EntityID, ViewTypeEnum.系统视图.ToString());
                ViewBag.ViewModel = ViewModel;
                viewid = ViewModel.ID;
                ViewBag.ViewType = ViewModel.Type;
                ViewBag.ViewTitle = ViewModel.Title;
            }
            ViewBag.ViewID = viewid;
            ViewBag.GridFields = SystemSetService.View.GetGridFieldsByViewID(viewid);
            ViewBag.ViewList = SystemSetService.View.GetPagingViewList(new Sys_View() { EntityID = EntityID }, new Pagination() { }).Data.Where(e => e.ID != viewid).ToList();
            ViewBag.SearchField = SystemSetService.Search.GetSearchFieldsByView(viewid, EntityID);
            ViewBag.KeySearchFild = SystemSetService.Search.GetSearchKeyValue(EntityID);
            ViewBag.PATH = string.IsNullOrEmpty(Entity.AreaName) ? "/" + Entity.ControllerName + "/" : "/" + Entity.AreaName + "/" + Entity.ControllerName + "/";
            return View();
        }

        public JsonResult _List(Pagination rq, AdminCredential User)
        {
            KendoFilterHelper.Single.SetPagination(Request.Form, rq);
            return new ListJsonResult
            {
                Data = JsonConvert.DeserializeObject(SystemSetService.ListDataSource.GetListDataSource(rq, User)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public virtual ActionResult TabList(int pid, int peid, string pname)
        {
            ViewBag.PID = pid;
            ViewBag.PName = pname;
            ViewBag.PEID = peid;
            ViewBag.EntityID = EntityID;
            ViewBag.EntityName = EntityName;
            var viewid = !string.IsNullOrEmpty(Request.QueryString["v"]) ? Convert.ToInt32(Request.QueryString["v"]) : 0;
            var ViewModel = new Sys_View();
            if (viewid != 0)
            {
                ViewModel = SystemSetService.View.GetViewItem(viewid).Data;
            }
            else
            {
                ViewModel = SystemSetService.View.GetViewByTypeEntityID(EntityID, "关联视图");
            }
            ViewBag.ViewModel = ViewModel;
            viewid = ViewModel.ID;
            ViewBag.ViewType = ViewModel.Type;
            ViewBag.ViewTitle = ViewModel.Title;
            ViewBag.ViewID = viewid;
            var Entity = SystemSetService.Entity.GetEntityItem(EntityID).Data;
            ViewBag.EntityID = Entity.ID;
            ViewBag.AreaName = Entity.AreaName;
            ViewBag.Controller = Entity.ControllerName;
            ViewBag.GridFields = SystemSetService.View.GetGridFieldsByViewID(viewid);
            ViewBag.ViewList = SystemSetService.View.GetPagingViewList(new Sys_View() { EntityID = EntityID }, new Pagination() { }).Data.Where(e => e.ID != viewid).ToList();
            ViewBag.SearchField = SystemSetService.Search.GetSearchFieldsByView(viewid, EntityID);
            ViewBag.AccessOperations = OperationConfigService.Instance.GetAccessOperations(CurrentLoginAdmin.Roles, 0, Entity.ControllerName, CurrentLoginAdmin.ID).Data;
            return View();
        }

        /// <summary>
        /// 工作台视图列表
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ViewList()
        {
            var Entity = SystemSetService.Entity.GetEntityItem(EntityID).Data;
            ViewBag.EntityID = Entity.ID;
            ViewBag.EntityName = Entity.Name;
            ViewBag.AreaName = Entity.AreaName;
            ViewBag.Controller = Entity.ControllerName;
            ViewBag.EntityName = Entity.Name;
            ViewBag.Title = Entity.ShowName;
            var viewid = !string.IsNullOrEmpty(Request.QueryString["v"]) ? Convert.ToInt32(Request.QueryString["v"]) : 0;
            if (viewid > 0)
            {
                var ViewModel = SystemSetService.View.GetViewItem(Convert.ToInt32(viewid)).Data;
                ViewBag.ViewModel = ViewModel;
                ViewBag.ViewType = ViewModel.Type;
                ViewBag.ViewTitle = ViewModel.Title;
            }
            ViewBag.ViewID = viewid;
            ViewBag.GridFields = SystemSetService.View.GetGridFieldsByViewID(viewid);
            ViewBag.ViewList = SystemSetService.View.GetPagingViewList(new Sys_View() { EntityID = EntityID }, new Pagination() { }).Data.Where(e => e.ID != viewid).ToList();
            return View();
        }

        public void BaseForm(int id = 0, int formid = 0)
        {
            ViewBag.ID = id;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ViewBag.FormModel = SystemSetService.Form.GetForm(EntityID, formid);
            var Entity = SystemSetService.Entity.GetEntityItem(EntityID).Data;
            ViewBag.EntityModel = Entity;
            ViewBag.IsDialog = Entity.IsDialog;
            ViewBag.LayoutForm = "~/Views/Shared/_LayoutForm.cshtml";
            //if (Entity.IsDialog)
            //{
            //    ViewBag.LayoutForm = "~/Views/Shared/_LayoutDialogForm.cshtml";
            //}
            ViewBag.IsDialog = Entity.IsDialog;
            ViewBag.SaveAction = "/_Save";
            ViewBag.Title = Entity.ShowName;
            ViewBag.Name = "";
            ViewBag.Controller = Entity.ControllerName;
            ViewBag.PATH = string.IsNullOrEmpty(Entity.AreaName) ? "/" + Entity.ControllerName + "/" : "/" + Entity.AreaName + "/" + Entity.ControllerName + "/";
            #region 系统字段初始化值
            if (id > 0)
            {
                string CacheKey = EntityName + id;
                var PageData = CacheHelper.Single.TryGet(CacheKey, 0, () =>
                 {
                     Dictionary<string, string> objModel = new Dictionary<string, string>();
                     objModel = SystemSetService.Common.GetPageFormData(EntityName, id);
                     if (objModel.ContainsKey("OwnerID") && objModel["OwnerID"] != "")
                     {
                         objModel["OwnerIDName"] = SystemSetService.Common.GetFKField_Name("Sys_user", Convert.ToInt32(objModel["OwnerID"]));
                     }
                     if (objModel.ContainsKey("UpdateUserID") && objModel["UpdateUserID"] != "")
                     {
                         objModel["UpdateUserIDName"] = SystemSetService.Common.GetFKField_Name("Sys_user", Convert.ToInt32(objModel["UpdateUserID"]));
                         objModel["UpdateTimeDisplay"] = objModel["UpdateTime"];
                     }
                     objModel["UpdateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                     objModel["UpdateUserID"] = CurrentLoginAdmin.ID.ToString();
                     objModel["CreateTime"] = Convert.ToDateTime(objModel["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                     if (objModel.ContainsKey("CacheKey") && string.IsNullOrEmpty(objModel["CacheKey"]))
                     {
                         objModel["CacheKey"] = Guid.NewGuid().ToString();
                     }
                     if (objModel.ContainsKey("Name"))
                     {
                         ViewBag.Name = System.Web.HttpUtility.UrlEncode(objModel["Name"]);
                     }
                     if (Request.QueryString["action"] == "copy")
                     {
                         objModel["UpdateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                         objModel["UpdateUserID"] = CurrentLoginAdmin.ID.ToString();
                         objModel["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                         objModel["CreateUserID"] = CurrentLoginAdmin.ID.ToString();
                         objModel["OwnerID"] = CurrentLoginAdmin.ID.ToString();
                         objModel["DepartmentID"] = CurrentLoginAdmin.DepartmentID.ToString();
                         objModel["UpdateUserID"] = CurrentLoginAdmin.ID.ToString();
                         objModel["StateCode"] = "0";
                     }
                     return objModel;
                 });
                if (PageData.ContainsKey("Name"))
                {
                    ViewBag.Name = System.Web.HttpUtility.UrlEncode(PageData["Name"]);
                }
                ViewBag.PageData = PageData;
            }
            else
            {
                var objModel = new Dictionary<string, string>();
                objModel.Add("CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                objModel.Add("CreateUserID", CurrentLoginAdmin.ID.ToString());
                objModel.Add("UpdateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                objModel.Add("OwnerID", CurrentLoginAdmin.ID.ToString());
                objModel.Add("DepartmentID", CurrentLoginAdmin.DepartmentID.ToString());
                objModel.Add("UpdateUserID", CurrentLoginAdmin.ID.ToString());
                objModel.Add("StateCode", "0");
                objModel["CacheKey"] = Guid.NewGuid().ToString();
                ViewBag.PageData = objModel;
            }
            #endregion
            ViewBag.AccessOperations = OperationConfigService.Instance.GetAccessOperations(CurrentLoginAdmin.Roles, 0, RouteData.Values["controller"].ToString().ToLower(), CurrentLoginAdmin.ID).Data;
            sw.Stop();
            ApplicationContext.Log.Debug("BaseForm", sw.ElapsedMilliseconds.ToString());
        }

        [PageAuth]
        public void BaseDetail(int id = 0, int formid = 0)
        {
            ViewBag.ID = id;
            ViewBag.FormModel = SystemSetService.Form.GetForm(EntityID, formid);
            var Entity = SystemSetService.Entity.GetEntityItem(EntityID).Data;
            ViewBag.EntityModel = Entity;
            ViewBag.Title = Entity.ShowName;
            ViewBag.Name = "";
            ViewBag.LayoutForm = "~/Views/Shared/_LayoutDetail.cshtml";
            //if (Entity.IsDialog)
            //{
            //    ViewBag.LayoutForm = "~/Views/Shared/_LayoutDialogDetail.cshtml";
            //}
            if (id > 0)
            {
                string CacheKey = EntityName + id;
                ViewBag.PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    Dictionary<string, string> objModel = new Dictionary<string, string>();
                    objModel = SystemSetService.Common.GetPageFormData(EntityName, id);
                    if (objModel.ContainsKey("OwnerID") && objModel["OwnerID"] != "")
                    {
                        objModel["OwnerIDName"] = SystemSetService.Common.GetFKField_Name("Sys_user", Convert.ToInt32(objModel["OwnerID"]));
                    }
                    if (objModel.ContainsKey("UpdateUserID") && objModel["UpdateUserID"] != "")
                    {
                        objModel["UpdateUserIDName"] = SystemSetService.Common.GetFKField_Name("Sys_user", Convert.ToInt32(objModel["UpdateUserID"]));
                        objModel["UpdateTimeDisplay"] = objModel["UpdateTime"];
                    }
                    objModel["UpdateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    objModel["UpdateUserID"] = CurrentLoginAdmin.ID.ToString();
                    if (objModel.ContainsKey("Name") && objModel["Name"] != "")
                    {
                        ViewBag.Name = System.Web.HttpUtility.UrlEncode(objModel["Name"]);
                    }
                    objModel["CreateTime"] = Convert.ToDateTime(objModel["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    if (objModel.ContainsKey("CacheKey") && string.IsNullOrEmpty(objModel["CacheKey"]))
                    {
                        objModel["CacheKey"] = Guid.NewGuid().ToString();
                    }
                    return objModel;
                });
            }
            ViewBag.AccessOperations = OperationConfigService.Instance.GetAccessOperations(CurrentLoginAdmin.Roles, 0, RouteData.Values["controller"].ToString().ToLower(), CurrentLoginAdmin.ID).Data;
        }

        [PageAuth]
        public ActionResult DetailEdit(int id)
        {
            BaseForm(id);
            return View();
        }

        #endregion

        #region 其他方法
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="RequestTag">Request中的参数名</param>
        /// <param name="FilePath">上传文件夹下的子文件夹路径</param>
        /// <param name="deleteFilename">删除该文件夹下的某文件</param>
        /// <returns></returns>
        public string SaveFileFromClient(string RequestTag, string FilePath = "Temp", string deleteFilename = "")
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
                string uploadPath1 = baseUrl + @"Upload\" + FilePath + @"\" + "Smaill" + @"\";
                string fileUrl = "../../../Upload/" + FilePath + "/" + fileName;
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                if (!string.IsNullOrWhiteSpace(deleteFilename) && System.IO.File.Exists(uploadPath + deleteFilename))
                {
                    System.IO.File.Delete(uploadPath + deleteFilename);
                }
                fs = new FileStream(uploadPath + fileName, FileMode.Create, FileAccess.ReadWrite);

                while ((contentLen = uploadStream.Read(buffer, 0, bufferLen)) != 0)
                {
                    fs.Write(buffer, 0, bufferLen);
                    fs.Flush();
                }
                var image = Image.FromFile(uploadPath + fileName);
                //  var image2 = Image.FromFile(fileUrl);
                //  ApplicationContext.Image.CreateThumbnail()
                return fileUrl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UploadAttachment(string RequestTag, string SavePath = "Temp", string deleteFilename = "")
        {
            return ApplicationContext.Image.UploadAttachment(Request.Files[RequestTag], Server.MapPath("/"), SavePath, deleteFilename);

        }
        public string UploadImg(string RequestTag, string SavePath = "Temp", string deleteFilename = "")
        {
            return ApplicationContext.Image.UploadImg(Request.Files[RequestTag], Server.MapPath("/"), SavePath, deleteFilename);

            //try
            //{
            //    #region 原图
            //    HttpPostedFileBase upImage = Request.Files[RequestTag];
            //    var FileLength = upImage.ContentLength;
            //    string ExtendName = System.IO.Path.GetExtension(upImage.FileName).ToLower();
            //    byte[] myData = new Byte[FileLength];
            //    upImage.InputStream.Read(myData, 0, FileLength);
            //    string FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks;
            //    string baseUrl = Server.MapPath("/");
            //    string UploadPath = baseUrl + @"upload\" + SavePath + @"\";
            //    string FilePath = UploadPath + @"images\" + FileName + ExtendName;
            //    var SourceFilePath = UploadPath + @"images\";
            //    if (!Directory.Exists(SourceFilePath))
            //    {
            //        Directory.CreateDirectory(SourceFilePath);
            //    }
            //    FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            //    newFile.Write(myData, 0, myData.Length);
            //    newFile.Close();
            //    // var FileUrl = "../../../upload/" + SavePath + "/images/" + FileName + ExtendName;
            //    #endregion
            //    var FileUrl = "../../../upload/" + SavePath + @"/x" + ApplicationContext.AppSetting.DefaultThumbnail_Size + @"/" + FileName + ExtendName;
            //    string[] thumbnail_size = ApplicationContext.AppSetting.Thumbnail_Size.Split(new char[] { '|' });
            //    //原图加载
            //    using (Image sourceImage = Image.FromFile(FilePath))
            //    {
            //        int width = sourceImage.Width;
            //        int height = sourceImage.Height;
            //        #region 缩略图

            //        foreach (string size in thumbnail_size)
            //        {
            //            var ThumFilePath = UploadPath + @"x" + size + @"\";
            //            if (!Directory.Exists(ThumFilePath))
            //            {
            //                Directory.CreateDirectory(ThumFilePath);
            //            }
            //            int s_width = (int)Math.Floor(Convert.ToDouble(width) * Convert.ToDouble(size));
            //            int s_height = (int)Math.Floor(Convert.ToDouble(height) * Convert.ToDouble(size));
            //            //新建一个图板,等比例压缩大小绘制原图
            //            using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(s_width, s_height))
            //            {
            //                //绘制中间图
            //                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            //                {
            //                    //高清,平滑
            //                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //                    g.Clear(Color.Transparent);
            //                    g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, s_width, s_height), new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.GraphicsUnit.Pixel);
            //                    g.Dispose();
            //                    ThumFilePath = ThumFilePath + FileName + ExtendName;
            //                    bitmap.Save(ThumFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                }
            //            }
            //        }
            //        #endregion
            //        //if (width >= height)
            //        //{
            //        //    smallHeight = (int)Math.Floor(Convert.ToDouble(height) * (Convert.ToDouble(smallWidth) / Convert.ToDouble(width)));
            //        //}
            //        //else
            //        //{
            //        //    smallWidth = (int)Math.Floor(Convert.ToDouble(width) * (Convert.ToDouble(smallWidth) / Convert.ToDouble(height)));
            //        //}
            //    }


            //    //
            //    #region 删除旧图
            //    try
            //    {
            //        if (!string.IsNullOrWhiteSpace(deleteFilename))
            //        {
            //            var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
            //            var sourceimg = UploadPath + @"images\" + _delefilename;
            //            if (System.IO.File.Exists(sourceimg))
            //            {
            //                //删除原图
            //                System.IO.File.Delete(sourceimg);
            //            }

            //            foreach (string size in thumbnail_size)
            //            {
            //                var th = UploadPath + @"x" + size + @"\" + _delefilename;
            //                if (System.IO.File.Exists(th))
            //                {
            //                    System.IO.File.Delete(th);
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //    #endregion
            //    return FileUrl;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}

        }

        //public string UploadImage(string RequestTag, string SavePath = "Temp", string deleteFilename = "")
        //{
        //    return ApplicationContext.Image.UploadImage(Request.Files[RequestTag], Server.MapPath("/"), SavePath, deleteFilename);

        //}


        //private string CreateThumbnail(Image sourceImage, int smallWidth, int smallHeight, string FilePath)
        //{
        //    int width = sourceImage.Width;
        //    int height = sourceImage.Height;
        //    using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(smallWidth, smallHeight))
        //    {
        //        //绘制中间图
        //        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
        //        {
        //            //高清,平滑
        //            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //            g.Clear(Color.Transparent);
        //            g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, smallWidth, smallHeight), new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.GraphicsUnit.Pixel);
        //            g.Dispose();
        //            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        }
        //    }
        //}


        public JsonResult UpdateAttachmentById(string fieldname, string value, int id)
        {
            ApplicationContext.Cache.Remove(EntityName + id);
            return Json(SystemSetService.Common.UpdateEntityFieldById(EntityName, fieldname, value, id));
        }

        #endregion

        public JsonResult _ItemName(int id)
        {
            return Json(fieldService.GetName(EntityName, id));
        }

        public JsonResult _DictionaryName(int value, int fieldid)
        {
            return Json(fieldService.GetDictionaryName(value, fieldid));
        }

        /// <summary>
        /// 当前登录 平台管理员对象
        /// </summary>
        public AdminCredential CurrentLoginAdmin
        {
            get { return Session["AdminCredential"] as AdminCredential; }
        }

        /// <summary>
        /// 当前登录 平台管理员ID
        /// </summary>
        public int CurrentLoginAdminID
        {
            get { return CurrentLoginAdmin.ID; }
        }

    }


}
