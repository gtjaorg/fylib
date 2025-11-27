// Other

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

/// <summary>
/// 其他辅助扩展类
/// </summary>
public static class Other
{
    private static readonly int MiStartTime = TimeStamp();


    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="ms">毫秒</param>
    /// <returns></returns>
    public static TimeSpan GetTimeSpan(long ms)
    {
        var milliseconds = Convert.ToInt32(ms % 1000);
        var seconds = Convert.ToInt32(ms / 1000 % 60);
        var minutes = Convert.ToInt32(ms / 1000 / 60 % 60);
        var hours = Convert.ToInt32(ms / 1000 / 60 / 60 % 24);
        return new TimeSpan(Convert.ToInt32(ms / 1000 / 60 / 60 / 24), hours, minutes, seconds, milliseconds);
    }

    /// <summary>
    /// Thread Sleep
    /// </summary>
    /// <param name="time"></param>
    public static void Sleep(int time)
    {
        Thread.Sleep(time);
    }

    /// <summary>
    /// 取当前时间戳 十位
    /// </summary>
    /// <returns></returns>
    public static int TimeStamp()
    {
        return checked((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
    }

    /// <summary>
    /// 获取当前时区时间戳
    /// </summary>
    /// <returns></returns>
    public static int LocalTimeStamp()
    {
        return checked((int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
    }

    /// <summary>
    /// 获取当前时区时间戳毫秒
    /// </summary>
    /// <returns></returns>
    public static int LocalTimeStampX()
    {
        return checked((int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
    }

    /// <summary>
    /// 取时间戳 十三位
    /// </summary>
    /// <returns></returns>
    public static long TimeStampX()
    {
        return checked((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
    }

    /// <summary>
    /// 获取基于当前时间的特定时间时间戳
    /// </summary>
    /// <param name="daysFromToday">日期， 1明天 2后天</param>
    /// <param name="time">文本日期， 比如"07:00"</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static long GetTimestampAtSpecificTime(int daysFromToday, string time)
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
    /// 时间戳到文本
    /// </summary>
    /// <param name="timestamp">十位的时间戳</param>
    /// <param name="isMilliseconds">是否毫秒</param>
    /// <returns></returns>
    public static string TimeStampToString(int timestamp, bool isMilliseconds = false)
    {
        // 判断时间戳是否为毫秒级
        if (isMilliseconds)
        {
            // 如果是毫秒级，将时间戳除以1000得到以秒为单位的时间戳
            timestamp = timestamp / 1000;
        }

        // 转换时间戳为DateTime
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        // 返回文本日期格式
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 时间戳到日期
    /// </summary>
    /// <param name="timestamp">日期</param>
    /// <param name="isMilliseconds">是否毫秒</param>
    /// <returns></returns>
    public static DateTime StampToDatetime(long timestamp, bool isMilliseconds = false)
    {
        // 判断时间戳是否为13位
        if (isMilliseconds)
        {
            // 如果是13位，将时间戳除以1000得到以秒为单位的时间戳
            timestamp = timestamp / 1000;
        }

        // 转换时间戳为DateTime
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();

        return dateTime;
    }

    /// <summary>
    /// 取随机字节集
    /// </summary>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static byte[] RandBytes(int len = 16)
    {
        var random = RandomNumberGenerator.Create();
        var array = new byte[len];
        random.GetBytes(array);
        return array;
    }

    /// <summary>
    /// 取随机数文本
    /// </summary>
    /// <param name="len">长度</param>
    /// <param name="type">类型:0数字大小写英文,1数字,2小写英文,3大写英文</param>
    /// <returns></returns>
    public static string RandString(int len = 16, int type = 0)
    {
        var separator = ",".ToCharArray();

        var text = type switch
        {
            0 =>
                "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
            1 => "0,1,2,3,4,5,6,7,8,9",
            2 => "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z",
            3 => "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
            _ =>
                "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
        };
        var array = text.Split(separator, text.Length);
        var text2 = string.Empty;
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        for (var i = 0; i < len; i++)
        {
            rng.GetBytes(randomBytes);
            var randomValue = BitConverter.ToUInt32(randomBytes, 0);
            text2 += array[randomValue % array.Length];
        }

        return text2;
    }


    /// <summary>
    /// 随机数字
    /// </summary>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns></returns>
    public static int RandInt(int minValue, int maxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue);
        if (minValue == maxValue) return minValue;

        using var rng = RandomNumberGenerator.Create();
        var randomNumber = new byte[4]; // Create a byte array to hold the data
        rng.GetBytes(randomNumber); // Fill the array with random bytes
        var result = BitConverter.ToInt32(randomNumber, 0); // Convert bytes to an integer
        return (Math.Abs(result) % (maxValue - minValue + 1)) + minValue; // Map the result to the specified range
    }

    /// <summary>
    /// 生成指定范围内的随机整数
    /// </summary>
    /// <param name="max">随机整数的最大值, 默认100</param>
    /// <returns>返回一个大于等于1且小于等于max的随机整数</returns>
    public static int RandInt(int max = 100)
    {
        return RandInt(1, max);
    }


    /// <summary>
    /// 格式化文件大小
    /// </summary>
    /// <param name="fileSize">单位bytes</param>
    /// <returns></returns>
    public static string FormatFileSize(long fileSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(fileSize);

        return fileSize switch
        {
            >= 1073741824 => $"{fileSize / 1073741824.0:########0.00} GB",
            >= 1048576 => $"{fileSize / 1048576.0:####0.00} MB",
            >= 1024 => $"{fileSize / 1024.0:####0.00} KB",
            _ => $"{fileSize} bytes"
        };
    }

    /// <summary>
    /// 获取程序启动时间
    /// </summary>
    /// <returns></returns>
    public static string GetRunTime()
    {
        return FormatSeconds(checked(TimeStamp() - MiStartTime));
    }

    /// <summary>
    /// 格式化时间,将经过的秒数转化为格式文本
    /// </summary>
    /// <param name="t">秒数</param>
    /// <returns></returns>
    public static string FormatSeconds(int t)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(t);

        var days = t / 86400;
        var hours = (t % 86400) / 3600;
        var minutes = (t % 3600) / 60;
        var seconds = t % 60;

        var parts = new List<string>();

        if (days > 0)
            parts.Add($"{days}天");

        if (hours > 0)
            parts.Add($"{hours}小时");

        if (minutes > 0)
            parts.Add($"{minutes}分钟");

        if (seconds > 0 || parts.Count == 0) // 如果没有其他部分，至少显示秒数
            parts.Add($"{seconds}秒");

        return string.Join("", parts);
    }


    /// <summary>
    /// 取随机网卡
    /// </summary>
    /// <returns></returns>
    public static string GetRandMac()
    {
        var stringBuilder = new StringBuilder();
        _ = Array.Empty<byte>();
        var array = RandBytes(6);
        foreach (var b in array)
        {
            stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
            stringBuilder.Append(':');
        }

        var text = stringBuilder.ToString();
        return text[..^1];
    }

    /// <summary>
    /// 根据一个字节集取网卡
    /// </summary>
    /// <param name="bin">一个字节数组最小长度为6</param>
    /// <returns></returns>
    public static string GetMac(byte[] bin)
    {
        var stringBuilder = new StringBuilder();
        if (bin.Length > 6)
        {
            bin = bin.Take(6).ToArray();
        }

        if (bin.Length < 6)
        {
            return "";
        }

        var array = bin;
        foreach (var b in array)
        {
            stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
            stringBuilder.Append(':');
        }

        var text = stringBuilder.ToString();
        return text[..^1];
    }

    /// <summary>
    /// 获取随机IMEI
    /// </summary>
    /// <returns></returns>
    public static string GetRandImei()
    {
        var text = "86" + RandString(14, 1);
        var num = long.Parse(text);
        var num2 = 0;
        checked
        {
            for (var i = 0; i < 7; i++)
            {
                var num3 = (int)(num % 10) * 2;
                num = (num / 10);
                var num4 = (int)(num % 10);
                num = (num / 10);
                num2 += num4 + (num3 / 10) + (num3 % 10);
            }

            num2 = 10 - (num2 % 10);
            if (num2 == 10)
            {
                num2 = 0;
            }

            return text + num2;
        }
    }

    /// <summary>
    /// 计算IMEI最后一位
    /// </summary>
    /// <param name="str">imei的前14位</param>
    /// <returns></returns>
    public static string HashImeiCode(string str)
    {
        var num = long.Parse(str);
        var num2 = 0;
        checked
        {
            for (var i = 0; i < 7; i++)
            {
                var num3 = (int)(num % 10) * 2;
                num = num / 10;
                var num4 = (int)(num % 10);
                num = num / 10;
                num2 += num4 + num3 / 10 + num3 % 10;
            }

            num2 = 10 - num2 % 10;
            if (num2 == 10)
            {
                num2 = 0;
            }

            return str + num2;
        }
    }

    /// <summary>
    /// 检测通信
    /// </summary>
    /// <param name="ip">IP地址或域名</param>
    /// <param name="timeOut">超时时间</param>
    /// <returns></returns>
    public static bool Ping(string ip, int timeOut = 1000)
    {
        var ping = new Ping();
        new PingOptions().DontFragment = true;
        return ping.Send(ip, timeOut).Status == IPStatus.Success;
    }

    /// <summary>
    /// 通讯检测
    /// </summary>
    /// <returns>返回与目标的延迟</returns>
    public static int NetTest(string ip)
    {
        var ping = new Ping();
        new PingOptions().DontFragment = true;
        return checked((int)ping.Send(ip, 1000).RoundtripTime);
    }

    /// <summary>
    /// 域名转换IP
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static string DomainToIp(string domain)
    {
        if (domain.IsIp)
        {
            return domain;
        }

        try
        {
            return new IPEndPoint(Dns.GetHostEntry(domain).AddressList[0], 0).Address.ToString();
        }
        catch
        {
            return "0.0.0.0";
        }
    }

    /// <summary>
    /// 获取一个随机TCP端口
    /// </summary>
    /// <returns></returns>
    public static int GetRandomPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var listenPort = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return listenPort;
    }

    /// <summary>
    /// 获取机器码（基于硬件特征生成唯一标识）
    /// </summary>
    /// <returns>32位十六进制机器码字符串</returns>
    /// <remarks>
    /// 机器码基于以下信息生成：
    /// 1. 所有网卡MAC地址组合
    /// 2. 计算机名称和用户信息
    /// 3. 操作系统详细信息
    /// 4. 处理器和内存信息
    /// 5. 磁盘序列号信息
    /// 6. 系统安装时间等唯一标识
    /// 生成的机器码在同一台机器上保持稳定，不同机器间具有高度唯一性
    /// </remarks>
    public static string GetMachineCode()
    {
        try
        {
            var machineInfo = new StringBuilder();

            // 1. 获取所有网卡MAC地址（不只是第一个）
            var allMacAddresses = GetAllMacAddresses();
            machineInfo.Append(string.Join("", allMacAddresses));

            // 2. 计算机名称和用户信息
            machineInfo.Append(Environment.MachineName);
            machineInfo.Append(Environment.UserName);
            machineInfo.Append(Environment.UserDomainName);

            // 3. 操作系统详细信息
            machineInfo.Append(Environment.OSVersion.ToString());
            machineInfo.Append(Environment.OSVersion.Version.ToString());
            machineInfo.Append(RuntimeInformation.OSDescription);
            machineInfo.Append(RuntimeInformation.OSArchitecture.ToString());

            // 4. 处理器详细信息
            machineInfo.Append(Environment.ProcessorCount.ToString());
            machineInfo.Append(RuntimeInformation.ProcessArchitecture.ToString());

            // 5. 系统目录路径（包含驱动器信息）- 替代内存信息，更稳定
            machineInfo.Append(Environment.SystemDirectory);
            machineInfo.Append(Environment.CurrentDirectory);

            // 6. 环境变量中的唯一信息
            var computerName = Environment.GetEnvironmentVariable("COMPUTERNAME")
                            ?? Environment.GetEnvironmentVariable("HOSTNAME");
            if (!string.IsNullOrEmpty(computerName))
            {
                machineInfo.Append(computerName);
            }

            // 7. 获取系统安装相关的稳定信息
            try
            {
                // 使用系统目录的创建时间（系统安装时间），这是相对稳定的
                var systemDir = new DirectoryInfo(Environment.SystemDirectory);
                if (systemDir.Exists)
                {
                    // 只取年月日，忽略时分秒，提高稳定性
                    var installDate = systemDir.CreationTime.ToString("yyyyMMdd");
                    machineInfo.Append(installDate);
                }
            }
            catch
            {
                // 最后的备用方案：使用机器名和处理器数量的组合
                machineInfo.Append($"{Environment.MachineName}_{Environment.ProcessorCount}");
            }
            // 8. 获取驱动器信息
            try
            {
                var drives = DriveInfo.GetDrives()
                    .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
                    .OrderBy(d => d.Name)
                    .ToArray();

                foreach (var drive in drives)
                {
                    machineInfo.Append(drive.Name);
                    machineInfo.Append(drive.TotalSize.ToString());

                    // 添加卷标信息
                    if (!string.IsNullOrEmpty(drive.VolumeLabel))
                    {
                        machineInfo.Append(drive.VolumeLabel);
                    }
                }
            }
            catch
            {
                // 忽略驱动器信息获取失败
            }

            // 9. 如果信息太少，添加随机但持久的标识
            if (machineInfo.Length < 50)
            {
                machineInfo.Append(GetPersistentIdentifier());
            }

            // 使用SHA256生成固定长度的机器码
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(machineInfo.ToString()));

            // 转换为32位十六进制字符串
            return Convert.ToHexString(hashBytes)[..32].ToUpper();
        }
        catch
        {
            // 如果获取硬件信息失败，返回基于时间和随机数的备用码
            return GetFallbackMachineCode();
        }
    }

    /// <summary>
    /// 获取所有可用的网卡MAC地址
    /// </summary>
    /// <returns>MAC地址列表</returns>
    private static List<string> GetAllMacAddresses()
    {
        var macAddresses = new List<string>();

        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up
                          && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                          && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                .OrderBy(ni => ni.Name) // 保证顺序一致性
                .ToArray();

            foreach (var ni in networkInterfaces)
            {
                var mac = ni.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(mac) && mac != "000000000000")
                {
                    macAddresses.Add(mac);
                }
            }
        }
        catch
        {
            // 忽略获取MAC地址的异常
        }

        return macAddresses;
    }

    /// <summary>
    /// 获取持久的标识符（基于系统特征生成的伪随机值）
    /// </summary>
    /// <returns>持久标识符</returns>
    private static string GetPersistentIdentifier()
    {
        // 基于系统路径和用户信息生成持久的标识
        var persistentInfo = new StringBuilder();
        persistentInfo.Append(Environment.GetFolderPath(Environment.SpecialFolder.System));
        persistentInfo.Append(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
        persistentInfo.Append(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        persistentInfo.Append(Environment.MachineName);
        persistentInfo.Append(Environment.ProcessorCount);

        // 使用固定种子确保在同一机器上生成相同的"随机"值
        var seed = persistentInfo.ToString().GetHashCode();
        var random = new Random(seed);

        return random.Next(100000000, 999999999).ToString();
    }

    /// <summary>
    /// 获取备用机器码（当硬件信息获取失败时使用）
    /// </summary>
    /// <returns>备用机器码</returns>
    private static string GetFallbackMachineCode()
    {
        var fallbackInfo = new StringBuilder();
        fallbackInfo.Append(Environment.MachineName);
        fallbackInfo.Append(Environment.UserName);
        fallbackInfo.Append(Environment.OSVersion.Platform.ToString());
        fallbackInfo.Append(Environment.ProcessorCount.ToString());
        fallbackInfo.Append(Environment.SystemDirectory);

        // 添加一个基于系统信息的稳定哈希值
        var systemHash = (Environment.MachineName + Environment.UserName).GetHashCode();
        fallbackInfo.Append(systemHash.ToString());

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fallbackInfo.ToString()));
        return Convert.ToHexString(hashBytes)[..32].ToUpper();
    }
}