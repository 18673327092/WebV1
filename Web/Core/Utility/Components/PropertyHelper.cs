using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Routing;

namespace Utility.Components
{
    /// <summary>
    /// 公共属性帮助器
    /// </summary>
    public class PropertyHelper
    {
        private PropertyHelper()
        {
        }

        private static PropertyHelper _single = new PropertyHelper();

        public static PropertyHelper Single
        {
            get { return _single; }
        }

        /// <summary>
        /// 属性缓存
        /// </summary>
        private static ConcurrentDictionary<Type, PropertyInfo[]> _reflectionCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// 获取所有公共属性
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public PropertyInfo[] GetProperties(object instance)
        {
            var dictionary = new RouteValueDictionary();
            PropertyInfo[] propertyArray;
            var type = instance.GetType();
            //读取缓存
            if (!_reflectionCache.TryGetValue(type, out propertyArray))
            {
                //公开属性
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var query = from prop in properties
                            where prop.GetIndexParameters().Length == 0
                            && prop.GetGetMethod() != null
                            select prop;
                var list = new List<PropertyInfo>();
                foreach (var item in query)
                {
                    list.Add(item);
                }
                propertyArray = list.ToArray();
                _reflectionCache.TryAdd(type, propertyArray);
            }

            return propertyArray;
        }
    }
}
