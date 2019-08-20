using Base.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;
using Utility.ResultModel;

namespace Web.Controllers
{
    public class UploadController : Controller
    {
        IImagesService imageService;
        public UploadController(IImagesService _imageService)
        {
            imageService = _imageService;
        }
        // GET: Upload
        /// <summary>
        /// 图片上传（Files）
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadImageFile()
        {
            ItemResult<List<string>> result = new ItemResult<List<string>>();
            try
            {
                List<string> imgs = new List<string>();
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    string fileurl = string.Empty;
                    var file = Request.Files[i];
                    var length = file.ContentLength;
                    string extName = System.IO.Path.GetExtension(file.FileName).ToLower();
                    if (ApplicationContext.AppSetting.AllowImageExt.IndexOf(extName.ToLower()) == -1)
                    {
                        result.Message = "请上传 " + ApplicationContext.AppSetting.AllowImageExt + "格式的图片";
                        return Json(result);
                    }
                    byte[] myData = new Byte[length];
                    var kb = myData.Length * 1.0 / 1024;
                    if (kb > ApplicationContext.AppSetting.AllowImageSize)
                    {
                        result.Message = "请上传小于" + ApplicationContext.AppSetting.AllowImageSize + " KB的图片";
                        return Json(result);
                    }
                    file.InputStream.Read(myData, 0, length);
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks + extName;

                    //本地存储
                    var Image_Upload_Path = Server.MapPath(ApplicationContext.AppSetting.Image_Upload_Path) + DateTime.Now.ToString("yyyyMMdd") + "/";
                    if (!Directory.Exists(Image_Upload_Path))
                    {
                        Directory.CreateDirectory(Image_Upload_Path);
                    }

                    FileStream newFile = new FileStream(Image_Upload_Path + filename, FileMode.Create, FileAccess.Write);
                    newFile.Write(myData, 0, myData.Length);
                    newFile.Close();
                   fileurl = filename;
                    //存储OSS
                    if (ApplicationContext.AppSetting.StorageMode == 2)
                    {
                        var res = imageService.ImageToOSS(filename, myData);
                        if (res.Success) fileurl = res.Data;
                    }
                    imgs.Add(fileurl);
                }
                result.Success = true;
                result.Data = imgs;
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 图片上传 64位字符串
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImage(string file)
        {
            ItemResult<string> result = new ItemResult<string>();
            string message = string.Empty;
            //过滤特殊字符即可   
            string dummyData = file.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            byte[] filedata = Convert.FromBase64String(dummyData);
            if (filedata.Length > ApplicationContext.AppSetting.AllowImageSize)
            {
                result.Message = "请上传大小" + ApplicationContext.AppSetting.AllowImageSize + "KB以内的图片";
                return Json(result);
            }
            string extName = filedata[0].ToString() + filedata[1].ToString();
            if (extName == "7173")
            {
                extName = "gif";
            }
            else if (extName == "255216")
            {
                extName = "jpg";
            }
            else if (extName == "13780")
            {
                extName = "png";
            }
            else if (extName == "6677")
            {
                extName = "bmp";
            }
            else if (extName == "7373")
            {
                extName = "tif";
            }
            if (ApplicationContext.AppSetting.AllowImageExt.IndexOf(extName.ToLower()) == -1)
            {
                result.Message = "请上传" + ApplicationContext.AppSetting.AllowImageExt + "格式的图片";
                return Json(result);
            }
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString() + "." + extName;
            //本地存储
            var Image_Upload_Path = Server.MapPath(ApplicationContext.AppSetting.Image_Upload_Path) + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(Image_Upload_Path))
            {
                Directory.CreateDirectory(Image_Upload_Path);
            }
            string filePath = Image_Upload_Path + fileName;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(filedata);
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
            fs = null;
            ms = null;
            return Json(result);
        }


        //public JsonResult A() {

        //}
    }
}