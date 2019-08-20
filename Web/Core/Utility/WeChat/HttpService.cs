using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Utility.WeChat
{
    /// <summary>
    /// http连接基础类，负责底层的http通信
    /// </summary>
    public class HttpService
    {

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        public static string Post(string xml, string url, bool isUseCert, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(Wechat_Config.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    X509Certificate2 cert = new X509Certificate2(path + Wechat_Config.SSLCERT_PATH, Wechat_Config.SSLCERT_PASSWORD);
                    request.ClientCertificates.Add(cert);
                    Log.Debug("WxPayApi", "PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new WxPayException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if(request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        public static string Post(string xml, string url, bool isUseCert, int timeout, string CertPath, string CertPassword)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(Wechat_Config.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    X509Certificate2 cert = new X509Certificate2(CertPath, CertPassword, X509KeyStorageFlags.MachineKeySet);
                    request.ClientCertificates.Add(cert);
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">参数</param>
        /// <returns></returns>
        public string Post(string url,string postData)
        {
            //定义request并设置request的路径
            WebRequest request = WebRequest.Create(url);
            request.Method = "post";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //定义response为前面的request响应
            WebResponse response = request.GetResponse();

            //获取相应的状态代码
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //定义response字符流
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();//读取所有
            return responseFromServer;
            // GetOpenidAndAccessToken();
        }

        /// <summary>
        /// WebClient-POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string WebClient_Post(string url, string postData, Dictionary<string, string> headers = null)
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
                return "";
            }
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(Wechat_Config.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Log.Error("HttpService","Thread - caught ThreadAbortException - resetting.");
                Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new WxPayException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
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
    }
}