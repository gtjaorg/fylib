using System;
using System.Collections.Generic;
using System.Linq;
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
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp);
        DateTime dateTime = dateTimeOffset.LocalDateTime;
        return dateTime;
    }
    /// <summary>
    /// 十位时间戳转Datetime
    /// </summary>
    /// <param name="unixTimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);
        DateTime dateTime = dateTimeOffset.LocalDateTime;
        return dateTime;
    }
    /// <summary>
    /// Datetime转十三位时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToUnixTimeStampX(this DateTime dateTime)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
        return dateTimeOffset.ToUnixTimeMilliseconds();
    }
    /// <summary>
    /// Datetime转十位时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static int DateTimeToUnixTimeStamp(this DateTime dateTime)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
        return (int)dateTimeOffset.ToUnixTimeSeconds();
    }


}

