using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FyLib
{
    public static class IPHelper
    {
        /// <summary>
        /// 判断是否IP地址
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsIP(string IP)
        {
            if (IP.IsNullOrEmpty())
            {
                return false;
            }
            if (!Regex.IsMatch(IP, @"\d{1,3}(\.\d{1,3}){3}"))
            {
                return false;
            }
            string[] array = IP.Split('.');
            if (array.Length != 4)
            {
                return false;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (!int.TryParse(array[i], out int value) || value < 0 || value > 255)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否内网地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsInternalIP(IPAddress ipAddress)
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();

            // 根据IPv4地址范围进行判断，你也可以根据需要添加IPv6的判断
            if (ipBytes[0] == 10 ||
                (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) ||
                (ipBytes[0] == 192 && ipBytes[1] == 168))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 获取网络IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetIP()
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                for (int i = 0; i < hostEntry.AddressList.Length; i = checked(i + 1))
                {
                    if (hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostEntry.AddressList[i].ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取内网地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // 过滤掉虚拟接口和回环接口
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ppp ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                {
                    continue;
                }
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation ipInfo in ipProperties.UnicastAddresses)
                {
                    IPAddress ipAddress = ipInfo.Address;
                    // 检查IP地址是否属于内网范围
                    if (IsInternalIP(ipAddress))
                    {
                        return ipAddress.ToString();
                    }
                }
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// IP转换byte[]
        /// </summary>
        /// <returns></returns>
        public static byte[]? IPtoBytes(string ip)
        {
            if (IsIP(ip) == false)
            {
                return null;
            }
            string[] ipAddressParts = ip.Split('.');

            byte[] ipAddressArray =new byte[4];

            for (int i = 0; i < ipAddressParts.Length; i++)
            {
                ipAddressArray[i] = byte.Parse(ipAddressParts[i]);
            }
            return ipAddressArray;
        }
        /// <summary>
        /// 获取当前网段所有ip
        /// </summary>
        /// <returns></returns>
        public static List<string>? GetAllAddress(string ip)
        {
            if (ip.IndexOf("*") > -1) ip = ip.Replace("*", "1");
            var bin = IPtoBytes(ip);
            if (bin == null) return null;
            var ls = new List<string>();
            for (int i = 0; i < 255; i++)
            {
                string temp = $"{bin[0]}.{bin[1]}.{bin[2]}.{i + 1}";
                ls.Add(temp);
            }
            return ls;
        }
        /// <summary>
        /// 获取指定网段的所有IP地址
        /// </summary>
        /// <param name="StartIP"></param>
        /// <param name="EndIP"></param>
        /// <returns></returns>
        public static List<string>? GetAllAddress(string StartIP, string EndIP)
        {
            if(IsIP(StartIP) == false) { return null; }
            if(IsIP(EndIP) == false) { return null; }
            var ls = new List<string>();
            var end = IPtoBytes(EndIP);
            var start = IPtoBytes(StartIP);
            if(end ==null || start ==null) return null;
            int len =end.ToInt() - start.ToInt();
            if(len<0) return null;  
            for (int i = 0; i < len+1; i++)
            {
                string temp = $"{start[0]}.{start[1]}.{start[2]}.{start[3]+i}";
                ls.Add(temp);
            }
            return ls;
        }
    }
}
