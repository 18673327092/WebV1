using Base.IService;
using Base.Model;
using Base.Model.Base.Model;
using Base.Model.Sys.Model;
using Base.Service.SystemSet;
using Newtonsoft.Json;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Utility;
using Utility.ResultModel;
using Web.Attribute;

namespace Web.Controllers
{
    public class ImagesController : AdminBaseController
    {
        public IImagesService service { get; set; }
        public ImagesController()
        {
            EntityName = "Base_Images";
        }

        #region 通用方法（除_Save方法的实体参数外，其他方法不用改）

        #region 表单页面
        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            base.BaseForm(id);
            Base_Images entity = new Base_Images();
            if (id > 0)
            {
                entity = service.Get(id).Data;
            }
            return View(entity);
        }
        #endregion

        #region 详细页
        [PageAuth]
        public ActionResult Detail(AdminCredential user, int id)
        {
            base.BaseDetail(id);
            Base_Images entity = new Base_Images();
            if (id > 0)
            {
                entity = service.Get(id).Data;
            }
            return View(entity);
        }
        #endregion

        #region 表单提交
        [HttpPost]
        public JsonResult _Save(Base_Images entity, AdminCredential User)
        {
            string ThumbnailSrc = string.Empty;
            ItemResult<int> result = new ItemResult<int>();
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserID = User.ID;
            entity.CreateTime = DateTime.Now;
            entity.Name = entity.Src;
            if (!string.IsNullOrEmpty(entity.UserEntityName))
            {
                var sys_entity = SystemSetService.Entity.GetEntityItem(entity.UserEntityName);
                var fieltTitle = SystemSetService.Field.GetTitle(sys_entity.ID, entity.UserFieldName);
                entity.Name = string.Format("{0}-{1}", sys_entity.ShowName, fieltTitle);
                entity.UserAreaName = sys_entity.AreaName;
                if (!entity.IsSaveOriginalGraph && !string.IsNullOrEmpty(entity.OriginalSrc))
                {
                    DeleteImages(entity.OriginalSrc);
                    entity.OriginalSrc = string.Empty;
                }
                if (entity.IsCreateThumbnail)
                {
                    entity.ThumbnailSrc = CreateThumbnail(entity.Src);
                }
                else
                {
                    entity.ThumbnailSrc = entity.Src;
                }
            }
            if (entity.ID == 0)
            {
                result = service.Insert(entity);
            }
            else
            {
                ApplicationContext.Cache.Remove(EntityName + entity.ID);
                entity.StateCode = 0;
                result = service.Update(entity);
                result.Data = entity.ID;
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 数据删除
        [HttpPost]
        public JsonResult _Delete(string ids)
        {
            JsonConvert.DeserializeObject<List<int>>(ids).ForEach(id =>
            {
                var entity = service.Get(id);
                DeleteImages(entity.Data.Src);
                DeleteImages(entity.Data.ThumbnailSrc);
                DeleteImages(entity.Data.OriginalSrc);
            });
            return Json(service.Delete(JsonConvert.DeserializeObject<List<int>>(ids)));
        }
        #endregion

        #region 数据停用

        [HttpPost]
        public JsonResult _Disable(string ids, int statecode = 1)
        {
            return Json(service.Disable(EntityName, JsonConvert.DeserializeObject<List<int>>(ids), statecode), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult _IsUnDisable(int id)
        {
            return Json(service.IsUnDisable(EntityName, id), JsonRequestBehavior.DenyGet);
        }
        #endregion

        public JsonResult _Item(int id)
        {
            return Json(service.Get(id));
        }
        #endregion

        public ActionResult Upload(string type, int fieldid, string fieldname, string entityname, int dataid = 0, int value = 0)
        {
            ViewBag.UserFieldName = fieldname;
            ViewBag.UserEntityName = entityname;
            ViewBag.UserDataID = dataid;
            ViewBag.Type = type;
            Sys_Field FieldEntity = fieldService.Get(fieldid).Data;
            ViewBag.ImageHeight = FieldEntity.ImageHeight;
            ViewBag.ImageWidth = FieldEntity.ImageWidth;
            ViewBag.IsCreateThumbnail = FieldEntity.IsCreateThumbnail.HasValue ? FieldEntity.IsCreateThumbnail.Value : false;
            ViewBag.IsSaveOriginalGraph = FieldEntity.IsSaveOriginalGraph.HasValue ? FieldEntity.IsSaveOriginalGraph.Value : false;
            ViewBag.Base_Images = new Base_Images()
            {

            };
            if (value > 0)
            {
                ViewBag.Base_Images = service.Get(value).Data;
            }
            return View();
        }

        public string AjaxUploadImages(int UserID = 0)
        {
            try
            {
                var file = Request.Files[0];
                var uploadresult = UploadImages(Request.Files[0], "images");
                if (uploadresult.Success)
                {
                    Base_Images entity = uploadresult.Data;
                    entity.OwnerID = UserID;
                    entity.CreateTime = DateTime.Now;
                    entity.UpdateTime = DateTime.Now;
                    entity.StateCode = 1;
                    entity.UpdateUserID = UserID;
                    var result = service.Insert(entity);
                    entity.ID = result.Data;
                }
                return JsonConvert.SerializeObject(uploadresult);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ItemResult<Base_Images> UploadImages(HttpPostedFileBase upFile, string SavePath = "images", string deleteFilename = "")
        {
            ItemResult<Base_Images> result = new ItemResult<Base_Images>();
            Base_Images Images = new Base_Images();
            try
            {
                var FileLength = upFile.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();

                if (ApplicationContext.AppSetting.AllowFileExt.IndexOf(ExtendName.ToLower()) == -1)
                {
                    result.Message = "上传文件暂不支持“" + ExtendName + "”格式";
                    return result;
                }

                byte[] myData = new Byte[FileLength];
                upFile.InputStream.Read(myData, 0, FileLength);
                string UploadPath = Server.MapPath(ApplicationContext.AppSetting.Image_Upload_Path);

                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }
                string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString() + ".png";
                string FilePath = UploadPath + NewFileName;
                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
                Images.Name = NewFileName;
                Images.OriginalSrc = ApplicationContext.AppSetting.Image_Upload_Path + NewFileName;
                var kb = myData.Length * 1.0 / 1024;
                if (kb > 1024)
                {
                    Images.Size = (kb / 1024).ToString("0.00") + "M";
                }
                else
                {
                    Images.Size = kb.ToString("0.00") + "KB";
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(deleteFilename))
                    {
                        var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
                        var Imagesurl = UploadPath + _delefilename;
                        if (System.IO.File.Exists(Imagesurl))
                        {
                            System.IO.File.Delete(Imagesurl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                result.Success = true;
                result.Data = Images;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string CreateThumbnail(string src)
        {
            string UploadPath = Server.MapPath(ApplicationContext.AppSetting.Image_Upload_Path);
            string FileName = src.Substring(src.LastIndexOf("/") + 1).Replace(src.Substring(src.LastIndexOf(".")), "_thum.png");

            //原图加载

            src = src.Substring(src.LastIndexOf("/"));
            using (Image sourceImage = Image.FromFile(UploadPath + src))
            {
                int width = sourceImage.Width;
                int height = sourceImage.Height;
                var ThumFilePath = ApplicationContext.AppSetting.Image_Upload_Path;
                int s_width = (int)Math.Floor(Convert.ToDouble(width) * Convert.ToDouble(0.5));
                int s_height = (int)Math.Floor(Convert.ToDouble(height) * Convert.ToDouble(0.5));
                //新建一个图板,等比例压缩大小绘制原图
                using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(s_width, s_height))
                {
                    //绘制中间图
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        //高清,平滑
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.Clear(Color.White);
                        g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, s_width, s_height), new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.GraphicsUnit.Pixel);
                        g.Dispose();
                        UploadPath = UploadPath + FileName;
                        bitmap.Save(UploadPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                return ThumFilePath + FileName;
            }
        }
        public JsonResult _AjaxDeleteImages(string src = "")
        {
            return Json(DeleteImages(src));
        }

        /// 删除图片
        [HttpPost]
        public JsonResult DeleteImg(int id)
        {
            var result = service.Get(id);
            if (result.Success)
            {
                DeleteImages(result.Data.Src);
                DeleteImages(result.Data.ThumbnailSrc);
                DeleteImages(result.Data.OriginalSrc);
            }
            return Json(service.DeleteByEntityId(id));
        }

        [HttpPost]
        public ItemResult<bool> DeleteImages(string src = "")
        {
            ItemResult<bool> result = new ItemResult<bool>();
            try
            {
                src = HttpUtility.UrlDecode(src);
                var baseUrl = Server.MapPath(ApplicationContext.AppSetting.Image_Upload_Path);
                // string UploadPath = baseUrl + @"\upload\images\";
                try
                {
                    if (!string.IsNullOrWhiteSpace(src))
                    {
                        var _delefilename = src.Substring(src.LastIndexOf("/") + 1);
                        var Imagesurl = baseUrl + _delefilename;
                        if (System.IO.File.Exists(Imagesurl))
                        {
                            System.IO.File.Delete(Imagesurl);
                        }
                        result.Data = true;
                        result.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    result.Data = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult CopyImageToService(string file, string filename)
        {
            ItemResult<string> res = new ItemResult<string>();
            try
            {
                var index = filename.LastIndexOf("/");
                if (index > -1)
                    filename = filename.Substring(index + 1);
                //过滤特殊字符即可   
                string dummyData = file.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                if (dummyData.Length % 4 > 0)
                {
                    dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
                }
                byte[] filedata = Convert.FromBase64String(dummyData);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(filedata);
                System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("~/upload/images/") + filename, System.IO.FileMode.Create);
                ms.WriteTo(fs);
                ms.Close();
                fs.Close();
                fs = null;
                ms = null;
                res.Success = true;
                res.Data = filename;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult UploadImg(string file)
        {
            string message = string.Empty;
            //过滤特殊字符即可   
            string dummyData = file.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            byte[] filedata = Convert.FromBase64String(dummyData);

            //保存图片
            if (filedata.Length > 1048576)
            {
                message = "图片大小不能超过1M";
            }
            string fileextension = filedata[0].ToString() + filedata[1].ToString();
            if (fileextension == "7173")
            {
                fileextension = "gif";
            }
            else if (fileextension == "255216")
            {
                fileextension = "jpg";
            }
            else if (fileextension == "13780")
            {
                fileextension = "png";
            }
            else if (fileextension == "6677")
            {
                fileextension = "bmp";
            }
            else if (fileextension == "7373")
            {
                fileextension = "tif";
            }
            else
            {
                message = "上传的文件不是图片";
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString() + "." + fileextension;
            var kb = filedata.Length * 1.0 / 1024;
            string filePath = ApplicationContext.AppSetting.Image_Upload_Path + fileName;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(filedata);
            System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("~/upload/images/") + fileName, System.IO.FileMode.Create);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
            fs = null;
            ms = null;
            return Json(message);
        }

        public string UploadImage()
        {
            ItemResult<List<string>> result = new ItemResult<List<string>>();
            try
            {
                List<string> imgs = new List<string>();
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var upFile = Request.Files[i];
                    var FileLength = upFile.ContentLength;
                    string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();
                    if (ApplicationContext.AppSetting.AllowImageExt.IndexOf(ExtendName.ToLower()) == -1)
                    {
                        result.Message = "暂不支持“" + ExtendName + "”格式";
                        return JsonConvert.SerializeObject(result);
                    }
                    byte[] myData = new Byte[FileLength];
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks + ExtendName; ;
                    var res = service.ImageToOSS(filename, myData);
                    imgs.Add(res.Data);
                }
                result.Success = true;
                result.Data = imgs;
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string DeleteImage(string filename)
        {
            ItemResult<string> result = new ItemResult<string>();
            var res = service.OSSImageDelete(filename);
            result.Message = res.Message;
            result.Success = true;
            result.Data = res.Data;
            return JsonConvert.SerializeObject(result);
        }

    }
}
