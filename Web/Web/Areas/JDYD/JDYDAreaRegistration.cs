using System.Web.Mvc;

namespace Web.Areas.JDYD
{
    public class JDYDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JDYD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JDYD_default",
                "JDYD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}