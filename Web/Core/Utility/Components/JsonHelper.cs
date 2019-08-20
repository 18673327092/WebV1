using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.Components
{
    public class JsonHelper
    {

        /// <summary>
        /// Json帮助器
        /// </summary>
        private JsonHelper()
        {
        }

        private static JsonHelper _single = new JsonHelper();

        public static JsonHelper Single
        {
            get { return _single; }
        }

        #region DataTable 与 Json 互转

        public static string ToListResultJson(DataTable dt, DataSet sum_ds, int PagesCount, int PageSize, int Total)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"Data\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));


                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            Json.Append(",\"SumData\":[");
            if (sum_ds != null && sum_ds.Tables != null && sum_ds.Tables.Count > 0)
            {
                var sumdt = sum_ds.Tables[0];
                if (sumdt != null && sumdt.Rows.Count > 0)
                {
                    for (int i = 0; i < sumdt.Rows.Count; i++)
                    {
                        Json.Append("{");
                        for (int j = 0; j < sumdt.Columns.Count; j++)
                        {

                            Type type = sumdt.Rows[i][j].GetType();
                            Json.Append("\"" + sumdt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(sumdt.Rows[i][j].ToString(), type));


                            if (j < sumdt.Columns.Count - 1)
                            {
                                Json.Append(",");
                            }
                        }
                        Json.Append("}");
                        if (i < sumdt.Rows.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                }
            }
            Json.Append("],\"PagesCount\": " + PagesCount + ",\"PageSize\": " + PageSize + ",\"Total\": " + Total + "}");
            return Json.ToString();
        }

        public static string ToListResultJson(DataTable dt, int PagesCount, int PageSize, int Total)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"Data\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));


                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("],\"PagesCount\": " + PagesCount + ",\"PageSize\": " + PageSize + ",\"Total\": " + Total + "}");
            return Json.ToString();
        }

        /// <summary>    
        /// Datatable转换为Json    
        /// </summary>    
        /// <param name="table">Datatable对象</param>    
        /// <returns>Json字符串</returns>    
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>   
        /// DataTable转成Json    
        /// </summary>   
        /// <param name="jsonName"></param>   
        /// <param name="dt"></param>   
        /// <returns></returns>   
        public static string DataTableToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>    
        /// DataSet转换为Json    
        /// </summary>    
        /// <param name="dataSet">DataSet对象</param>    
        /// <returns>Json字符串</returns>    
        public static string DataSetToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + DataTableToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }

        /// <summary>   
        /// 过滤特殊字符   
        /// </summary>   
        /// <param name="s"></param>   
        /// <returns></returns>   
        public static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>   
        /// 格式化字符型、日期型、布尔型   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="type"></param>   
        /// <returns></returns>   
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = Convert.ToDateTime(str).ToString("yyyy-MM-dd HH:mm:ss");
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        #endregion

        #region Object 与 Json 互转
        public string ObjectToJson<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(ms, t);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    string json = reader.ReadToEnd();
                    string p = @"\\/Date\((\d+)\+\d+\)\\/";
                    MatchEvaluator evaluator = new MatchEvaluator(ConvertJsonDataToDataString);
                    Regex reg = new Regex(p);
                    json = reg.Replace(json, evaluator);
                    return json;
                }
            }

        }

        public T JsonToObject<T>(string json)
        {
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator evaluator = new MatchEvaluator(ConvertDateStringToJsonDate);    //对时间进行处理
            Regex reg = new Regex(p);
            json = reg.Replace(json, evaluator);
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                T data = (T)serializer.ReadObject(ms);
                return data;
            }
        }

        /// <summary>
        /// 将Json时间转换为时间字符
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ConvertJsonDataToDataString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符转换成JSON时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        #endregion

    }
}
