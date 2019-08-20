using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Extension
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// 将DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToUinxTime(this DateTime time)
        {
            long intResult = 0;
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            intResult = long.Parse((time - startTime).TotalSeconds.ToString().Split('.')[0]);
            return intResult;
        }
        /// <summary>
        /// DateTime.TryParse(s, out dt)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime? s)
        {
            if (!s.HasValue) return DateTime.Now;
            return s.Value;
        }

        public static string ToWeek(this DateTime date)
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(date.DayOfWeek)];
            return week;
        }

        public static int ToTimeSpanDay(this DateTime dateA, DateTime dateB)
        {
            TimeSpan ts = dateA - dateB;
            return ts.Days;
        }

        /// <summary>
        /// 取dateA减去dateB的小时数
        /// </summary>
        /// <param name="dateA"></param>
        /// <param name="dateB"></param>
        /// <returns></returns>
        public static int ToTimeSpanHour(this DateTime dateA, DateTime dateB)
        {
            TimeSpan ts = dateA - dateB;
            return ts.Hours;
        }

        /// <summary>
        /// 取dateA减去dateB的分钟数
        /// </summary>
        /// <param name="dateA"></param>
        /// <param name="dateB"></param>
        /// <returns></returns>
        public static int ToTimeSpanMinutes(this DateTime dateA, DateTime dateB)
        {
            TimeSpan ts = dateA - dateB;
            return ts.Minutes;
        }

        /// <summary>
        /// 取dateA减去dateB的分钟数
        /// </summary>
        /// <param name="dateA"></param>
        /// <param name="dateB"></param>
        /// <returns></returns>
        public static int ToTimeSpanAllMinutes(this DateTime dateA, DateTime dateB)
        {
            TimeSpan ts = dateA - dateB;
            return ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
        }



        public static string ToFuzzy(this DateTime? time, string format = "yyyy-MM-dd HH:mm")
        {
            DateTime mytime = new DateTime();
            if (time != null)
            {
                mytime = time.Value;
            }
            return mytime.ToFuzzy(format);
        }

        public static string ToFuzzy(this DateTime time, string format = "yyyy-MM-dd HH:mm")
        {
            DateTime now = DateTime.Now;
            if (now > time)
            {
                double second = Math.Floor((now - time).TotalSeconds);
                if (second < 60)
                {
                    return "刚刚";
                }
                else
                {
                    double minute = Math.Floor(second / 60);
                    if (minute < 60)
                    {
                        return minute + "分钟前";
                    }
                    else
                    {
                        double hour = Math.Floor(minute / 60);
                        if (hour < 24)
                        {
                            return hour + "小时前";
                        }
                        else if (Math.Floor(hour / 24) == 1.0)
                        {
                            return "昨天 " + time.ToShortTimeString();
                        }
                    }
                }
            }
            return time.ToString(format);
        }

        /// <summary>
        /// 多久前
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AfterTime(this DateTime input)
        {
            var currentTime = DateTime.Now;
            var diff = currentTime - input;
            var day = diff.Days;
            if (day > 0)
            {
                return day + "天前";
            }
            var hour = diff.Hours;
            if (hour > 0)
            {
                return hour + "小时前";
            }
            var minute = diff.Minutes;
            if (minute > 0)
            {
                return minute + "分钟前";
            }
            var second = diff.Seconds;
            if (second > 10)
            {
                return second + "秒前";
            }
            return "刚刚";
        }

        /// <summary>
        /// 将【秒】转化成文字显示
        /// </summary>
        /// <param name="secondPara"></param>
        /// <returns></returns>
        public static string GetStrTimeShow(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var secondPara = (long)(time - startTime).TotalSeconds;
            int iDay = 0, iHour = 0, iMinute = 0, iSecond = 0;
            string sDay = "", sHour = "", sMinute = "", sSecond = "", sTime = "";
            iDay = (int)(secondPara / 24 / 3600);
            if (iDay > 0)
            {
                sDay = iDay + "天";
            }
            iHour = (int)((secondPara / 3600) % 24);
            if (iHour > 0)
            {
                sHour = iHour + "时";
            }
            iMinute = (int)((secondPara / 60) % 60);
            if (iMinute > 0)
            {
                sMinute = iMinute + "分";
            }
            iSecond = (int)(secondPara % 60);
            if (iSecond >= 0)
            {
                sSecond = iSecond + "秒";
            }
            if ((sDay == "") && (sHour == ""))
            {
                sTime = sMinute + sSecond;
            }
            else
            {
                sTime = sDay + sHour + sMinute + sSecond;
            }
            return sTime;
        }
    }
}
