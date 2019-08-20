using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Utility.Components
{
    /// <summary>
    /// 图片处理类
    /// 1、生成缩略图片或按照比例改变图片的大小和画质
    /// 2、将生成的缩略图放到指定的目录下
    /// </summary>
    public class AttachmentHelper
    {
        private AttachmentHelper()
        {
        }

        private static AttachmentHelper _single = new AttachmentHelper();

        public static AttachmentHelper Single
        {
            get { return _single; }
        }

        public string UploadAttachment(HttpPostedFileBase upFile, string baseUrl, string SavePath = "attactment", string deleteFilename = "")
        {
            try
            {
                var FileLength = upFile.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();
                byte[] myData = new Byte[FileLength];
                upFile.InputStream.Read(myData, 0, FileLength);
                string UploadPath = baseUrl + @"upload\" + SavePath + @"\";
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
                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                var FileUrl = "../../../upload/" + SavePath + NewFileName;
                //
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
                catch (Exception)
                {

                }
                return FileUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
