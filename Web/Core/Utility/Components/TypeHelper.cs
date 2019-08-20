using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Utility.Components
{
    /// <summary>
    /// 类型转换器
    /// </summary>
    public class TypeHelper
    {
        private TypeHelper()
        {
        }

        private static TypeHelper _single = new TypeHelper();

        public static TypeHelper Single
        {
            get { return _single; }
        }

         //<summary>
         //对象转集合
         //</summary>
         //<param name="value"></param>
         //<returns></returns>
        public RouteValueDictionary ObjectToDictionary(object value)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (value != null)
            {
                foreach (var item in PropertyHelper.Single.GetProperties(value))
                {
                    dictionary.Add(item.Name, item.GetValue(value));
                }
            }
            return dictionary;
        }
    }
}
