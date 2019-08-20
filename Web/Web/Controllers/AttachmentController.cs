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
using System.IO;
using Base.Model;
using Base.Model.Sys.Model;
using Qiniu.Storage;
using Qiniu.Util;

namespace Web.Controllers
{
    public class AttachmentController : AdminBaseController
    {
        public IAttachmentService service { get; set; }
        public AttachmentController()
        {
            EntityName = "Base_Attachment";
        }

        #region 通用方法（除_Save方法的实体参数外，其他方法不用改）

        #region 表单页面
        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            base.BaseForm(id);
            return View();
        }
        #endregion

        #region 表单提交
        [HttpPost]
        public JsonResult _Save(Base_Attachment entity, AdminCredential User)
        {
            if (Request.Files.Count > 0)
            {
                if (Request.Files["file_Attachment"].ContentLength > 0)
                    entity.Attachment = UploadAttachment("file_Attachment", "附件", entity.Attachment);
            }
            if (entity.ID == 0)
            {
                return Json(service.Insert(entity), JsonRequestBehavior.DenyGet);
            }
            else
            {
                ApplicationContext.Cache.Remove(EntityName + entity.ID);
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserID = User.ID;
                return Json(service.Update(entity), JsonRequestBehavior.DenyGet);
            }
        }
        #endregion

        #region 数据删除
        [HttpPost]
        public JsonResult _Delete(string ids)
        {
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

        public string AjaxAttachmentUpload(int UserID = 0)
        {
            try
            {
                var file = Request.Files[0];
                var uploadresult = UploadAttachment(Request.Files[0], "附件");
                if (uploadresult.Success)
                {
                    Base_Attachment attachment = uploadresult.Data;
                    attachment.OwnerID = UserID;
                    attachment.CreateTime = DateTime.Now;
                    attachment.UpdateTime = DateTime.Now;
                    attachment.StateCode = 1;
                    attachment.UpdateUserID = UserID;
                    var result = service.Insert(attachment);
                    attachment.ID = result.Data;
                }
                return JsonConvert.SerializeObject(uploadresult);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public bool DeleteFileAttachment(int id)
        {
            try
            {
                var attachment = service.Get(id).Data;
                string filepath = attachment.Attachment;
                if (!string.IsNullOrWhiteSpace(attachment.Attachment))
                {
                    var baseUrl = Server.MapPath("/");
                    //string file = "upload/附件/786375650181127085 (1)(1340).png";
                    filepath = filepath.Replace(ApplicationContext.AppSetting.File_Upload_Path, "");
                    var SavePath = filepath.Split(new char[] { '/' })[0];
                    string UploadPath = baseUrl + @"\upload\" + SavePath + @"\";
                    var attachmenturl = UploadPath + filepath.Split(new char[] { '/' })[1];
                    if (System.IO.File.Exists(attachmenturl))
                    {
                        System.IO.File.Delete(attachmenturl);
                    }
                    service.Delete(attachment);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static readonly string AccessKey = "v7woBIW694hgxjLiURDzmS42YQdGWqzmffoq-t4T";
        private static readonly string SecretKey = "EK5w9ydWIEYFiuK1sXIuHFMCZkz1Z_nTNp0_KFhM";
        // 存储空间名
        private static readonly string Bucket = "mall";
        public ItemResult<string> ImageToQiNiu(string key, byte[] data)
        {
            ItemResult<string> res = new ItemResult<string>();
            Mac mac = new Mac(AccessKey, SecretKey);
            // 上传文件名
            //   string key = "12312312312313123213";
            // 本地文件路径
            string filePath = "D:\\tools\\putty.exe";

            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = Bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传token
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Qiniu.Storage.Config config = new Qiniu.Storage.Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_East;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            // 表单上传
            FormUploader target = new FormUploader(config);
            Qiniu.Http.HttpResult result = target.UploadData(data, key, token, null);
            res.Message = res.Message;
            return res;
        }

        public ItemResult<Base_Attachment> UploadAttachment(HttpPostedFileBase upFile, string SavePath = "attactment", string deleteFilename = "")
        {
            ItemResult<Base_Attachment> result = new ItemResult<Base_Attachment>();
            Base_Attachment attachment = new Base_Attachment();
            try
            {
                var baseUrl = Server.MapPath("/");
                var FileLength = upFile.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();

                if (ApplicationContext.AppSetting.AllowFileExt.IndexOf(ExtendName.ToLower()) == -1)
                {
                    result.Message = "上传文件暂不支持“" + ExtendName + "”格式";
                    return result;
                }
                byte[] myData = new Byte[FileLength];


                upFile.InputStream.Read(myData, 0, FileLength);
                string UploadPath = baseUrl + @"\upload\" + SavePath + @"\";
                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }
                string NewFileName = upFile.FileName;
                string FilePath = UploadPath + upFile.FileName;
                if (System.IO.File.Exists(FilePath))
                {
                    NewFileName = upFile.FileName.Replace(ExtendName, "") + "(" + DateTime.Now.ToString("mmss") + ")" + ExtendName; ;
                    FilePath = UploadPath + NewFileName;
                }

                var res = ImageToQiNiu(NewFileName, myData);
                result.Message = res.Message;

                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                attachment.Name = NewFileName;
                attachment.Attachment = ApplicationContext.AppSetting.File_Upload_Path + SavePath + "/" + NewFileName;
                var kb = myData.Length * 1.0 / 1024;
                if (kb > 1024)
                {
                    attachment.Size = (kb / 1024).ToString("0.00") + "M";
                }
                else
                {
                    attachment.Size = kb.ToString("0.00") + "KB";
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(deleteFilename))
                    {
                        var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
                        var attachmenturl = UploadPath + _delefilename;
                        if (System.IO.File.Exists(attachmenturl))
                        {
                            System.IO.File.Delete(attachmenturl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                result.Success = true;
                result.Data = attachment;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///启用附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Enable(string json)
        {
            List<Base_Attachment> list = JsonConvert.DeserializeObject<List<Base_Attachment>>(json);
            return Json(service.Enable(list));
        }
    }
}
