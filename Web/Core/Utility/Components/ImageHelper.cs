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
    public class ImageHelper
    {
        private ImageHelper()
        {
        }

        private static ImageHelper _single = new ImageHelper();

        public static ImageHelper Single
        {
            get { return _single; }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
     

        public string UploadImg(HttpPostedFileBase upImage, string baseUrl, string SavePath = "Temp", string deleteFilename = "")
        {
            try
            {
                #region 原图

                var FileLength = upImage.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upImage.FileName).ToLower();
                byte[] myData = new Byte[FileLength];
                upImage.InputStream.Read(myData, 0, FileLength);
                string FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks;
                // string baseUrl = Server.MapPath("/");
                string UploadPath = baseUrl + @"upload\" + SavePath + @"\";
                string FilePath = UploadPath + @"images\" + FileName + ExtendName;
                var SourceFilePath = UploadPath + @"images\";
                if (!Directory.Exists(SourceFilePath))
                {
                    Directory.CreateDirectory(SourceFilePath);
                }
                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
                // var FileUrl = "../../../upload/" + SavePath + "/images/" + FileName + ExtendName;
                #endregion
                var FileUrl = "../../../upload/" + SavePath + @"/x" + ApplicationContext.AppSetting.DefaultThumbnail_Size + @"/" + FileName + ExtendName;
                string[] thumbnail_size = ApplicationContext.AppSetting.Thumbnail_Size.Split(new char[] { '|' });
                //原图加载
                using (Image sourceImage = Image.FromFile(FilePath))
                {
                    int width = sourceImage.Width;
                    int height = sourceImage.Height;
                    #region 缩略图

                    foreach (string size in thumbnail_size)
                    {
                        var ThumFilePath = UploadPath + @"x" + size + @"\";
                        if (!Directory.Exists(ThumFilePath))
                        {
                            Directory.CreateDirectory(ThumFilePath);
                        }
                        int s_width = (int)Math.Floor(Convert.ToDouble(width) * Convert.ToDouble(size));
                        int s_height = (int)Math.Floor(Convert.ToDouble(height) * Convert.ToDouble(size));
                        //新建一个图板,等比例压缩大小绘制原图
                        using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(s_width, s_height))
                        {
                            //绘制中间图
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                            {
                                //高清,平滑
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.Clear(Color.Transparent);
                                g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, s_width, s_height), new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.GraphicsUnit.Pixel);
                                g.Dispose();
                                ThumFilePath = ThumFilePath + FileName + ExtendName;
                                bitmap.Save(ThumFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }
                    #endregion
                    //if (width >= height)
                    //{
                    //    smallHeight = (int)Math.Floor(Convert.ToDouble(height) * (Convert.ToDouble(smallWidth) / Convert.ToDouble(width)));
                    //}
                    //else
                    //{
                    //    smallWidth = (int)Math.Floor(Convert.ToDouble(width) * (Convert.ToDouble(smallWidth) / Convert.ToDouble(height)));
                    //}
                }


                //
                #region 删除旧图
                try
                {
                    if (!string.IsNullOrWhiteSpace(deleteFilename))
                    {
                        var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
                        var sourceimg = UploadPath + @"images\" + _delefilename;
                        if (System.IO.File.Exists(sourceimg))
                        {
                            //删除原图
                            System.IO.File.Delete(sourceimg);
                        }

                        foreach (string size in thumbnail_size)
                        {
                            var th = UploadPath + @"x" + size + @"\" + _delefilename;
                            if (System.IO.File.Exists(th))
                            {
                                System.IO.File.Delete(th);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion
                return FileUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UploadImage(HttpPostedFileBase upFile, string baseUrl, string SavePath = "Temp", string deleteFilename = "")
        {
            try
            {

                var FileLength = upFile.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();
                byte[] myData = new Byte[FileLength];
                upFile.InputStream.Read(myData, 0, FileLength);
                string FileName = upFile.FileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                // string baseUrl = Server.MapPath("/");
                string UploadPath = baseUrl + @"upload\" + SavePath + @"\";
                string FilePath = UploadPath + @"images\" + FileName + ExtendName;
                var SourceFilePath = UploadPath + @"images\";
                if (!Directory.Exists(SourceFilePath))
                {
                    Directory.CreateDirectory(SourceFilePath);
                }
                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                var FileUrl = "../../../upload/" + SavePath + "/images/" + FileName + ExtendName;
                //
                try
                {
                    if (!string.IsNullOrWhiteSpace(deleteFilename))
                    {
                        var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
                        var attachmenturl = UploadPath + @"images\" + _delefilename;
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

        public string UploadAttachment(HttpPostedFileBase upFile, string baseUrl, string SavePath = "Temp", string deleteFilename = "")
        {
            try
            {
           
                var FileLength = upFile.ContentLength;
                string ExtendName = System.IO.Path.GetExtension(upFile.FileName).ToLower();
                byte[] myData = new Byte[FileLength];
                upFile.InputStream.Read(myData, 0, FileLength);
                string FileName = upFile.FileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                // string baseUrl = Server.MapPath("/");
                string UploadPath = baseUrl + @"upload\" + SavePath + @"\";
                string FilePath = UploadPath + @"attachment\" + FileName + ExtendName;
                var SourceFilePath = UploadPath + @"attachment\";
                if (!Directory.Exists(SourceFilePath))
                {
                    Directory.CreateDirectory(SourceFilePath);
                }
                FileStream newFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
             
                var FileUrl = "../../../upload/" + SavePath + "/attachment/" + FileName + ExtendName;
                //
                try
                {
                    if (!string.IsNullOrWhiteSpace(deleteFilename))
                    {
                        var _delefilename = deleteFilename.Substring(deleteFilename.LastIndexOf("/"));
                        var attachmenturl = UploadPath + @"attachment\" + _delefilename;
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

        #region  生成缩略图
        /// <summary>
        /// 生成缩略图重载方法1，返回缩略图的Image对象
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetThumbnailImage(string imageFilePath, int width, int height)
        {
            try
            {
                Image.GetThumbnailImageAbort callb = ThumbnailCallback;
                var image = Image.FromFile(imageFilePath);
                //大小不够不缩放
                if (image.Width < width || image.Height < height)
                {
                    return image;
                }
                var result = image.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                image.Dispose();
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法2，将缩略图文件保存到指定的路径
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool SaveThumbnailImage(string imageFilePath, int width, int height, string targetFilePath)
        {
            try
            {
                var reducedImage = GetThumbnailImage(imageFilePath, width, height);
                reducedImage.Save(@targetFilePath, ImageFormat.Png);

                reducedImage.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法3，返回缩略图的Image对象
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="percent">缩略图的宽度百分比如：需要百分之80，就填0.8</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetThumbnailImage(string imageFilePath, double percent)
        {
            try
            {
                var callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                var image = Image.FromFile(imageFilePath);

                var imageWidth = Convert.ToInt32(image.Width * percent);
                var imageHeight = Convert.ToInt32(image.Height * percent);

                Image reducedImage = image.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);

                return reducedImage;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法4，返回缩略图的Image对象
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="percent">缩略图的宽度百分比如：需要百分之80，就填0.8</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool SaveThumbnailImage(string imageFilePath, double percent, string targetFilePath)
        {
            try
            {
                Image reducedImage = GetThumbnailImage(imageFilePath, percent);
                reducedImage.Save(@targetFilePath, ImageFormat.Png);
                reducedImage.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法5，返回缩略图的Image对象
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="width">缩略图的宽度，高度自动计算</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetThumbnailImage(string imageFilePath, int width)
        {
            try
            {
                var callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                var image = Image.FromFile(imageFilePath);

                //比例
                double proportion = image.Width / width;

                var imageWidth = width;
                var imageHeight = Convert.ToInt32(image.Height / proportion);

                Image reducedImage = image.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);

                return reducedImage;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图重载方法6，返回缩略图的Image对象
        /// </summary>
        /// <param name="imageFilePath">图片文件的全路径名称</param>
        /// <param name="percent">缩略图的宽度百分比如：需要百分之80，就填0.8</param>
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:\Images\filename.jpg</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool SaveThumbnailImage(string imageFilePath, int width, string targetFilePath)
        {
            try
            {
                Image reducedImage = GetThumbnailImage(imageFilePath, width);
                reducedImage.Save(@targetFilePath, ImageFormat.Png);
                reducedImage.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        //#region 获取图片流
        ///// <summary>
        ///// 通过NET获取网络图片
        ///// </summary>
        ///// <param name="url">要访问的图片所在网址</param>
        ///// <param name="requestAction">对于WebRequest需要进行的一些处理，比如代理、密码之类</param>
        ///// <param name="responseFunc">如何从WebResponse中获取到图片</param>
        ///// <returns></returns>
        //public static Image GetImageFromNet(this Uri url, Action<WebRequest> requestAction = null, Func<WebResponse, Image> responseFunc = null)
        //{
        //    Image img;
        //    try
        //    {
        //        WebRequest request = WebRequest.Create(url);
        //        if (requestAction != null)
        //        {
        //            requestAction(request);
        //        }
        //        using (WebResponse response = request.GetResponse())
        //        {
        //            if (responseFunc != null)
        //            {
        //                img = responseFunc(response);
        //            }
        //            else
        //            {
        //                img = Image.FromStream(response.GetResponseStream());
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        img = null;
        //    }
        //    return img;
        //}

        //public static Image GetImageFromNet(this string url, Action<WebRequest> requestAction = null, Func<WebResponse, Image> responseFunc = null)
        //{
        //    return GetImageFromNet(new Uri(url), requestAction, responseFunc);
        //}

        //public static MemoryStream ConvertImgToStream(Image img)
        //{
        //    BinaryFormatter binFormatter = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream();
        //    binFormatter.Serialize(ms, img);
        //    return ms;
        //}

        //public static MemoryStream GetImgStreamFromNet(string ImgUrl)
        //{
        //    Image img = GetImageFromNet(ImgUrl,
        //     (request) =>
        //     {//此处可以对request进行相关设定,因为此部分均为基类，所以也可以用于FtpWebRequest之类  
        //         request.Timeout = 2000;
        //     });
        //    return ConvertImgToStream(img);
        //} 
        //#endregion
    }
}
