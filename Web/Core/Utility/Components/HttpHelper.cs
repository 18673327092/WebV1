using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility.Components
{
    public class HttpHelper
    {
        /// <summary>
        /// HTTP
        /// </summary>
        private HttpHelper()
        {
        }

        private static HttpHelper _single = new HttpHelper();

        public static HttpHelper Single
        {
            get { return _single; }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="method"> post get put delete</param>
        /// <param name="url">url</param>
        /// <param name="data">data</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public string WebClient(string method, string url, string data, Dictionary<string, string> headers = null)
        {
            var client = new WebClient();
            try
            {
                //设置请求头信息
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }
                if (method.ToUpper() == "GET")
                {
                    client.Encoding = Encoding.UTF8;
                    var result = client.DownloadString(url);
                    return result;
                }
                else //POST  PUT
                {
                    byte[] sendData = Encoding.UTF8.GetBytes(data);
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.Headers.Add("ContentLength", sendData.Length.ToString());
                    var result = client.UploadData(url, method, sendData);
                    return Encoding.UTF8.GetString(result);
                }

            }
            catch (Exception ex)
            {
                //   ApplicationContext.Log.Error("WebClient", $"{method}_{url}_{data}_{ex.Message}");
                return "";
            }
            finally
            {
                client.Dispose();
            }
        }
        //HTTPSQ请求  
        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

        #region webclient
        /// <summary>
        /// WebClient-Get请求 async
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> WebClient_GetAsync(string url, Dictionary<string, string> headers = null)
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
                var result = await client.DownloadStringTaskAsync(url);
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("WebClient_GetAsync", url + "_" + ex.Message);
                return "";
            }
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
        public string WebClient_Get(string url, Dictionary<string, string> headers, Encoding encoding)
        {
            try
            {
                var client = new WebClient();
                client.Encoding = encoding;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }
                return client.DownloadString(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string HttpWebRequest_Get(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 30000;
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                //设置连接超时时间
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result = "";
                using (Stream streamReceive = response.GetResponseStream())
                {
                    using (GZipStream zipStream = new GZipStream(streamReceive, CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new StreamReader(zipStream, encoding))
                        {
                            result = sr.ReadToEnd();
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// HttpClient-Get请求 async
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream WebClient_GetStream(string url, Dictionary<string, string> headers = null)
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
                var result = client.OpenRead(url);
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("WebClient_Get", url + "_" + ex.Message);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// WebClient-POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public MemoryStream WebClient_PostStream(string url, string postData, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = new WebClient();

                //设置请求头信息
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }

                // 构造POST参数
                byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString());
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", sendData.Length.ToString());
                var result = client.UploadData(url, "POST", sendData);
                return new MemoryStream(result);
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("WebClient_PostStream", url + "_" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// WebClient-POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string WebClient_Post(string url, string postData, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = new WebClient();
                //设置请求头信息
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }
                // 构造POST参数
                byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString());
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", sendData.Length.ToString());
                var result = client.UploadData(url, "POST", sendData);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("WebClient_Post", url + "_" + postData + "_" + ex.Message);
                return "";
            }
        }

        public string WebClient_Post_ByJson(string url, string postData)
        {
            try
            {
                var client = new WebClient();
                // 构造POST参数
                byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString());
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("ContentLength", sendData.Length.ToString());
                var result = client.UploadData(url, "POST", sendData);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// HttpClient-POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public async Task<string> HttpClient_Post(string url, string postData, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = new HttpClient();
                //设置请求头信息
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                //client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");

                // HTTP KeepAlive设为false，防止HTTP连接保持
                client.DefaultRequestHeaders.Add("KeepAlive", "false");

                // 构造POST参数
                var content = new StringContent(postData, Encoding.UTF8);
                var response = await client.PostAsync(url, content);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("HttpClient_Post", url + "_" + ex.Message);
                return "";
            }
        }

        #region 暂不使用


        /// <summary>
        /// post请求，复杂
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {
            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var result = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();
            return result;
        }

        #endregion 暂不使用

        #region 获得客户端IP

        /// <summary>
        /// 获得客户端IP
        /// </summary>
        /// <returns></returns>
        public string GetClientIp()
        {
            //穿过代理服务器取远程用户真实IP地址
            var ip = string.Empty;
            if (HttpContext.Current == null)
            {
                return "127.0.0.1";
            }
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                    {
                        ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    }
                    else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                    {
                        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
                else
                {
                    ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
            }
            else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }

        #endregion 获得客户端IP

        #region 请求Url，不发送数据，目前没有用到

        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        private string Request_PostUrl(string url, string data, string method = "post")
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = method;
                request.ContentType = "text/html";
                request.Headers.Add("charset", "utf-8");

                //发送请求并获取相应回应数据//直到request.GetResponse()程序才开始向目标网页发送Post请求
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                //返回结果页信息
                var content = sr.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("Request_PostUrl", url + "_" + ex.Message);
                return "";
            }
        }

        #endregion 请求Url，不发送数据，目前没有用到

        #region 请求Url，发送数据 暂时不用

        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public string PostUrl_No(string url, string postData)
        {
            try
            {
                byte[] responseData = null;
                Stream requestStream = null;
                HttpWebResponse response = null;
                Stream responseStream = null;
                MemoryStream ms = null;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Post;
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    if (data != null && data.Length > 0)
                    {
                        request.ContentLength = data.Length;
                        requestStream = request.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    //由于微信服务器的响应有时没有正确设置ContentLength，这里不检查ContentLength
                    //if (response.ContentLength > 0)
                    {
                        ms = new MemoryStream();
                        responseStream = response.GetResponseStream();
                        int bufferLength = 2048;
                        byte[] buffer = new byte[bufferLength];
                        int size = responseStream.Read(buffer, 0, bufferLength);
                        while (size > 0)
                        {
                            ms.Write(buffer, 0, size);
                            size = responseStream.Read(buffer, 0, bufferLength);
                        }
                        responseData = ms.ToArray();
                    }
                }
                finally
                {
                    if (requestStream != null)
                        requestStream.Close();
                    if (responseStream != null)
                        responseStream.Close();
                    if (ms != null)
                        ms.Close();
                    if (response != null)
                        response.Close();
                }
                return Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("PostUrl_No", url + "_" + ex.Message);
                return "";
            }
        }

        #endregion 请求Url，发送数据 暂时不用


        #region HttpRequest方式发送请求

        /// <summary>
        /// 使用HttpRequest发送请求，默认GET
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string RequestUrl(string url, string data = "", string method = "GET")
        {
            try
            {
                HttpWebRequest request = null;
                //HTTPS请求 
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                // 设置参数
                request = WebRequest.Create(url) as HttpWebRequest;
                //
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = method;
                request.ContentType = "application/json";
                //适应https协议
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //request.ContentType = "application/x-www-form-urlencoded";
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //request.AllowAutoRedirect = true;
                //request.KeepAlive = false;
                //request.ContentType = "text/html";
                //request.Headers.Add("charset", "utf-8");

                if (!string.IsNullOrEmpty(data))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(data);
                    using (var stream = request.GetRequestStream())
                    {
                        //头部字节流
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                //发送请求并获取相应回应数据
                var response = request.GetResponse() as HttpWebResponse;
                var result = response.GetResponseStream();
                var sr = new StreamReader(result, Encoding.UTF8);
                //返回结果页信息
                var content = sr.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("请求地址：", url + (string.IsNullOrEmpty(data) ? "" : "    请求参数：" + data) + "    错误信息：" + ex.Message);
                return "";
            }
        }
        #endregion

        #region 下载文件

        public void DownFile(string url, string path, string extention)
        {
            try
            {
                WebClient client = new WebClient();
                Stream str = client.OpenRead(url);
                StreamReader reader = new StreamReader(str);
                byte[] mbyte = new byte[1000000];
                int allmybyte = (int)mbyte.Length;
                int startmbyte = 0;

                while (allmybyte > 0)
                {

                    int m = str.Read(mbyte, startmbyte, allmybyte);
                    if (m == 0)
                        break;

                    startmbyte += m;
                    allmybyte -= m;
                }

                reader.Dispose();
                str.Dispose();

                string ex = System.IO.Path.GetFileName(url);
                if (!string.IsNullOrEmpty(extention))
                    ex = extention;
                string p = path + ex;
                FileStream fstr = new FileStream(p, FileMode.OpenOrCreate, FileAccess.Write);
                fstr.Write(mbyte, 0, startmbyte);
                fstr.Flush();
                fstr.Close();
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("DownFile", url + "_" + ex.Message);

            }
        }

        public MemoryStream GetMemoryStreamByDownFile(string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream str = client.OpenRead(url);

                byte[] mbyte = new byte[1000000];
                int allmybyte = (int)mbyte.Length;
                int startmbyte = 0;

                while (allmybyte > 0)
                {
                    int m = str.Read(mbyte, startmbyte, allmybyte);
                    if (m == 0)
                        break;

                    startmbyte += m;
                    allmybyte -= m;
                }

                MemoryStream ms = new MemoryStream(mbyte);

                str.Dispose();
                client.Dispose();
                return ms;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("GetMemoryStreamByDownFile", url + "_" + ex.Message);
                return null;
            }
        }

        public FileStream GetFileStreamByLocalFile(string fileUrl)
        {
            try
            {
                FileStream ms = new FileStream(fileUrl, FileMode.Open);
                return ms;
            }
            catch (Exception ex)
            {
                ApplicationContext.Log.Error("GetFileStreamByLocalFile", fileUrl + "_" + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }
        #endregion

        /// <summary>
        /// 浏览上传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string UploadFile(string url, HttpPostedFileBase file, string charset = "utf-8")
        {
            return UploadFile(url, file.FileName, file.InputStream, charset);
        }

        /// <summary>
        /// 文件流上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string UploadFile(string url, FileStream file, string charset = "utf-8")
        {
            var filename = Path.GetFileName(file.Name);
            return UploadFile(url, filename, file, charset);
        }

        public string UploadFile(string url, string fileName, MemoryStream file)
        {
            return UploadFile(url, fileName, file, "utf-8");
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        private string UploadFile(string url, string filename, Stream file, string charset = "utf-8")
        {
            var encoding = Encoding.GetEncoding(charset);
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.KeepAlive = true;

            //随机数
            string boundary = DateTime.Now.Ticks.ToString("X");
            request.ContentType = "multipart/form-data;charset=" + charset + ";boundary=" + boundary;
            //分界线
            var startLine = encoding.GetBytes("\r\n--" + boundary + "\r\n");
            var endLine = encoding.GetBytes("\r\n--" + boundary + "--\r\n");

            //请求头格式
            var headerString = string.Format("Content-Disposition:form-data;name=\"filedata\";filename=\"{0}\"\r\nContent-Type:{1}\r\n\r\n", filename, "");
            try
            {
                using (var reqStream = request.GetRequestStream())
                {
                    //请求头缓冲
                    var headerBuffer = encoding.GetBytes(headerString);
                    //文件缓冲
                    var fileBuffer = new byte[file.Length];
                    file.Read(fileBuffer, 0, (int)file.Length);

                    //线
                    reqStream.Write(startLine, 0, startLine.Length);
                    //头
                    reqStream.Write(headerBuffer, 0, headerBuffer.Length);
                    //文件
                    reqStream.Write(fileBuffer, 0, fileBuffer.Length);
                    //线
                    reqStream.Write(endLine, 0, endLine.Length);
                }
                //返回
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var resStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(resStream, encoding))
                        {
                            var result = reader.ReadToEnd();
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string HttpPostFile(string url,
                                  System.Web.HttpPostedFileBase postedFile,
                                  Dictionary<string, object> parameters = null,
                                  CookieContainer cookieContainer = null)
        {
            string output = "";
            //1>创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //2>Cookie容器
            request.CookieContainer = cookieContainer;
            request.Method = "POST";
            request.Timeout = 20000;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.KeepAlive = true;

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");//分界线
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            request.ContentType = "multipart/form-data; boundary=" + boundary; ;//内容类型

            //3>表单数据模板
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            //4>读取流
            byte[] buffer = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(buffer, 0, buffer.Length);

            //5>写入请求流数据
            string strHeader = "Content-Disposition:application/x-www-form-urlencoded; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
            strHeader = string.Format(strHeader,
                                     "filedata",
                                     postedFile.FileName,
                                     postedFile.ContentType);
            //6>HTTP请求头
            byte[] byteHeader = System.Text.ASCIIEncoding.ASCII.GetBytes(strHeader);
            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    //写入请求流
                    if (null != parameters)
                    {
                        foreach (KeyValuePair<string, object> item in parameters)
                        {
                            stream.Write(boundaryBytes, 0, boundaryBytes.Length);//写入分界线
                            byte[] formBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(formdataTemplate, item.Key, item.Value));
                            stream.Write(formBytes, 0, formBytes.Length);
                        }
                    }
                    //6.0>分界线============================================注意：缺少次步骤，可能导致远程服务器无法获取Request.Files集合
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    //6.1>请求头
                    stream.Write(byteHeader, 0, byteHeader.Length);
                    //6.2>把文件流写入请求流
                    stream.Write(buffer, 0, buffer.Length);
                    //6.3>写入分隔流
                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    stream.Write(trailer, 0, trailer.Length);
                    //6.4>关闭流
                    stream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    output = reader.ReadToEnd();
                }
                response.Close();
                return output;
            }
            catch (Exception ex)
            {
                throw new Exception("上传文件时远程服务器发生异常！", ex);
            }
        }
    }

    public class HttpClientHelper
    {
        public static HttpClientHelper Single { get; } = new HttpClientHelper();

        HttpClient httpClient = null;
        public HttpClientHelper()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            // if (!string.IsNullOrEmpty(config.AccessToken))
            //  httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
            //   httpClient.BaseAddress = new Uri(ApplicationContext.AppSetting.API_DOMAIN);
            httpClient.Timeout = new TimeSpan(TimeSpan.TicksPerMinute);
        }

        public static async Task<string> PostAsync(string url, params object[] content)
        {
            var hc = new HttpClientHelper();
            var result = await hc.httpClient.PostAsync(url, new FormUrlEncodedContent(CreateParam(content)));
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        public static async Task<T> PostAsync<T>(string url, params object[] content)
        {
            var res_str = await PostAsync(url, content);
            if (!string.IsNullOrEmpty(res_str))
            {
                return JsonHelper.Single.JsonToObject<T>(res_str);
            }
            else
            {
                return default(T);
            }
        }

        public T Post<T>(string url, params object[] content)
        {
            var res_str = Post(url, content);
            if (!string.IsNullOrEmpty(res_str))
            {
                return JsonHelper.Single.JsonToObject<T>(res_str);
            }
            else
            {
                return default(T);
            }
        }

        public static string Post(string url, params object[] content)
        {
            var hc = new HttpClientHelper();
            var result = hc.httpClient.PostAsync(url, new FormUrlEncodedContent(CreateParam(content)));
            result.Result.EnsureSuccessStatusCode();
            return result.Result.Content.ReadAsStringAsync().Result;
        }


        private static Dictionary<string, string> CreateParam(object[] content)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var item in content)
            {
                foreach (var p in item.GetType().GetProperties())
                {
                    keyValuePairs.Add(p.Name, p.GetValue(item)?.ToString());
                }
            }
            return keyValuePairs;
        }
    }
}
