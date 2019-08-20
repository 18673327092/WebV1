using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Util;
using Qiniu.Storage;
using Newtonsoft.Json;
using Base.Model.Base.Model;
using Aliyun.OSS;
using System.IO;

namespace Base.Service
{
    /// <summary>
    ///图片业务类
    /// </summary>
    public class ImagesService : BaseService<Base_Images>, IImagesService
    {
        public ListResult<Base_Images> GetPagingList(Base_Images request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        public async Task<ItemResult<Base_Images>> GetAsync(int id)
        {
            return await Task.Run(() =>
            {
                ItemResult<Base_Images> result = new ItemResult<Base_Images>();
                try
                {
                    result.Data = base.Get(id).Data;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = "网络异常";
                    ApplicationContext.Log.Error("ImagesService.GetAsync", ex);
                }
                return result;
            });
        }

        public async Task<ItemResult<string>> GetSrc(int id)
        {
            return await Task.Run(() =>
            {
                ItemResult<string> result = new ItemResult<string>();
                try
                {
                    result.Data = ApplicationContext.AppSetting.FILE_DOMAIN + base.Get(id).Data.Src;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = "网络异常";
                    ApplicationContext.Log.Error("ImagesService.GetAsync", ex);
                }
                return result;
            });
        }
        public async Task<ItemResult<string>> GetOriginalSrc(int id)
        {
            return await Task.Run(() =>
            {
                ItemResult<string> result = new ItemResult<string>();
                try
                {
                    result.Data = ApplicationContext.AppSetting.FILE_DOMAIN + base.Get(id).Data.OriginalSrc;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = "网络异常";
                    ApplicationContext.Log.Error("ImagesService.GetAsync", ex);
                }
                return result;
            });
        }

        public async Task<ItemResult<string>> GetThumbnailSrc(int id)
        {
            return await Task.Run(() =>
            {
                ItemResult<string> result = new ItemResult<string>();
                try
                {
                    result.Data = ApplicationContext.AppSetting.FILE_DOMAIN + base.Get(id).Data.ThumbnailSrc;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = "网络异常";
                    ApplicationContext.Log.Error("ImagesService.GetAsync", ex);
                }
                return result;
            });
        }

        public ItemResult<string> ImageToOSS(string filename, byte[] data, string localFilename = "")
        {
            ItemResult<string> res = new ItemResult<string>();
            if (!string.IsNullOrEmpty(ApplicationContext.AppSetting.QiNiu_SecretKey))
            {
                var qiniu_res = UploadQiNiu(filename, data);
                if (qiniu_res.Success)
                {
                    res.Data = qiniu_res.Data;
                    res.Success = true;
                }
                else
                {
                    res.Success = false;
                    res.Message = qiniu_res.Message;
                }
            }
            else if (!string.IsNullOrEmpty(ApplicationContext.AppSetting.Aliyun_SecretKey))
            {
                var aliyun_res = ImageToAliyun(filename, data);
                if (aliyun_res.Success)
                {
                    res.Data = aliyun_res.Data;
                    res.Success = true;
                }
                else
                {
                    res.Success = false;
                    res.Message = aliyun_res.Message;
                }
            }
            return res;
        }

        /// <summary>
        /// 上传到阿里云
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <param name="localFilename"></param>
        /// <returns></returns>
        public ItemResult<string> ImageToAliyun(string filename, byte[] data)
        {
            ItemResult<string> res = new ItemResult<string>();
            //上传到阿里云  
            using (Stream fileStream = new MemoryStream(data))//转成Stream流  
            {
                // var fs = Request.Files[UploadConfig.UploadFieldName];
                // string md5 = OssUtils.ComputeContentMd5(fileStream, fs.ContentLength);
                string today = DateTime.Now.ToString("yyyyMMdd");
                //  string fileName = filename + today + Path.GetExtension(filename);//文件名=文件名+当前上传时间  
                string filePath = "Upload/Images/" + today + "/" + filename;//云文件保存路径  
                try
                {
                    //初始化阿里云配置--外网Endpoint、访问ID、访问password  
                    // 创建OSSClient实例。
                    var aliyun = new OssClient(ApplicationContext.AppSetting.Aliyun_EndPoint,
                        ApplicationContext.AppSetting.Aliyun_AccessKey, ApplicationContext.AppSetting.Aliyun_SecretKey);
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5  
                    //  var objectMeta = new ObjectMetadata
                    // {
                    //  ContentMd5 = md5,
                    // };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)  
                    aliyun.PutObject(ApplicationContext.AppSetting.Aliyun_Bucket, filePath, fileStream);
                    res.Data = filePath;
                    res.Success = true;
                }
                catch (Exception e)
                {
                    res.Message = e.Message;
                }
            }
            return res;
        }

        /// <summary>
        /// 图片上传到七牛
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static ItemResult<string> UploadQiNiu(string filename, byte[] data)
        {
            ItemResult<string> res = new ItemResult<string>();
            Mac mac = new Mac(ApplicationContext.AppSetting.QiNiu_AccessKey, ApplicationContext.AppSetting.QiNiu_SecretKey);
            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = ApplicationContext.AppSetting.QiNiu_Bucket;
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
            Qiniu.Http.HttpResult result = target.UploadData(data, filename, token, null);
            res.Data = JsonConvert.DeserializeObject<QiniuOss>(result.Text).key;
            res.Success = true;
            return res;
        }

        public ItemResult<string> OSSImageDelete(string filename)
        {
            ItemResult<string> res = new ItemResult<string>();
            Mac mac = new Mac(ApplicationContext.AppSetting.QiNiu_AccessKey, ApplicationContext.AppSetting.QiNiu_SecretKey);
            // 设置存储区域
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_East;
            BucketManager bucketManager = new BucketManager(mac, config);
            Qiniu.Http.HttpResult deleteRet = bucketManager.Delete(ApplicationContext.AppSetting.QiNiu_Bucket, filename);
            if (deleteRet.Code != (int)Qiniu.Http.HttpCode.OK)
            {
                res.Message = deleteRet.ToString();
            }
            else
            {
                res.Success = true;
            }
            return res;
        }
    }

    public class QiniuOss
    {
        public string hash { get; set; }
        public string key { get; set; }
    }
}
