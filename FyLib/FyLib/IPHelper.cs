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
    /// <summary>
    /// IP操作类
    /// </summary>
    public static partial class IPHelper
    {
        /// <summary>
        /// 判断是否IP地址
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IsIP(string IP)
        {
            if (IP.IsNull)
            {
                return false;
            }
            if (!MyRegex().IsMatch(IP))
            {
                return false;
            }
            var array = IP.Split('.');
            if (array.Length != 4)
            {
                return false;
            }
            for (var i = 0; i < array.Length; i++)
            {
                if (!int.TryParse(array[i], out var value) || value < 0 || value > 255)
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
            var ipBytes = ipAddress.GetAddressBytes();

            // 根据IPv4地址范围进行判断，你也可以根据需要添加IPv6的判断
            if (ipBytes[0] == 10 ||
                ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31 ||
                ipBytes[0] == 192 && ipBytes[1] == 168)
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
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                for (var i = 0; i < hostEntry.AddressList.Length; i = checked(i + 1))
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
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                // 过滤掉虚拟接口和回环接口
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ppp ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                {
                    continue;
                }
                var ipProperties = networkInterface.GetIPProperties();
                foreach (var ipInfo in ipProperties.UnicastAddresses)
                {
                    var ipAddress = ipInfo.Address;
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
            var ipAddressParts = ip.Split('.');

            var ipAddressArray = new byte[4];

            for (var i = 0; i < ipAddressParts.Length; i++)
            {
                ipAddressArray[i] = byte.Parse(ipAddressParts[i]);
            }
            return ipAddressArray;
        }
        /// <summary>
        /// 获取当前网段所有ip
        /// </summary>
        /// <param name="ip">当前网段的IP地址，支持使用"*"作为通配符</param>
        /// <returns>当前网段所有IP地址的列表</returns>
        public static List<string>? GetAllAddress(string ip)
        {
            if (ip.IndexOf("*") > -1) ip = ip.Replace("*", "1");
            var bin = IPtoBytes(ip);
            if (bin == null) return null;
            var ls = new List<string>();
            for (var i = 0; i < 255; i++)
            {
                var temp = $"{bin[0]}.{bin[1]}.{bin[2]}.{i + 1}";
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
            if (IsIP(StartIP) == false) { return null; }
            if (IsIP(EndIP) == false) { return null; }
            var ls = new List<string>();
            var end = IPtoBytes(EndIP);
            var start = IPtoBytes(StartIP);
            if (end == null || start == null) return null;
            var len = end.ToInt() - start.ToInt();
            if (len < 0) return null;
            for (var i = 0; i < len + 1; i++)
            {
                var temp = $"{start[0]}.{start[1]}.{start[2]}.{start[3] + i}";
                ls.Add(temp);
            }
            return ls;
        }
        /// <summary>
        /// 测试通讯
        /// </summary>
        /// <param name="host">IP地址</param>
        /// <param name="timeout">毫秒</param>
        /// <returns></returns>
        public static async Task<bool> IsHostPingedAsync(string host, int timeout = 200)
        {
            using Ping ping = new();
            try
            {
                // 发送ping请求，包含超时设置
                var reply = await ping.SendPingAsync(host, timeout);
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Ping请求失败，可能是因为超时或其他网络错误
                return false;
            }
        }
        /// <summary>
        /// 获取本地IP段, 192.168.0
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetLocalIPAddressBase()
        {
            // 创建一个Socket，指定使用IPv4，类型为Dgram(数据报，UDP), 协议为0（自动选择）
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            // 连接到一个远程端点，这里使用的是谷歌公共DNS服务器的IP和一个不常用的端口
            // 这里的目的不是要发送数据，而是为了让操作系统选择一个用于外出连接的本地IP地址
            socket.Connect("8.8.8.8", 65530);

            // 获取本地端点的信息，这应该是操作系统为了连接到远程端点所选择的本地IP地址
            var endPoint = socket.LocalEndPoint as IPEndPoint;

            // 获取本地IP地址
            var localIP = (endPoint?.Address.ToString()) ?? throw new InvalidOperationException("Local IP Address Not Found!");

            // 找到IP地址字符串中最后一个'.'的位置
            var lastIndex = localIP.LastIndexOf('.');

            // 获取基础IP地址，即去掉最后一部分的IP地址
            return localIP[..lastIndex];
        }
        /// <summary>
        /// 端口检测
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<bool> IsPortOpenAsync(string host, int port, int timeout)
        {
            var b = await IsHostPingedAsync(host);
            if (!b)
            {
                return false;
            }
            using var client = new TcpClient();
            try
            {
                var task = client.ConnectAsync(host, port);
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    return client.Connected;
                }
                else
                {
                    // Timeout
                    return false;
                }
            }
            catch
            {
                // Exception means we couldn't connect (port is closed or host is not reachable)
                return false;
            }
        }

        [GeneratedRegex("\\d{1,3}(\\.\\d{1,3}){3}")]
        private static partial Regex MyRegex();
    }
}
