using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility.Components
{
    /// <summary>
    /// 安全类型转换帮助类
    /// </summary>
    public class ConvertHelper
    {
        private ConvertHelper()
        {

        }
        private static ConvertHelper _single = new ConvertHelper();

        public static ConvertHelper Single
        {
            get { return _single; }
        }

        #region 基本类型转换

        public Guid ToGuid(object obj)
        {
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    return new Guid(obj.ToString());
                }
                catch
                {
                    return Guid.Empty;
                }
            }

            return Guid.Empty;
        }


        public TimeSpan ToTimeSpan(object obj)
        {
            return ToTimeSpan(obj, TimeSpan.Zero);
        }

        public TimeSpan ToTimeSpan(object obj, TimeSpan defaultValue)
        {
            if (obj != null)
                return ToTimeSpan(obj.ToString(), defaultValue);

            return defaultValue;
        }


        public TimeSpan ToTimeSpan(string s, TimeSpan defaultValue)
        {
            TimeSpan result;
            bool success = TimeSpan.TryParse(s, out result);

            return success ? result : defaultValue;
        }


        public TimeSpan ToTimeSpan(string s)
        {
            return ToTimeSpan(s, TimeSpan.Zero);
        }


        public string ToString(object obj)
        {
            if (obj != null) return obj.ToString();

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ToString(string s)
        {
            return ToString(s, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public string ToString(string s, string defaultString)
        {
            if (s == null) return defaultString;

            return s.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public string ToString(object s, string defaultString)
        {
            if (s == null) return defaultString;

            return s.ToString();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double ToDouble(string s, double defaultValue)
        {
            double result;
            bool success = double.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public double ToDouble(string s)
        {
            return ToDouble(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public double ToDouble(object obj)
        {
            return ToDouble(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double ToDouble(object obj, double defaultValue)
        {
            if (obj != null)
                return ToDouble(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float ToSingle(string s, float defaultValue)
        {
            float result;
            bool success = float.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public float ToSingle(string s)
        {
            return ToSingle(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public float ToSingle(object obj)
        {
            return ToSingle(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float ToSingle(object obj, float defaultValue)
        {
            if (obj != null)
                return ToSingle(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public decimal ToDecimal(string s, decimal defaultValue)
        {
            decimal result;
            bool success = decimal.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool IsDecimal(string s)
        {
            decimal result;
            bool success = decimal.TryParse(s, out result);
            return success;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public decimal ToDecimal(string s)
        {
            return ToDecimal(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public decimal ToDecimal(object obj, decimal defaultValue)
        {
            if (obj != null)
                return ToDecimal(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float ToFloat(string s, float defaultValue)
        {
            float result;
            bool success = float.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public float ToFloat(string s)
        {
            return ToFloat(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public float ToFloat(object obj)
        {
            return ToFloat(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float ToFloat(object obj, float defaultValue)
        {
            if (obj != null)
                return ToFloat(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool ToBoolean(string s, bool defaultValue)
        {
            //修复1被转换为false的BUG
            if (s == "1")
                return true;


            bool result;
            bool success = bool.TryParse(s, out result);

            return success ? result : defaultValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ToBoolean(string s)
        {
            return ToBoolean(s, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ToBoolean(object obj)
        {
            return ToBoolean(obj, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool ToBoolean(object obj, bool defaultValue)
        {
            if (obj != null)
                return ToBoolean(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char ToChar(string s, char defaultValue)
        {
            char result;
            bool success = char.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public char ToChar(string s)
        {
            return ToChar(s, '\0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public char ToChar(object obj)
        {
            return ToChar(obj, '\0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char ToChar(object obj, char defaultValue)
        {
            if (obj != null)
                return ToChar(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public byte ToByte(string s, byte defaultValue)
        {
            byte result;
            bool success = byte.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public byte ToByte(string s)
        {
            return ToByte(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte ToByte(object obj)
        {
            return ToByte(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public byte ToByte(object obj, byte defaultValue)
        {
            if (obj != null)
                return ToByte(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public sbyte ToSByte(string s, sbyte defaultValue)
        {
            sbyte result;
            bool success = sbyte.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public sbyte ToSByte(string s)
        {
            return ToSByte(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sbyte ToSByte(object obj)
        {
            return ToSByte(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public sbyte ToSByte(object obj, sbyte defaultValue)
        {
            if (obj != null)
                return ToSByte(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short ToInt16(string s, short defaultValue)
        {
            short result;
            bool success = short.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public short ToInt16(string s)
        {
            return ToInt16(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public short ToInt16(object obj)
        {
            return ToInt16(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short ToInt16(object obj, short defaultValue)
        {
            if (obj != null)
                return ToInt16(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ushort ToUInt16(string s, ushort defaultValue)
        {
            ushort result;
            bool success = ushort.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ushort ToUInt16(string s)
        {
            return ToUInt16(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ushort ToUInt16(object obj)
        {
            return ToUInt16(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ushort ToUInt16(object obj, ushort defaultValue)
        {
            if (obj != null)
                return ToUInt16(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int ToInt32(string s, int defaultValue)
        {
            int result;
            bool success = int.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int ToInt32(string s)
        {
            return ToInt32(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int ToInt32(object obj, int defaultValue)
        {
            if (obj != null)
                return ToInt32(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public uint ToUInt32(string s, uint defaultValue)
        {
            uint result;
            bool success = uint.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public uint ToUInt32(string s)
        {
            return ToUInt32(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public uint ToUInt32(object obj)
        {
            return ToUInt32(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public uint ToUInt32(object obj, uint defaultValue)
        {
            if (obj != null)
                return ToUInt32(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long ToInt64(string s, long defaultValue)
        {
            long result;
            bool success = long.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public long ToInt64(string s)
        {
            return ToInt64(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long ToInt64(object obj)
        {
            return ToInt64(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long ToInt64(object obj, long defaultValue)
        {
            if (obj != null)
                return ToInt64(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ulong ToUInt64(string s, ulong defaultValue)
        {
            ulong result;
            bool success = ulong.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ulong ToUInt64(string s)
        {
            return ToUInt64(s, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ulong ToUInt64(object obj)
        {
            return ToUInt64(obj, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ulong ToUInt64(object obj, ulong defaultValue)
        {
            if (obj != null)
                return ToUInt64(obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DateTime ToDateTime(string s, DateTime defaultValue)
        {
            DateTime result;
            bool success = DateTime.TryParse(s, out result);

            return success ? result : defaultValue;
        }

        public bool IsDateTime(string s)
        {
            DateTime result;
            bool success = DateTime.TryParse(s, out result);
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public DateTime ToDateTime(string s)
        {
            return ToDateTime(s, DateTime.MinValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, DateTime.MinValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            if (obj != null)
                return ToDateTime(obj.ToString(), defaultValue);

            return defaultValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object ToEnum(Type enumType, string text, object defaultValue)
        {
            if (Enum.IsDefined(enumType, text))
            {
                return Enum.Parse(enumType, text, false);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object ToEnum(Type enumType, object obj, object defaultValue)
        {
            if (obj != null)
                return ToEnum(enumType, obj.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object ToEnum(Type enumType, int index)
        {
            return Enum.ToObject(enumType, index);
        }

        #endregion

        #region 其他转换
        /// <summary>
        /// 判读是否DBNull
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsDBNull(object obj)
        {
            return Convert.IsDBNull(obj);
        }

        ///// <summary>  
        ///// 将c# DateTime时间格式转换为Unix时间戳格式  
        ///// </summary>  
        ///// <param name="time">时间</param>  
        ///// <returns>long</returns>  
        //public long DateTimeToInt(System.DateTime time)
        //{
        //    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        //    return (long)(time - startTime).TotalSeconds;
        //}
        ///// <summary>        
        ///// 时间戳转为C#格式时间        
        ///// </summary>        
        ///// <param name=”timeStamp”></param>        
        ///// <returns></returns>        
        //public DateTime StringToDateTime(string unixTimeStamp)
        //{
        //    DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //    long lTime = long.Parse(unixTimeStamp + "0000000");
        //    TimeSpan toNow = new TimeSpan(lTime); return dateTimeStart.Add(toNow);
        //}

        #endregion
    }
}
