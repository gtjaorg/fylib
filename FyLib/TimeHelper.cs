using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FyLib
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 根据时间戳获取某日的起始时间戳和结束时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static (int, int) GetStartEndTimestamps(int timestamp = 0)
        {
            if (timestamp == 0)
            {
                timestamp = TimeStamp();
            }
            // 将10位Unix时间戳转换为DateTime
            var date = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            // 获取当天的开始时间和结束时间
            var startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);
            var endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, DateTimeKind.Local);
            // 将DateTime转换回Unix时间戳
            var startTimestamp = (int)(new DateTimeOffset(startDate).ToUnixTimeSeconds());
            var endTimestamp = (int)(new DateTimeOffset(endDate).ToUnixTimeSeconds());

            return (startTimestamp, endTimestamp);
        }
        /// <summary>
        /// 取当前时区时间戳 十位
        /// </summary>
        /// <returns></returns>
        public static int LocalTimeStamp()
        {
            return checked((int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        /// <summary>
        /// 获取时间戳UTC
        /// </summary>
        /// <returns></returns>
        public static int TimeStamp()
        {
            return checked((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        /// <summary>
        /// 获取时间戳毫秒UTC
        /// </summary>
        /// <returns></returns>
        public static long TimeStampX()
        {
            return checked((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMicroseconds);
        }
        /// <summary>
        /// 取当前时区时间戳 十三位
        /// </summary>
        /// <returns></returns>
        public static long LocalTimeStampX()
        {
            return checked((long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
        }

        /// <summary>
        /// 取指定时间的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int TimeStamp(this DateTime time)
        {
            return checked((int)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        /// <summary>
        /// 取指定时间的本地时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int LocalTimeStamp(this DateTime time)
        {
            return checked((int)(time- new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        /// <summary>
        /// UTC时间戳转换本地时间戳
        /// </summary>
        /// <param name="utcTimestamp"></param>
        /// <returns></returns>
        public static long UtcTimestampToLocalTimestamp(long utcTimestamp)
        {
            // 将 UTC 时间戳转换为 DateTime
            var utcDateTime = DateTimeOffset.FromUnixTimeSeconds(utcTimestamp).UtcDateTime;
            // 将 UTC 时间转换为本地时间
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
            // 将本地时间转换为本地时间戳（以秒为单位）
            long localTimestamp = localDateTime.LocalTimeStamp();
            return localTimestamp;
        }
        /// <summary>
        /// UTC时间戳转换本地时间戳
        /// </summary>
        /// <param name="utcTimestamp"></param>
        /// <returns></returns>
        public static int UtcTimestampToLocalTimestamp(int utcTimestamp)
        {
            // 将 UTC 时间戳转换为 DateTime
            var utcDateTime = DateTimeOffset.FromUnixTimeSeconds(utcTimestamp).UtcDateTime;
            // 将 UTC 时间转换为本地时间
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
            // 将本地时间转换为本地时间戳（以秒为单位）
            var localTimestamp = localDateTime.LocalTimeStamp();
            return localTimestamp;
        }
        /// <summary>
        /// 本地时间戳转换Utc时间戳
        /// </summary>
        /// <param name="localTimestamp"></param>
        /// <returns></returns>
        public static int LocalTimestampToUtcTimestamp(int localTimestamp)
        {
            // 将时间戳转换为 DateTime
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(localTimestamp).DateTime;
            // 将UTC时间转换为UTC时间戳（以秒为单位）
            var utcTimestamp = dateTime.TimeStamp();
            return utcTimestamp;
        }
        /// <summary>
        /// 本地时间戳转换Utc时间戳
        /// </summary>
        /// <param name="localTimestamp"></param>
        /// <returns></returns>
        public static long LocalTimestampToUtcTimestamp(long localTimestamp)
        {
            // 将时间戳转换为 DateTime
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(localTimestamp).DateTime;
            // 将UTC时间转换为UTC时间戳（以秒为单位）
            long utcTimestamp = dateTime.TimeStamp();
            return utcTimestamp;
        }
        /// <summary>
        /// 获取基于当前时间的特定时间时间戳
        /// </summary>
        /// <param name="daysFromToday">日期， 1明天 2后天</param>
        /// <param name="time">文本日期， 比如"07:00"</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static long GetUtcTimestampAtSpecificTime(int daysFromToday, string time)
        {
            // 尝试从提供的时间字符串中解析时间
            if (!TimeSpan.TryParse(time, out var parsedTime))
            {
                throw new ArgumentException("Invalid time format. Please use HH:mm format.");
            }
            // 获取当前日期
            var today = DateTime.Today;
            // 设置新的日期和时间，增加天数和解析得到的小时及分钟
            var targetDateTime = today.AddDays(daysFromToday)
                .AddHours(parsedTime.Hours)
                .AddMinutes(parsedTime.Minutes);
            // 获取本地时区信息
            var localZone = TimeZoneInfo.Local;
            // 将本地时间转换为 UTC 时间
            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(targetDateTime, localZone);
            // 获取从1970年1月1日到 UTC 时间的时间间隔（时间戳）
            var elapsedTime = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // 返回时间戳（秒）
            return (long)elapsedTime.TotalSeconds;
        }
        /// <summary>
        /// 判断时间戳是否大于指定的时间，如 10:00
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="compareTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsTimestampGreaterThanTime(long timestamp, string compareTime)
        {
            // 将时间戳转换为 DateTime 对象（UTC时间）
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            // 将 UTC 时间转换为本地时间
            var localDateTime = dateTime.ToLocalTime();
            // 提取小时和分钟
            var hour = localDateTime.Hour;
            var minute = localDateTime.Minute;
            // 解析传入的时间字符串
            var timeParts = compareTime.Split(':');
            if (timeParts.Length != 2 || !int.TryParse(timeParts[0], out var compareHour) || !int.TryParse(timeParts[1], out var compareMinute))
            {
                throw new ArgumentException("时间格式不正确，请使用 HH:mm 格式。");
            }
            // 只比较小时和分钟
            return hour > compareHour || (hour == compareHour && minute > compareMinute);
        }
        /// <summary>
        /// 判断时间戳是否小于指定时间如 10:00
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="compareTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsTimestampLessThanTime(long timestamp, string compareTime)
        {
            // 将时间戳转换为 DateTime 对象（UTC时间）
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            // 将 UTC 时间转换为本地时间
            var localDateTime = dateTime.ToLocalTime();
            // 提取小时和分钟
            var hour = localDateTime.Hour;
            var minute = localDateTime.Minute;
            // 解析传入的时间字符串
            var timeParts = compareTime.Split(':');
            if (timeParts.Length != 2 || !int.TryParse(timeParts[0], out var compareHour) || !int.TryParse(timeParts[1], out var compareMinute))
            {
                throw new ArgumentException("时间格式不正确，请使用 HH:mm 格式。");
            }

            // 只比较小时和分钟
            return hour < compareHour || (hour == compareHour && minute < compareMinute);
        }
        /// <summary>
        /// 判断时间是否在范围内
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool IsTimestampWithinTimeRange(long timestamp, string startTime, string endTime)
        {
            // 将时间戳转换为 DateTime 对象（UTC时间）
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            // 将 UTC 时间转换为本地时间
            var localDateTime = dateTime.ToLocalTime();

            // 获取当前时间
            // 将 StartTime 和 EndTime 转换为 TimeSpan
            var startTimeSpan = TimeSpan.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
            var endTimeSpan = TimeSpan.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture);
            // 构造当天的开始和结束时间
            var startDateTime = localDateTime.Date + startTimeSpan;
            var endDateTime = localDateTime.Date + endTimeSpan;
            // 如果结束时间小于开始时间，说明跨越了午夜，将结束时间加一天
            if (endTimeSpan < startTimeSpan)
            {
                endDateTime = endDateTime.AddDays(1);
            }
            // 判断当前时间是否在范围内
            return (localDateTime >= startDateTime) && (localDateTime <= endDateTime);
        }

        /// <summary>
        /// 解析时间文本，返回小时和分钟
        /// </summary>
        /// <param name="time">HH:mm</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (int hour, int minute) ParseTime(string time)
        {
            var timeParts = time.Split(':');
            if (timeParts.Length != 2 || !int.TryParse(timeParts[0], out var hour) || !int.TryParse(timeParts[1], out var minute))
            {
                throw new ArgumentException("时间格式不正确，请使用 HH:mm 格式。");
            }
            return (hour, minute);
        }

        /// <summary>
        /// 检查给定的时间是否在时间段内
        /// </summary>
        /// <param name="now">要检查的时间</param>
        /// <param name="timeRange">时间段字符串，格式为 "HH:mm-HH:mm"</param>
        /// <returns>如果时间在范围内，则为 true，否则为 false。</returns>
        public static bool IsTimeInRange(string timeRange, DateTime? now = null)
        {
            var time = now ?? DateTime.Now;
            var times = timeRange.Split('-');
            if (times.Length != 2)
                return false;
            var startTime = DateTime.ParseExact(times[0], "HH:mm", CultureInfo.InvariantCulture);
            var endTime = DateTime.ParseExact(times[1], "HH:mm", CultureInfo.InvariantCulture);
            // 将日期设置为今天
            startTime = time.Date + startTime.TimeOfDay;
            endTime = time.Date + endTime.TimeOfDay;
            // 如果结束时间小于开始时间，认为时间段跨越了午夜
            if (endTime < startTime)
            {
                endTime = endTime.AddDays(1);
            }
            return time >= startTime && time <= endTime;
        }
        /// <summary>
        /// 获取两个时间戳之间的天数差 time2-time1
        /// </summary>
        /// <param name="timestamp1"></param>
        /// <param name="timestamp2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetTimeDifferenceInDays(int timestamp1, int timestamp2)
        {
            if (timestamp1 < 0 || timestamp2 < 0)
            {
                throw new ArgumentException("时间戳不能为负数。");
            }
            var dateTimeOffset1 = timestamp1.UnixTimeStampToDateTime();
            var dateTimeOffset2 = timestamp2.UnixTimeStampToDateTime();
            return (int)(dateTimeOffset2.Date - dateTimeOffset1.Date).TotalDays;
        }
        /// <summary>
        /// 获取两个时间戳之间的小时差
        /// </summary>
        /// <param name="timestamp1"></param>
        /// <param name="timestamp2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetTimeDifferenceInHours(int timestamp1, int timestamp2)
        {
            if (timestamp1 < 0 || timestamp2 < 0)
            {
                throw new ArgumentException("时间戳不能为负数。");
            }
            var dateTimeOffset1 = timestamp1.UnixTimeStampToDateTime();
            var dateTimeOffset2 = timestamp2.UnixTimeStampToDateTime();
            return (int)(dateTimeOffset2 - dateTimeOffset1).TotalHours;
        }
        /// <summary>
        /// 获取两个时间戳之间的分钟差
        /// </summary>
        /// <param name="timestamp1"></param>
        /// <param name="timestamp2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetTimeDifferenceInMinutes(int timestamp1, int timestamp2)
        {
            if (timestamp1 < 0 || timestamp2 < 0)
            {
                throw new ArgumentException("时间戳不能为负数。");
            }
            var dateTimeOffset1 = timestamp1.UnixTimeStampToDateTime();
            var dateTimeOffset2 = timestamp2.UnixTimeStampToDateTime();
            return (int)(dateTimeOffset2 - dateTimeOffset1).TotalMinutes;
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
            Debug.WriteLine(milliseconds);
            // NTP 时间从 1900 年 1 月 1 日开始
            var ntpTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ntpTime = ntpTime.AddMilliseconds(milliseconds);
            // 转换为北京时间（UTC+8）
            return ntpTime.AddHours(8);
        }

    }
}
