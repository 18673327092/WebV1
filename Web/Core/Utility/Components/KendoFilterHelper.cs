using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.Components
{
    public class KendoFilterHelper
    {
        private KendoFilterHelper()
        {

        }

        private static KendoFilterHelper _single = new KendoFilterHelper();

        public static KendoFilterHelper Single
        {
            get { return _single; }
        }

        public void SetPagination(NameValueCollection forms, Pagination page)
        {
            if (string.IsNullOrEmpty(page.WhereSql))
            {
                if (!string.IsNullOrEmpty(forms["sort[0][field]"]))
                {
                    page.SortField = forms["sort[0][field]"].Replace("$", ".");
                    page.SortType = forms["sort[0][dir]"];
                }
                string logic = "and";
                List<string> filterlist = new List<string>();
                List<string> filterchild_list = new List<string>();
                StringBuilder WhereSql = new StringBuilder();
                for (int i = 0; i < forms.Count; i++)
                {
                    string date;
                    string _field;
                    string _operator;
                    string _value;
                    if (!string.IsNullOrEmpty(forms["filter[filters][" + i + "][logic]"]))
                    {
                        filterchild_list = new List<string>();
                        logic = forms["filter[filters][" + i + "][logic]"];
                        _field = forms["filter[filters][" + i + "][filters][0][field]"];
                        _operator = forms["filter[filters][" + i + "][filters][0][operator]"];
                        _value = forms["filter[filters][" + i + "][filters][0][value]"];
                        _field = _field.Replace("$", ".");

                        if (IsDate(_value, out date)) _value = date;
                        filterchild_list.Add(FilterToSql(_field, _operator, _value));

                        _field = forms["filter[filters][" + i + "][filters][1][field]"];
                        _operator = forms["filter[filters][" + i + "][filters][1][operator]"];
                        _value = forms["filter[filters][" + i + "][filters][1][value]"];
                        _field = _field.Replace("$", ".");

                        if (IsDate(_value, out date)) _value = date;
                        filterchild_list.Add(FilterToSql(_field, _operator, _value));
                        filterlist.Add("(" + string.Join(" " + logic + " ", filterchild_list) + ")");
                        filterchild_list = new List<string>();
                    }
                    else
                    {
                        logic = " and ";
                        _field = forms["filter[filters][" + i + "][field]"];
                        _operator = forms["filter[filters][" + i + "][operator]"];
                        _value = forms["filter[filters][" + i + "][value]"];
                        if (!string.IsNullOrEmpty(_field) && !string.IsNullOrEmpty(_operator) && !string.IsNullOrEmpty(_value))
                        {
                            _field = _field.Replace("$", ".");
                            if (IsDate(_value, out date)) _value = date;
                            filterlist.Add(FilterToSql(_field, _operator, _value));
                        }
                    }
                }
                page.WhereSql = string.Join(" " + forms["filter[logic]"] + " ", filterlist);
                var _WhereSql = GetFieldSearchFieldWhereSql(page.Filter);
                if (!string.IsNullOrEmpty(page.WhereSql) && !string.IsNullOrEmpty(_WhereSql))
                {
                    page.WhereSql += " and " + _WhereSql;
                }
                else
                {
                    page.WhereSql += _WhereSql;
                }
            }
        }

        public string FilterToSql(string field, string opera, string value)
        {
            string _sql = "";
            switch (opera)
            {
                case "eq":
                    _sql = "{0}='{1}'";
                    if (value.IndexOf("00:00:00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace("00:00:00", "");
                            _sql = "{0} BETWEEN '{1}00:00:00' AND '{1}23:59:59'";
                        }
                    }
                    else if (value.IndexOf(":00:00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace(":00:00", "");
                            _sql = "{0} BETWEEN '{1}:00:00' AND '{1}:59:59'";
                        }
                    }
                    else if (value.IndexOf(":00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace(":00", "");
                            _sql = "{0} BETWEEN '{1}:00' AND '{1}:59'";
                        }
                    }
                    break;
                case "neq":
                    _sql = "{0}!='{1}'";
                    //日期
                    if (value.IndexOf("00:00:00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace("00:00:00", "");
                            _sql = "{0} NOT BETWEEN '{1}00:00:00' AND '{1}23:59:59'";
                        }
                    }
                    else if (value.IndexOf(":00:00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace(":00:00", "");
                            _sql = "{0} NOT BETWEEN '{1}:00:00' AND '{1}:59:59'";
                        }
                    }
                    else if (value.IndexOf(":00") != -1)
                    {
                        DateTime result;
                        bool success = DateTime.TryParse(value, out result);
                        if (success)
                        {
                            value = value.Replace(":00", "");
                            _sql = "{0} NOT BETWEEN '{1}:00' AND '{1}:59'";
                        }
                    }
                    break;
                case "gt":
                    _sql = "{0}>'{1}'";
                    break;
                case "gte":
                    _sql = "{0}>='{1}'";
                    break;
                case "lte":
                    _sql = "{0}<='{1}'";
                    break;
                case "lt":
                    _sql = "{0}<'{1}'";
                    break;
                case "startswith":
                    _sql = "{0} like'{1}%'";
                    break;
                case "contains":
                    _sql = "{0} like '%{1}%'";
                    break;
                case "doesnotcontain":
                    _sql = "{0} not like '%{1}%'";
                    break;
                case "endswith":
                    _sql = "{0} like '%{1}'";
                    break;
            }
            return string.Format(_sql, field, value);
        }

        public bool IsDate(string value, out string date)
        {
            date = string.Empty;
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var reg = @"(\s{1}(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec){1}\s{1}(\d{2})\s+(\d{4})\s+(\d{2}:\d{2}:\d{2}))";
            if (value.IndexOf("中国标准时间") == -1) return false;
            MatchCollection mc = Regex.Matches(value, reg, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Singleline);
            var year = "";
            var month = "";
            var day = "";
            var time = "";
            foreach (Match item in mc)
            {
                if (item.Groups.Count > 5)
                {
                    month = item.Groups[2].Value;
                    day = item.Groups[3].Value;
                    year = item.Groups[4].Value;
                    time = item.Groups[5].Value;
                }
            }
            switch (month)
            {
                case "Jan":
                    month = "01";
                    break;
                case "Feb":
                    month = "02";
                    break;
                case "Mar":
                    month = "03";
                    break;
                case "Apr":
                    month = "04";
                    break;
                case "May":
                    month = "05";
                    break;
                case "Jun":
                    month = "06";
                    break;
                case "Jul":
                    month = "07";
                    break;
                case "Aug":
                    month = "08";
                    break;
                case "Sep":
                    month = "09";
                    break;
                case "Oct":
                    month = "10";
                    break;
                case "Nov":
                    month = "11";
                    break;
                case "Dec":
                    month = "12";
                    break;
            }
            date = string.Format("{0}-{1}-{2} {3}", year, month, day, time);
            return true;
        }

        public string GetFieldSearchFieldWhereSql(string filterjson)
        {
            if (string.IsNullOrEmpty(filterjson)) return string.Empty;
            List<KendoUIFilter> listfields = JsonConvert.DeserializeObject<List<KendoUIFilter>>(filterjson);
            if (listfields == null || listfields.Count <= 0) return string.Empty;
            string where_sql = string.Empty;
            foreach (var filter in listfields)
            {
                if (string.IsNullOrEmpty(filter.value)) continue;
                if (!string.IsNullOrEmpty(where_sql)) where_sql += " and ";
                var field = "";
                if (filter.field != null)
                {
                    field = filter.field.Replace("$", ".");
                }
                var value = filter.value;

                switch (filter.type)
                {
                    case "单行文本":
                    case "多行文本":
                        where_sql += string.Format("{0} like '%{1}%' ", field, value);
                        break;
                    case "整数":
                    case "浮点数":
                    case "货币":
                        where_sql += string.Format("{0} {1} {2}", field, filter.opera, value);
                        break;
                    case "两个选项":
                    case "选项集":
                    case "关联其他表":
                        where_sql += string.Format("{0} in({1}) ", field, value);
                        break;
                    case "选项集多选":
                        where_sql += string.Format("EXISTS (SELECT top 1 ID FROM Sys_Dictionary WHERE Value IN (SELECT * FROM fn_split({0},',')) AND Value IN (SELECT * FROM fn_split('{1}',',')))", field, value);
                        break;
                    case "关联其他表多选":
                        int i = 0;
                        foreach (var v in value.Split(new char[] { ',' }))
                        {
                            if (i++ > 0)
                            {
                                where_sql += " AND ";
                            }
                            where_sql += string.Format("{0} LIKE '%{1}%' ", field, v);
                        }
                        break;
                    case "时间":
                        if (filter.opera == "<=")
                        {
                            if (value.IndexOf("00:00:00") != -1)
                            {
                                value = value.Replace("00:00:00", "");
                                value = value + " 23:59:59";
                            }
                        }
                        where_sql += string.Format("{0} {1} '{2}' ", field, filter.opera, value);
                        break;
                    case "日期":
                        value = value.Replace("00:00:00", "");
                        if (filter.opera == "<=")
                        {
                            value = value + " 23:59:59";
                        }
                        else if (filter.opera == ">=")
                        {
                            value = value + " 00:00:00";
                        }
                        where_sql += string.Format("{0} {1} '{2}' ", field, filter.opera, value);
                        break;
                    case "keyword":
                        int z = 0;
                        foreach (var _field in filter.fieldlist.Split(new char[] { ',' }))
                        {
                            if (z++ > 0)
                            {
                                where_sql += " OR ";
                            }
                            else
                            {
                                where_sql += "(";
                            }
                            where_sql += string.Format("{0} LIKE '%{1}%' ", _field.Replace("$", "."), value);
                        }
                        if (z != 0)
                        {
                            where_sql += ")";
                        }
                        break;
                    case "sql":
                        where_sql += value;
                        break;
                    default:
                        where_sql += string.Format("{0}='{1}' ", field, value);
                        break;
                }
            }
            return where_sql;
        }
    }

    public class KendoUIFilter
    {
        public string field { get; set; }
        public string fieldlist { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string opera { get; set; }
        public int sort { get; set; }
    }
}
