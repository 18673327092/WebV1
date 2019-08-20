using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Utility.Container;

namespace Web
{
    /// <summary>
    /// Autofac容器
    /// </summary>
    public class AutofacContainer : IinjectContainer
    {
        ContainerBuilder builder;
        IContainer container;
        public AutofacContainer()
        {
            builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray().Where(e => e.FullName.Contains("Service")).ToArray();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        public void RegisterType<T>()
        {
            builder.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
