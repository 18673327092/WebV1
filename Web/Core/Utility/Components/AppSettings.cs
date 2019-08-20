using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Components
{
    public class AppSettings
    {
        private AppSettings()
        {
        }

        private static AppSettings _single = new AppSettings();

        public static AppSettings Single
        {
            get { return _single; }
        }

        #region 站点信息
        /// <summary>
        /// 网站静态资源路径（css、javascript）
        /// </summary>
        public string WEB_SITE_STATIC_RESOURCE_PATH = ApplicationContext.Config.GetAppSetting("WEB_SITE_STATIC_RESOURCE_PATH");

        /// <summary>
        /// 站点名称
        /// </summary>
        public string WEB_SITE_NAME = ApplicationContext.Config.GetAppSetting("WEB_SITE_NAME");

        /// <summary>
        /// 站点Tag
        /// </summary>
        public string WEB_SITE_TAG = ApplicationContext.Config.GetAppSetting("WEB_SITE_TAG");


        /// <summary>
        /// 是否开启站点授权
        /// </summary>
        public string IS_ENABLE_SITEAUTH = ApplicationContext.Config.GetAppSetting("IS_ENABLE_SITEAUTH", "false");

        /// <summary>
        /// 允许上传的附件格式
        /// </summary>
        public string AllowFileExt = ApplicationContext.Config.GetAppSetting("AllowFileExt", "*.jpg;*.jpeg;*.gif;*.png;*.doc;*.xls;*.ppt;*.txt;*.zip;*.rar;*.pdf;");

        /// <summary>
        /// 允许上传的附件最大值 KB
        /// </summary>
        public int FileSize = Convert.ToInt32(ApplicationContext.Config.GetAppSetting("FileSize", "1024"));

        /// <summary>
        /// 允许上传的图片最大值 KB
        /// </summary>
        public int AllowImageSize = Convert.ToInt32(ApplicationContext.Config.GetAppSetting("AllowImageSize", "10240"));

        /// <summary>
        /// 存储方式 1=本地，2=OSS
        /// </summary>
        public int StorageMode = Convert.ToInt32(ApplicationContext.Config.GetAppSetting("StorageMode", "1"));

        /// <summary>
        /// 七牛 AccessKey
        /// </summary>
        public string QiNiu_AccessKey = ApplicationContext.Config.GetAppSetting("QiNiu_AccessKey", "");

        /// <summary>
        /// 七牛 SecretKey
        /// </summary>
        public string QiNiu_SecretKey = ApplicationContext.Config.GetAppSetting("QiNiu_SecretKey", "");

        /// <summary>
        /// OOS  空间名
        /// </summary>
        public string QiNiu_Bucket = ApplicationContext.Config.GetAppSetting("QiNiu_Bucket", "");

        /// <summary>
        /// OOS 域名
        /// </summary>
        public string QiNiu_Domain = ApplicationContext.Config.GetAppSetting("QiNiu_Domain", "");

        /// <summary>
        /// 阿里云 AccessKey
        /// </summary>
        public string Aliyun_AccessKey = ApplicationContext.Config.GetAppSetting("Aliyun_AccessKey", "");

        /// <summary>
        /// 阿里云 SecretKey
        /// </summary>
        public string Aliyun_SecretKey = ApplicationContext.Config.GetAppSetting("Aliyun_SecretKey", "");

        /// <summary>
        /// 阿里云  空间名
        /// </summary>
        public string Aliyun_Bucket = ApplicationContext.Config.GetAppSetting("Aliyun_Bucket", "");

        /// <summary>
        /// 阿里云 EndPoint
        /// </summary>
        public string Aliyun_EndPoint = ApplicationContext.Config.GetAppSetting("Aliyun_EndPoint", "");

        /// <summary>
        /// 阿里云 域名
        /// </summary>
        public string Aliyun_Domain = ApplicationContext.Config.GetAppSetting("Aliyun_Domain", "");

        /// <summary>
        /// 允许上传的图片格式
        /// </summary>
        public string AllowImageExt = ApplicationContext.Config.GetAppSetting("AllowImageExt", "*.jpg;*.jpeg;*.gif;*.png;");

        /// <summary>
        /// 文件上传
        /// </summary>
        public string File_Upload_Path
        {
            get { return ApplicationContext.Config.GetAppSetting("File_Upload_Path", "/upload/files"); }
        }

        /// <summary>
        /// 图片上传地址
        /// </summary>
        public string Image_Upload_Path
        {
            get { return ApplicationContext.Config.GetAppSetting("Image_Upload_Path", "/upload/images"); }
        }

        #endregion

        #region 接口参数

        /// <summary>
        /// 接口域名地址
        /// </summary>
        public string API_DOMAIN
        {
            get { return ApplicationContext.Config.GetAppSetting("API_DOMAIN", "http://localhost/"); }
        }

        /// <summary>
        /// 文件/图片上传地址
        /// </summary>
        public string FILE_DOMAIN
        {
            get { return ApplicationContext.Config.GetAppSetting("File_Domain", ""); }
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageBaseUrl
        {
            get { return ApplicationContext.Config.GetAppSetting("ImageBaseUrl", ""); }
        }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string API_AppID
        {
            get { return ApplicationContext.Config.GetAppSetting("API_APPID", "olsf35fb23bbc824409"); }
        }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string API_AppSecret
        {
            get { return ApplicationContext.Config.GetAppSetting("API_APPSECRET", "72b25ec5b7f54c19838ebda1f96a84528a9bc15b8d494311b537d2c653737630"); }
        }

        #endregion 接口地址

        #region DES加解密

        public string DES_KEY
        {
            get { return ApplicationContext.Config.GetAppSetting("DES_KEY", "iaf/vi7IfG4="); }
        }

        public string DES_IV
        {
            get { return ApplicationContext.Config.GetAppSetting("DES_IV", "D7AuYXmvzd4="); }
        }

        #endregion DES加解密

        #region 后台管理

        /// <summary>
        /// 管理后台管理员登录超时（分钟）
        /// </summary>
        public int Admin_LogonTimeOutMinutes
        {
            get { return ApplicationContext.Config.GetAppSettingInt("Admin_LogonTimeOutMinutes", 60); }
        }

        /// <summary>
        /// 生成缩略图尺寸
        /// </summary>
        public string Thumbnail_Size
        {
            get { return ApplicationContext.Config.GetAppSetting("Thumbnail_Size", "0.4|0.6|0.8"); }
        }

        /// <summary>
        /// 默认缩略图尺寸
        /// </summary>
        public string DefaultThumbnail_Size
        {
            get { return ApplicationContext.Config.GetAppSetting("DefaultThumbnail_Size", "0.4"); }
        }

        /// <summary>
        /// 是否需要登录
        /// </summary>
        public bool IS_NeedLogin
        {
            get { return Convert.ToBoolean(ApplicationContext.Config.GetAppSetting("IS_NeedLogin", "true")); }
        }

        /// <summary>
        /// 管理员默认密码
        /// </summary>
        public string AdminDefaultPassword
        {
            get { return ApplicationContext.Config.GetAppSetting("AdminDefaultPassword", "123123"); }
        }


        #endregion 后台管理



    }
}
