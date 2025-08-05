using FyLib;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using static System.Net.WebRequestMethods;
using System.Formats.Tar;
using System;
using FyLib.FyLib;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace FyLibTest
{
    internal class Program
    {
        static  void Main(string[] args)
        {
            var t = Task.Run(async () => {
                var bjTime = await GetBeijingTimeFromNtpAsync();
                TimeSpan timeDifference = bjTime - DateTime.Now;
                Debug.WriteLine(timeDifference.TotalMilliseconds);
            });
            t.Wait();
        }
        class WxFriendInfo
        {
            public string nick;
            public string name;
            public string id;
            public string v3;
            public string tag;
            public string head;
            public override string ToString()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }
        static async Task<DateTime> GetBeijingTimeFromNtpAsync()
        {
            // 使用阿里云的 NTP 服务器（中国）
            const string ntpServer = "ntp.aliyun.com";
            const int ntpPort = 123;
            var ntpData = new byte[48];
            ntpData[0] = 0b00100011; // LI = 0 (no warning), VN = 4 (version 4), Mode = 3 (client mode)
            using var udpClient = new UdpClient();
            await udpClient.SendAsync(ntpData, ntpData.Length, ntpServer, ntpPort);
            var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var receiveResult = await udpClient.ReceiveAsync();
            ntpData = receiveResult.Buffer;
            // 提取时间戳
            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];
            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            Debug.WriteLine(milliseconds);
            // NTP 时间从 1900 年 1 月 1 日开始
            var ntpTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ntpTime = ntpTime.AddMilliseconds(milliseconds);
            // 转换为北京时间（UTC+8）
            return ntpTime.AddHours(8);
        }
        static string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
            {
                return $"{ts.Hours}小时{ts.Minutes}分钟{ts.Seconds}秒";
            }
            else if (ts.TotalMinutes >= 1)
            {
                return $"{ts.Minutes}分钟{ts.Seconds}秒";
            }
            else
            {
                return $"{ts.Seconds}秒";
            }
        }

    }
}