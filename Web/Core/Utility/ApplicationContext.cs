using Utility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// 应用程序上下文
    /// </summary>
    public class ApplicationContext
    {
        /// <summary>
        /// 缓存
        /// </summary>
        public static CacheHelper Cache { get { return CacheHelper.Single; } }

        /// <summary>
        /// 汉字处理
        /// </summary>
        public static ChineseHelper Chinese { get { return ChineseHelper.Single; } }

        /// <summary>
        /// 配置文件帮助器
        /// </summary>
        public static ConfigHelper Config { get { return ConfigHelper.Single; } }

        /// <summary>
        /// Cookie
        /// </summary>
        public static CookieHelper Cookie { get { return CookieHelper.Single; } }

        /// <summary>
        /// 加密
        /// </summary>
        public static EncryptHelper Encrypt { get { return EncryptHelper.Single; } }

        /// <summary>
        /// Http
        /// </summary>
        public static HttpHelper Http { get { return HttpHelper.Single; } }

        /// <summary>
        /// Json
        /// </summary>
        public static JsonHelper Json { get { return JsonHelper.Single; } }

        /// <summary>
        /// 日志
        /// </summary>
        public static LogHelper Log { get { return LogHelper.Single; } }

        /// <summary>
        /// 属性
        /// </summary>
        public static PropertyHelper Property { get { return PropertyHelper.Single; } }

        /// <summary>
        /// 类型
        /// </summary>
        public static TypeHelper Type { get { return TypeHelper.Single; } }
        /// <summary>
        /// 图片
        /// </summary>
        public static ImageHelper Image { get { return ImageHelper.Single; } }

        /// <summary>
        /// 附件
        /// </summary>
        public static AttachmentHelper Attachment { get { return AttachmentHelper.Single; } }

        /// <summary>
        /// 二维码
        /// </summary>
        public static QRCodeHelper QRCode { get { return QRCodeHelper.Single; } }
        /// <summary>
        /// 直接数据访问
        /// </summary>
        public static SqlHelper SqlHelper { get { return SqlHelper.Single; } }

        /// <summary>
        /// 配置文件
        /// </summary>
        public static AppSettings AppSetting { get { return AppSettings.Single; } }

        /// <summary>
        /// 接口
        /// </summary>
        public static ApiHelper API { get { return ApiHelper.Single; } }


        /// <summary>
        /// 类型转换
        /// </summary>
        public static ConvertHelper Convert { get { return ConvertHelper.Single; } }

        /// <summary>
        /// 列表控件 Kendo 筛选 辅助
        /// </summary>
        public static KendoFilterHelper KendoFilter { get { return KendoFilterHelper.Single; } }


        //发布后的参数
        public static string parameters = "/MvcMall";

    }
}
