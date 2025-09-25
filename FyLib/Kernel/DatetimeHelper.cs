using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 时间日期帮助类
/// </summary>
public static class DatetimeHelper
{
    /// <summary>
    /// 十三位时间戳转Datetime
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp);
        var dateTime = dateTimeOffset.LocalDateTime;
        return dateTime;
    }
    /// <summary>
    /// 十位时间戳转Datetime
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);
        var dateTime = dateTimeOffset.LocalDateTime;
        return dateTime;
    }
    /// <summary>
    /// Datetime转十三位时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToUnixTimeStampX(this DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime);
        return dateTimeOffset.ToUnixTimeMilliseconds();
    }
    /// <summary>
    /// Datetime转十位时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static int DateTimeToUnixTimeStamp(this DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime);
        return (int)dateTimeOffset.ToUnixTimeSeconds();
    }

    /// <summary>
    /// 获取阿里云的北京时间
    /// </summary>
    /// <returns></returns>
    public static async Task<DateTime> GetBeijingTimeFromNtpAsync()
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
        // NTP 时间从 1900 年 1 月 1 日开始
        var ntpTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        ntpTime = ntpTime.AddMilliseconds(milliseconds);
        // 转换为北京时间（UTC+8）
        return ntpTime.AddHours(8);
    }


}

