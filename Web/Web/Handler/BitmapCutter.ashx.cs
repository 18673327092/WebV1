using BitmapCutter.Core.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Web.Handler
{
    /// <summary>
    /// BitmapCutter 的摘要说明
    /// </summary>
    public class BitmapCutter : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string methodName = context.Request["action"];
            Callback ops = new Callback();
            MethodInfo method = typeof(Callback).GetMethod(methodName);
            string msg = method.Invoke(ops, new object[] { Path.Combine(context.Server.MapPath("~/"), context.Request["src"]) }).ToString();
            context.Response.Write(msg);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}