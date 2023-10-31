

// Other
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;

using static HashHelper;

public static class Other
{
    private static int m_iStartTime = TimeStamp();

    private static string CPU = "";

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="ms">毫秒</param>
    /// <returns></returns>
    public static TimeSpan GetTimeSpan(long ms)
    {
        int milliseconds = Convert.ToInt32(ms % 1000);
        int seconds = Convert.ToInt32(ms / 1000 % 60);
        int minutes = Convert.ToInt32(ms / 1000 / 60 % 60);
        int hours = Convert.ToInt32(ms / 1000 / 60 / 60 % 24);
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
    /// 取时间戳 十三位
    /// </summary>
    /// <returns></returns>
    public static long TimeStampX()
    {
        return checked((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
    }

    /// <summary>
    /// 取指定时间的时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int TimeStamp(this DateTime time)
    {
        return checked((int)(time - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
    }

    /// <summary>
    /// 时间戳到文本
    /// </summary>
    /// <param name="timeStamp">十位的时间戳</param>
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
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        // 返回文本日期格式
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 时间戳到日期
    /// </summary>
    /// <param name="TimeStamp">日期</param>
    /// <param name="isMinSeconds">是否毫秒</param>
    /// <returns></returns>
    public static DateTime StampToDatetime(long timestamp, bool isMillisecond = false)
    {
        // 判断时间戳是否为13位
        if (isMillisecond)
        {
            // 如果是13位，将时间戳除以1000得到以秒为单位的时间戳
            timestamp = timestamp / 1000;
        }
        // 转换时间戳为DateTime
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
        Random random = new Random(default(Guid).GetHashCode());
        byte[] array = new byte[len];
        random.NextBytes(array);
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
        Random random = new Random(default(Guid).GetHashCode());
        char[] separator = ",".ToCharArray();
        string text = "";
        text = type switch
        {
            0 => "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
            1 => "0,1,2,3,4,5,6,7,8,9",
            2 => "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z",
            3 => "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
            _ => "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z",
        };
        string[] array = text.Split(separator, text.Length);
        string text2 = string.Empty;
        for (int i = 0; i < len; i = checked(i + 1))
        {
            text2 += array[random.Next(array.Length)];
        }
        return text2;
    }

    private static readonly Random random = new Random();
    /// <summary>
    /// 随机数字
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static int RandInt(int min, int max)
    {
        return random.Next(min, max);
    }

    /// <summary>
    /// 随机数字
    /// </summary>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static int RandInt(int max)
    {
        return random.Next(max);
    }

    /// <summary>
    /// 随机数字
    /// </summary>
    /// <returns></returns>
    public static int RandInt()
    {
        return random.Next();
    }

    /// <summary>
    /// 格式化文件大小
    /// </summary>
    /// <param name="fileSize">单位bytes</param>
    /// <returns></returns>
    public static string FormatFileSize(long fileSize)
    {
        if (fileSize < 0)
        {
            throw new ArgumentOutOfRangeException("fileSize");
        }
        if (fileSize >= 1073741824)
        {
            return $"{(double)fileSize / 1073741824.0:########0.00} GB";
        }
        if (fileSize >= 1048576)
        {
            return $"{(double)fileSize / 1048576.0:####0.00} MB";
        }
        if (fileSize >= 1024)
        {
            return $"{(double)fileSize / 1024.0:####0.00} KB";
        }
        return $"{fileSize} bytes";
    }

    /// <summary>
    /// 获取程序启动时间
    /// </summary>
    /// <returns></returns>
    public static string GetRunTime()
    {
        return FormatSeconds(checked(TimeStamp() - m_iStartTime));
    }

    /// <summary>
    /// 格式化时间,将经过的秒数转化为格式文本
    /// </summary>
    /// <param name="t">秒数</param>
    /// <returns></returns>
    public static string FormatSeconds(int t)
    {
        if (t >= 86400)
        {
            Convert.ToInt16(t / 86400);
            Convert.ToInt16(t % 86400 / 3600);
            Convert.ToInt16(t % 86400 % 3600 / 60);
            Convert.ToInt16(t % 86400 % 3600 % 60);
        }
        else if (t >= 3600)
        {
            Convert.ToInt16(t / 3600);
            Convert.ToInt16(t % 3600 / 60);
            Convert.ToInt16(t % 3600 % 60);
        }
        else if (t >= 60)
        {
            Convert.ToInt16(t / 60);
            Convert.ToInt16(t % 60);
        }
        else
        {
            Convert.ToInt16(t);
        }
        return new TimeSpan(0, 0, 0, t).ToString();
    }



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
                return str ;
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
        if(cpu==null) throw new Exception("获取CPU序列号失败");
        string? bord = GetBaseBordID2();
        if (bord == null) throw new Exception("获取主板序列号失败");
        string disk = GetHardDiskID();
        if(disk.IsNullOrEmpty()) throw new Exception("获取硬盘序列号失败");
        CRC32 crc = new CRC32();
        return $"{HashHelper.Crc32_(cpu)}-{HashHelper.Crc32_(bord)}-{HashHelper.Crc32_(disk)}";
    }
    /// <summary>
    /// 取随机网卡
    /// </summary>
    /// <returns></returns>
    public static string GetRandMac()
    {
        StringBuilder stringBuilder = new StringBuilder();
        _ = new byte[0];
        byte[] array = RandBytes(6);
        foreach (byte b in array)
        {
            stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
            stringBuilder.Append(":");
        }
        string text = stringBuilder.ToString();
        return text.Substring(0, checked(text.Length - 1));
    }

    /// <summary>
    /// 根据一个字节集取网卡
    /// </summary>
    /// <param name="bin">一个字节数组最小长度为6</param>
    /// <returns></returns>
    public static string GetMac(byte[] bin)
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (bin.Length > 6)
        {
            bin = bin.Take(6).ToArray();
        }
        if (bin.Length < 6)
        {
            return "";
        }
        byte[] array = bin;
        foreach (byte b in array)
        {
            stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
            stringBuilder.Append(":");
        }
        string text = stringBuilder.ToString();
        return text.Substring(0, checked(text.Length - 1));
    }

    /// <summary>
    /// 获取随机IMEI
    /// </summary>
    /// <returns></returns>
    public static string GetRandImei()
    {
        string text = "86" + RandString(14, 1);
        long num = long.Parse(text);
        int num2 = 0;
        checked
        {
            for (int i = 0; i < 7; i++)
            {
                int num3 = (int)unchecked(num % 10) * 2;
                num = unchecked(num / 10);
                int num4 = (int)unchecked(num % 10);
                num = unchecked(num / 10);
                num2 += num4 + unchecked(num3 / 10) + unchecked(num3 % 10);
            }
            num2 = 10 - unchecked(num2 % 10);
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
        long num = long.Parse(str);
        int num2 = 0;
        checked
        {
            for (int i = 0; i < 7; i++)
            {
                int num3 = (int)unchecked(num % 10) * 2;
                num = unchecked(num / 10);
                int num4 = (int)unchecked(num % 10);
                num = unchecked(num / 10);
                num2 += num4 + unchecked(num3 / 10) + unchecked(num3 % 10);
            }
            num2 = 10 - unchecked(num2 % 10);
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
    /// <param name="IP">IP地址或域名</param>
    /// <param name="TimeOut">超时时间</param>
    /// <returns></returns>
    public static bool Ping(string IP, int TimeOut = 1000)
    {
        Ping ping = new Ping();
        new PingOptions().DontFragment = true;
        return ping.Send(IP, TimeOut).Status == IPStatus.Success;
    }

    /// <summary>
    /// 通讯检测
    /// </summary>
    /// <returns>返回与目标的延迟</returns>
    public static int NetTest(string ip)
    {
        Ping ping = new Ping();
        new PingOptions().DontFragment = true;
        return checked((int)ping.Send(ip, 1000).RoundtripTime);
    }

    /// <summary>
    /// 域名转换IP
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static string DomainToIP(string domain)
    {
        if (domain.IsIP())
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
        var Listener = new TcpListener(IPAddress.Loopback, 0);
        if (Listener == null) return -1;
        Listener.Start();
        var ListenPort = ((IPEndPoint)Listener.LocalEndpoint).Port;
        Listener.Stop();
        return ListenPort;
    }
}
