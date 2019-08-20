using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility.Components
{
    public class LogHelper
    {
        //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        private LogHelper()
        {
            //ILog log = LogManager.GetLogger("LogHelper");
            //log.Error(sb.ToString());
        }

        private static readonly LogHelper _single = new LogHelper();
        /// <summary>
        /// 饿汉模式 单例
        /// </summary>
        public static LogHelper Single
        {
            get { return _single; }
        }

        #region file

        private string SavePath { get; set; }

        public void Init(string path)
        {
            SavePath = path;
        }

        public void Init(object lOG_PATH)
        {
            throw new NotImplementedException();
        }

        #region 写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void WriteLog(string name, string message)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                //      从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                //      因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                LogWriteLock.EnterWriteLock();
                var currentDate = DateTime.Now;
                var filePath = SavePath + "/" + currentDate.Year + "/" + currentDate.Month + "/";
                var fileName = currentDate.Day + "_" + name + ".txt";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                StreamWriter sw;
                if (!File.Exists(filePath + fileName))
                {
                    sw = File.CreateText(filePath + fileName);
                }
                else
                {
                    sw = File.AppendText(filePath + fileName);
                }

                sw.Write(message);
                sw.Close();
            }
            finally
            {
                //退出写入模式，释放资源占用
                //注意：一次请求对应一次释放
                //      若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
                //      若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
                LogWriteLock.ExitWriteLock();
            }
        }
        #endregion

        #endregion file

        public void Error(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("记录时间：" + DateTime.Now);
            sb.AppendLine("异常信息：" + ex.Message);
            sb.AppendLine("异常描述：" + ex.ToString());
            sb.AppendLine("--------------------------------------------------");
            WriteLog(LogLevel.Error.ToString(), sb.ToString());
        }

        public void Error(string method, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("异常方法：" + method);
            sb.AppendLine("记录时间：" + DateTime.Now);
            sb.AppendLine("异常信息：" + ex.Message);
            sb.AppendLine("异常描述：" + ex.ToString());
            sb.AppendLine("--------------------------------------------------");
            WriteLog(LogLevel.Error.ToString(), sb.ToString());
        }

        public void Error(string method, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("异常方法：" + method + "(" + DateTime.Now + ")");
            sb.AppendLine("异常信息：" + message);
            sb.AppendLine("--------------------------------------------------");
            WriteLog(LogLevel.Error.ToString(), sb.ToString());
        }

        public void Debug(string method, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("调试方法：" + method + "(" + DateTime.Now + ")");
            sb.AppendLine("调试信息：" + message);
            sb.AppendLine("--------------------------------------------------");
            WriteLog(LogLevel.Debug.ToString(), sb.ToString());
        }

        public void Info(string method, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("方法名称：" + method + "(" + DateTime.Now + ")");
            sb.AppendLine("记录信息：" + message);
            sb.AppendLine("--------------------------------------------------");
            WriteLog(LogLevel.Info.ToString(), sb.ToString());
        }

        /// <summary>
        /// 一般错误
        /// </summary>
        /// <param name="message">消息</param>
        public static void Error(object message)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Error(message);
            }
            catch
            {

            }

        }

        /// <summary>
        /// 一般错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public void Error(object message, Exception exception)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Error(message, exception);
            }
            catch
            {

            }
        }


        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        public void Info(object message)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Info(message);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public void Info(object message, Exception ex)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Info(message, ex);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">消息</param>
        public void Warn(object message)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Warn(message);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public void Warn(object message, Exception ex)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Warn(message, ex);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">消息</param>
        public void Debug(object message)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Debug(message);
            }
            catch
            { }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public void Debug(object message, Exception ex)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(GetCurrentMethodFullName());
                log.Debug(message, ex);
            }
            catch
            { }
        }

        static string GetCurrentMethodFullName()
        {
            try
            {
                int depth = 2;
                StackTrace st = new StackTrace();
                int maxFrames = st.GetFrames().Length;
                StackFrame sf;
                string methodName, className;
                Type classType;
                do
                {
                    sf = st.GetFrame(depth++);
                    classType = sf.GetMethod().DeclaringType;
                    className = classType.ToString();
                } while (className.EndsWith("Exception") && depth < maxFrames);
                methodName = sf.GetMethod().Name;
                return className + "." + methodName;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("Core.Log").Error(e.Message, e);
                return "获取方法名失败";
            }
        }
    }

    public enum LogLevel
    {
        Error,
        Debug,
        Info
    }
}
