

// FyLib.NicControl.NicControl
using System.Collections.Generic;
using System.Management;

using FyLib.NicControl;

using Microsoft.Win32;

namespace FyLib.NicControl
{
    /// <summary>
    /// 网卡控制
    /// </summary>
    public static class NicControl
    {
        private static ManagementScope managementScope = new ManagementScope("\\root\\cimv2");

        /// <summary>
        /// 所有网卡列表
        /// </summary>
        public static List<NicInfo> NicList = new List<NicInfo>();

        /// <summary>
        /// 枚举所有网卡
        /// </summary>
        public static void GetList()
        {
            SelectQuery query = new SelectQuery("select * from Win32_NetworkAdapter where PhysicalAdapter=True");
            ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(managementScope, query).Get();
            NicList.Clear();
            if (managementObjectCollection.Count > 0)
            {
                foreach (ManagementBaseObject item in managementObjectCollection)
                {
                    NicInfo nicInfo = new NicInfo();
                    if (item["NetConnectionID"] != null)
                    {
                        nicInfo.Name = item["NetConnectionID"].ToString();
                    }
                    nicInfo.Caption = item["Caption"].ToString();
                    nicInfo.Index = item["Index"].ToString().ToInt();
                    nicInfo.InterfaceIndex = item["InterfaceIndex"].ToString().ToInt();
                    if (item["MacAddress"] != null)
                    {
                        nicInfo.MACAddress = item["MACAddress"].ToString();
                    }
                    nicInfo.GUID = item["GUID"].ToString();
                    nicInfo.ConnectionStatus = (NetConnectionStatus)item["NetConnectionStatus"].ToString().ToInt();
                    NicList.Add(nicInfo);
                }
            }
            if (NicList.Count <= 0)
            {
                return;
            }
            foreach (NicInfo nic in NicList)
            {
                query = new SelectQuery("select * from Win32_NetworkAdapterConfiguration where Index = " + nic.Index);
                managementObjectCollection = new ManagementObjectSearcher(managementScope, query).Get();
                foreach (ManagementObject item2 in managementObjectCollection)
                {
                    nic.IPAddress = (string[])item2["IPAddress"];
                    if (item2["IPConnectionMetric"] != null)
                    {
                        nic.IPConnectionMetric = item2["IPConnectionMetric"].ToString().ToInt();
                    }
                }
            }
        }

        /// <summary>
        /// 网卡是否已安装
        /// </summary>
        /// <returns></returns>
        public static bool IsInstall(NicInfo nic)
        {
            if (NicList.Count == 0)
            {
                GetList();
            }
            foreach (NicInfo nic2 in NicList)
            {
                if (nic2.InterfaceIndex == nic.InterfaceIndex)
                {
                    return true;
                }
                if (nic2.Name == nic.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定网卡
        /// </summary>
        /// <param name="NicName"></param>
        /// <returns></returns>
        public static NicInfo? GetVlan(string NicName)
        {
            if (NicList.Count == 0)
            {
                GetList();
            }
            foreach (NicInfo nic in NicList)
            {
                if (nic.Name == NicName)
                {
                    return nic;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置活跃点
        /// </summary>
        /// <returns></returns>
        public static bool SetMetric(NicInfo item)
        {
            if (item == null)
            {
                return false;
            }
            if (item.GUID == null)
            {
                return false;
            }
            RegistryKey localMachine = Registry.LocalMachine;
            string name = "SYSTEM\\ControlSet001\\Services\\Tcpip\\Parameters\\Interfaces\\" + item.GUID.ToLower();
            RegistryKey registryKey = localMachine.OpenSubKey(name, writable: true);
            registryKey.SetValue("InterfaceMetric", 100);
            registryKey.Close();
            localMachine.Close();
            return true;
        }

        /// <summary>
        /// 获取活跃点
        /// </summary>
        /// <returns></returns>
        public static int GetMetric(NicInfo item)
        {
            if (item == null)
            {
                return 0;
            }
            RegistryKey localMachine = Registry.LocalMachine;
            string name = "SYSTEM\\ControlSet001\\Services\\Tcpip\\Parameters\\Interfaces\\" + item.GUID.ToLower();
            RegistryKey registryKey = localMachine.OpenSubKey(name, writable: true);
            object value = registryKey.GetValue("InterfaceMetric");
            registryKey.SetValue("InterfaceMetric", 100);
            registryKey.Close();
            localMachine.Close();
            return value?.ToString().ToInt() ?? 0;
        }

        /// <summary>
        /// 启用网卡
        /// </summary>
        /// <returns></returns>
        public static bool EnableVlan(NicInfo item)
        {
            if (item == null)
            {
                return false;
            }
            SelectQuery query = new SelectQuery("select * from Win32_NetworkAdapter where Index = " + item.Index);
            ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(managementScope, query).Get();
            if (managementObjectCollection == null)
            {
                return false;
            }
            using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator())
            {
                if (managementObjectEnumerator.MoveNext())
                {
                    return ((ManagementObject)managementObjectEnumerator.Current).InvokeMethod("Enable", null).ToString().ToInt() == 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 禁用网卡
        /// </summary>
        /// <returns></returns>
        public static bool Disable(NicInfo item)
        {
            if (item == null)
            {
                return false;
            }
            SelectQuery query = new SelectQuery("select * from Win32_NetworkAdapter where Index = " + item.Index);
            ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher(managementScope, query).Get();
            if (managementObjectCollection == null)
            {
                return false;
            }
            using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator())
            {
                if (managementObjectEnumerator.MoveNext())
                {
                    return ((ManagementObject)managementObjectEnumerator.Current).InvokeMethod("Disable", null).ToString().ToInt() == 0;
                }
            }
            return false;
        }
    }

}
