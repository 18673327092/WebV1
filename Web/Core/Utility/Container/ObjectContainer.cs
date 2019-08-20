using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Container
{
    /// <summary>
    /// 容器
    /// </summary>
    public class ObjectContainer
    {
        static readonly object _lock = new object();
        static ObjectContainer current;
        static IinjectContainer container;
        public static ObjectContainer Current
        {
            get
            {
                if (current == null)
                {
                    lock (_lock)
                    {
                        if (current == null)
                        {
                            ApplicationStart(container);
                        }
                    }
                }
                return current;
            }

        }
        public static void ApplicationStart(IinjectContainer inversion)
        {
            current = new ObjectContainer(inversion);
        }
        public ObjectContainer(IinjectContainer inversion)
        {
            container = inversion;
        }

        public void RegisterType<T>()
        {
            container.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
