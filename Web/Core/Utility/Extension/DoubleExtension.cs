using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Extension
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this double d)
        {
            var time = DateTime.MinValue;
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }
    }
}
