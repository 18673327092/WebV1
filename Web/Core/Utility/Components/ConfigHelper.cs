using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Utility.Components
{

    /// <summary>
    /// 配置文件帮助器
    /// </summary>
    public class ConfigHelper
    {
        private ConfigHelper()
        {
        }

        private static ConfigHelper _single = new ConfigHelper();

        public static ConfigHelper Single
        {
            get { return _single; }
        }

        /// <summary>
        /// 获取appSettings节点项(string)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetAppSetting(string key, string defaultValue = "")
        {
            var setting = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(setting) ? defaultValue : setting;
        }

        /// <summary>
        /// 获取appSettings节点项(int)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetAppSettingInt(string key, int defaultValue = 0)
        {
            var setting = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(setting) ? defaultValue : Convert.ToInt32(setting);
        }

        /// <summary>
        /// 获取appSettings节点布尔值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetAppSettingBoolean(string key, bool defaultValue = false)
        {
            var setting = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(setting) ? defaultValue : Convert.ToBoolean(setting);
        }
    }


}
