using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utility.Extension;
using Utility.ResultModel;
namespace Utility.Components
{
    public class ApiHelper
    {
        private ApiHelper()
        {
        }

        private static ApiHelper _single = new ApiHelper();

        public static ApiHelper Single
        {
            get { return _single; }
        }

        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="method"> post get put delete</param>
        /// <param name="url">url</param>
        /// <param name="data">data</param>
        /// <param name="headers">headers</param>
        public string WebClient(string method, string url, string parameters, Dictionary<string, string> dic)
        {
            return ApplicationContext.Http.WebClient(method, url, parameters, dic);
        }

        /// <summary>
        /// 创建接口参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string CreateApiParameter(params string[] parameters)
        {
            if (parameters.Length == 0) { return string.Empty; }
            if (parameters.Length % 2 != 0)
            {
                throw new Exception("请传入偶数个数的参数列表");
            }
            //参数拼接
            var sbPara = new StringBuilder();
            //api认证id
            var appid = ApplicationContext.AppSetting.API_AppID;
            sbPara.AppendFormat("appid={0}", appid);
            //时间戳
            var time_stamp = DateTime.Now.ToUinxTime();
            sbPara.AppendFormat("&time_stamp={0}", time_stamp);
            //加密
            var secret = ApplicationContext.Encrypt.Md5(ApplicationContext.AppSetting.API_AppSecret + time_stamp);
            var echo_str = secret.Substring(secret.Length - 6);
            sbPara.AppendFormat("&echo_str={0}", echo_str);
            for (int i = 0; i < parameters.Length; i += 2)
            {
                //自定义参数
                sbPara.AppendFormat("&{0}={1}", parameters[i], parameters[i + 1]);
            }
            return sbPara.ToString();
        }
        /// <summary>
        /// 创建接口参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string CreateApiParameter(Dictionary<string, string> parameters)
        {
            if (parameters.Count == 0) { return string.Empty; }
            if (parameters.Count % 2 != 0)
            {
                throw new Exception("请传入偶数个数的参数列表");
            }
            //参数拼接
            var sbPara = new StringBuilder();
            //api认证id
            var appid = ApplicationContext.AppSetting.API_AppID;
            sbPara.AppendFormat("appid={0}", appid);
            //时间戳
            var time_stamp = DateTime.Now.ToUinxTime();
            sbPara.AppendFormat("&time_stamp={0}", time_stamp);
            //加密
            var secret = ApplicationContext.Encrypt.Md5(ApplicationContext.AppSetting.API_AppSecret + time_stamp);
            var echo_str = secret.Substring(secret.Length - 6);
            sbPara.AppendFormat("&echo_str={0}", echo_str);
            foreach (var param in parameters)
            {
                //自定义参数
                sbPara.AppendFormat("&{0}={1}", param.Key, param.Value);
            }
            return sbPara.ToString();
        }

        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> PostApi<T>(string url, string parameters)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.API_DOMAIN + url, parameters);
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PostApiAsync<T>(string url, params object[] content)
        {
            var json = await HttpClientHelper.PostAsync(url, content);
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> PostApi<T>(string url, params object[] content)
        {
            var json = HttpClientHelper.Post(ApplicationContext.AppSetting.API_DOMAIN + url, content);
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }


        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> PostApi<T>(string api_url, params string[] parameters)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.API_DOMAIN + api_url, CreateApiParameter(parameters));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string PostApi(string api_url, string parameters, Dictionary<string, string> dic)
        {
            var str = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.API_DOMAIN + api_url, parameters, dic);
            return str;
        }

        /// <summary>
        /// 提交API并返回API结果对象
        /// 注意：对于复杂接口类型的返回值，data属性中可能还包含一串json需要解析
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> GetApi<T>(string api_url, params string[] parameters)
        {
            var json = ApplicationContext.Http.RequestUrl(ApplicationContext.AppSetting.API_DOMAIN + api_url + "?" + CreateApiParameter(parameters));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> Upload<T>(string api_url, params string[] parameters)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.FILE_DOMAIN + api_url, CreateApiParameter(parameters));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="api_url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ApiResult<T> UploadImage<T>(string file)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.FILE_DOMAIN + "Images/UploadImg", CreateApiParameter(new string[] { "file", file }));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public ApiResult<T> DeleteImages<T>(string src)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.FILE_DOMAIN + "Images/DeleteImages", CreateApiParameter(new string[] { "src", src }));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }



        /// <summary>
        /// 图片复制到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ApiResult<T> CopyImageToService<T>(string file, string filename)
        {
            var json = ApplicationContext.Http.WebClient_Post(ApplicationContext.AppSetting.FILE_DOMAIN + "Images/CopyImageToService", CreateApiParameter(new string[] { "file", file, "filename", filename }));
            return JsonConvert.DeserializeObject<ApiResult<T>>(json);
        }


    }
}
