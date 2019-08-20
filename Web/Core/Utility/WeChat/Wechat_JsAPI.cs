using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Utility.WeChat
{
    public class Wechat_JsAPI
    {
        public static Wechat_JsAPI _Wechat_JsAPI { get; set; }
        public static Wechat_JsAPI Single
        {
            get
            {
                if (_Wechat_JsAPI == null)
                {
                    _Wechat_JsAPI = new Wechat_JsAPI();
                }
                return _Wechat_JsAPI;
            }
        }
        /// <summary>
        /// Token超时时间（分）
        /// </summary>
        private int JsapiTicket_Timeout = 100;
        Wechat_UrlGenerator UrlGenerator = new Wechat_UrlGenerator();

        /// <summary>
        /// 获取微信ticket
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result_JsapiTicket GetJSAPITicket(string appid, string access_token)
        {
            //生成URL
            var url = UrlGenerator.JSAPI_GetJSAPI_TicketUrl(access_token);
            //请求
            var json = WebClient_Get(url);
            //解析结果
            var ticket = JsonToObject<Result_JsapiTicket>(json);
            //超时
            JsapiTicket_Timeout = Convert.ToInt32(Math.Floor((ticket.expires_in / 60d) - 20d));
            return ticket;
        }

        /// <summary>
        /// 生成微信配置签名
        /// </summary>
        /// <returns></returns>
        public string GenerateSignature(string timestamp, string noncestr, string jsapi_ticket, string url)
        {
            var parameter = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
            var value = string.Format(parameter, jsapi_ticket, noncestr, timestamp, url);
            return SHA1(value);
        }

        /// <summary>
        /// 使用缺省密钥给字符串加密（支持微信）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SHA1(string input)
        {
            //FormsAuthentication.HashPasswordForStoringInConfigFile(content.ToString(), "SHA1");

            byte[] buffer = Encoding.Default.GetBytes(input);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            buffer = iSHA.ComputeHash(buffer);
            var ret = new StringBuilder();
            foreach (byte iByte in buffer)
            {
                ret.AppendFormat("{0:x2}", iByte);
            }
            return ret.ToString();
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T JsonToObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// WebClient-Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string WebClient_Get(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }
                var result = client.DownloadString(url);
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}