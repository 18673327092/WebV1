using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace Utility.Components
{
    public static class SysHelper
    {
        public static string GetServerMac()
        {
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MO in MOC)
                {
                    if (MO["IPEnabled"].ToString() == "True") return MO["MacAddress"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        public static string GetServiceIP()
        {
            try
            {
                string stringIP = "";
                ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MC.GetInstances();

                foreach (ManagementObject MO in MOC)
                {
                    if ((bool)MO["IPEnabled"] == true)
                    {
                        string[] IPAddresses = (string[])MO["IPAddress"]; //获取本地的IP地址
                        if (IPAddresses.Length > 0)
                            stringIP = IPAddresses[0];
                    }
                }
                return stringIP;
            }
            catch
            {
                return "";
            }
        }

        public static string SiteKey
        {
            get
            {
                return SysHelper.GetServerMac().Replace(":", "") + SysHelper.GetServiceIP().Replace(".", "");
            }
        }
    }
}
