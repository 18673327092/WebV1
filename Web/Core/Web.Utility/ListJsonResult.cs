using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Utility
{
    public class ListJsonResult : JsonResult
    {


        private EnumDateFormat? _format = null;

        public ListJsonResult() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="jsonRequestBehavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        public ListJsonResult(object data, JsonRequestBehavior jsonRequestBehavior)
            : this(data, jsonRequestBehavior, null)
        {
            // 添加逻辑

        }

        public ListJsonResult(object data, JsonRequestBehavior jsonRequestBehavior, EnumDateFormat? format)
        {
            Data = data;
            JsonRequestBehavior = jsonRequestBehavior;
            _format = format;
        }

        public ListJsonResult(object data, EnumDateFormat? format)
        {
            Data = data;
            _format = format;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (Data != null)
            {
                var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                response.Write(JsonConvert.SerializeObject(Data, Formatting.Indented, timeConverter));
            }
        }
    }

    /// <summary>
    /// 日期格式枚举
    /// </summary>
    public enum EnumDateFormat
    {
        /// <summary>
        /// 日期
        /// </summary>
        DATE,
        /// <summary>
        /// 日期时间
        /// </summary>
        DATETIME,
        /// <summary>
        /// 时间
        /// </summary>
        TIME,
        /// <summary>
        /// 日期1
        /// </summary>
        DATE1
    }

    /// <summary>
    /// Json帮助类
    /// </summary>
    public sealed class JsonUtil
    {
        private const string _dateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        private const string _dateFormat = "yyyy'-'MM'-'dd";
        private const string _timeFormat = "HH':'mm':'ss";
        private const string _dateFormat1 = "yyyy'.'MM'.'dd";

        /// <summary>
        /// 获取默认Json序列化配置对象
        /// </summary>
        /// <param name="format">日期格式化类型</param>
        /// <returns></returns>
        public static JsonSerializerSettings GetDefaultJsonSerializerSettings(EnumDateFormat? format = null)
        {
            var settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new LongConverter());

            if (format == null)
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _dateTimeFormat });
            else
            {
                switch (format.Value)
                {
                    case EnumDateFormat.DATE:
                        settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _dateFormat });
                        break;
                    case EnumDateFormat.DATETIME:
                        settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _dateTimeFormat });
                        break;
                    case EnumDateFormat.TIME:
                        settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _timeFormat });
                        break;
                    case EnumDateFormat.DATE1:
                        settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _dateFormat1 });
                        break;
                    default:
                        settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = _dateTimeFormat });
                        break;
                }
            }

            return settings;
        }
    }

    /// <summary>
    /// Long型转换器
    /// </summary>
    public class LongConverter : JsonConverter
    {
        /// <summary>
        /// 是否能转换
        /// </summary>
        /// <param name="objectType">对象类型</param>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Int64) || objectType == typeof(Int64?) || objectType == typeof(UInt64) || objectType == typeof(UInt64?))
                return true;
            else
                return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                string text = value.ToString();
                writer.WriteValue(text);
            }
        }
    }
}
