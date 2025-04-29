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
                var managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                var result = "";
                using (var managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
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
                var managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                var result = "";
                using (var managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
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
                var managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                var result = "";
                using (var managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
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
                var managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");
                var result = "";
                using (var managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
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
            return (CPU + GetBaseBoardID() + GetDisplayName() + GetHardDiskID()).Md5();
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string? GetBaseBordID2()
        {
            var mc = new ManagementClass("Win32_ComputerSystemProduct");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["UUID"] != null)
                {
                    var str = mo["UUID"].ToString();
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
            var mc = new ManagementClass("Win32_Processor");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["ProcessorId"] != null)
                {
                    var str = mo["ProcessorId"].ToString();
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
            var cpu = GetCpuID();
            if (cpu == null) throw new Exception("获取CPU序列号失败");
            var bord = GetBaseBordID2();
            if (bord == null) throw new Exception("获取主板序列号失败");
            var disk = GetHardDiskID();
            if (disk.IsNullOrEmpty()) throw new Exception("获取硬盘序列号失败");
            var crc = new CRC32();
            return $"{HashHelper.Crc32_(cpu)}-{HashHelper.Crc32_(bord)}-{HashHelper.Crc32_(disk)}";
        }
    }
}
