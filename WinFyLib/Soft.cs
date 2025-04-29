using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using static HashHelper;

namespace WinFyLib
{
    internal class HardWareHelper
    {


        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string result = "";
                using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
                {
                    while (managementObjectEnumerator.MoveNext())
                    {
                        var t = ((ManagementObject)managementObjectEnumerator.Current);
                        if (t["__PATH"].ToString().IndexOf("PHYSICALDRIVE0") > -1)
                        {
                            result = t["SerialNumber"].ToString().Trim();
                            break;
                        }
                        result = t["SerialNumber"].ToString().Trim();
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string GetBaseBoardID()
        {
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                string result = "";
                using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
                {
                    if (managementObjectEnumerator.MoveNext())
                    {
                        result = ((ManagementObject)managementObjectEnumerator.Current)["SerialNumber"].ToString().Trim();
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取CPU名称
        /// </summary>
        /// <returns></returns>
        public static string GetCPUName()
        {
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                string result = "";
                using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
                {
                    if (managementObjectEnumerator.MoveNext())
                    {
                        result = ((ManagementObject)managementObjectEnumerator.Current)["Name"].ToString().Trim();
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取显卡名称
        /// </summary>
        /// <returns></returns>
        public static string GetDisplayName()
        {
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");
                string result = "";
                using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
                {
                    if (managementObjectEnumerator.MoveNext())
                    {
                        result = ((ManagementObject)managementObjectEnumerator.Current)["Caption"].ToString().Trim();
                    }
                }
                return result;
            }
            catch
            {
                return "";
            }
        }
        private static string CPU = "";
        /// <summary>
        /// 获取一个唯一的机器码
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            if (CPU == "")
            {
                CPU = GetCPUName();
            }
            return (CPU + GetBaseBoardID() + GetDisplayName() + GetHardDiskID()).MD5();
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string? GetBaseBordID2()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["UUID"] != null)
                {
                    string? str = mo["UUID"].ToString();
                    return str;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string? GetCpuID()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["ProcessorId"] != null)
                {
                    string? str = mo["ProcessorId"].ToString();
                    return str;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取机器码2
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode2()
        {
            string? cpu = GetCpuID();
            if (cpu == null) throw new Exception("获取CPU序列号失败");
            string? bord = GetBaseBordID2();
            if (bord == null) throw new Exception("获取主板序列号失败");
            string disk = GetHardDiskID();
            if (disk.IsNullOrEmpty()) throw new Exception("获取硬盘序列号失败");
            CRC32 crc = new CRC32();
            return $"{HashHelper.Crc32_(cpu)}-{HashHelper.Crc32_(bord)}-{HashHelper.Crc32_(disk)}";
        }
    }
}
