using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Utility;
using Utility.Container;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ObjectContainer.ApplicationStart(new AutofacContainer());
            /// 初始化Log日志文件位置
            ApplicationContext.Log.Init(Server.MapPath("~/Logs"));

            //方便测试，暂时设置为一分钟生成
            //var TaskCycle = ConvertHelper.Single.ToInt32(ConfigurationManager.AppSettings["TaskCycle"]);
            //ScheduleTaskManager.Instance.AddTask(new ScheduleTask("TaskService", "BZM.Service.ScheduleTask.TaskService", null, 1000, TaskCycle * 1000));
        }
    }
}
