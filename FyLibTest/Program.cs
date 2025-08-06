using FyLib;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FyLibTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var t = Task.Run(() =>
            {
                Console.WriteLine("=== FyLib 测试程序 ===");

                // 测试IP地址验证功能
                string str = "12.3";
                Console.WriteLine($"字符串 \"{str}\" 是否为IP地址: {str.IsIp}");
                Debug.WriteLine($"Debug: 字符串 \"{str}\" 是否为IP地址: {str.IsIp}");

                // 测试时间戳功能
                var utcTimestamp = TimeHelper.TimeStamp();
                var localTimestamp = TimeHelper.LocalTimeStamp();
                Console.WriteLine($"UTC时间戳: {utcTimestamp}");
                Console.WriteLine($"本地时间戳: {localTimestamp}");
                Console.WriteLine($"Debug: UTC时间戳: {utcTimestamp}");
                Console.WriteLine($"Debug: 本地时间戳: {localTimestamp}");

                // 显示系统信息
                Console.WriteLine("\n=== 系统信息 ===");
                Console.WriteLine($"OS Description: {RuntimeInformation.OSDescription}");
                Console.WriteLine($"OS Architecture: {RuntimeInformation.OSArchitecture}");
                Console.WriteLine($"Is 64-bit OS: {Environment.Is64BitOperatingSystem}");
                Console.WriteLine($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
                Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
                Console.WriteLine($"Is Windows: {RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
                Console.WriteLine($"Is Linux: {RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
                Console.WriteLine($"Is macOS: {RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");

                // 增加更多测试用例
                Console.WriteLine("\n=== 更多测试用例 ===");
                string[] testIps = { "192.168.1.1", "256.1.1.1", "127.0.0.1", "invalid.ip", "10.0.0.1" };
                foreach (var ip in testIps)
                {
                    Console.WriteLine($"IP地址 \"{ip}\" 是否有效: {ip.IsIp()}");
                }
            });
            t.Wait();
            Console.WriteLine("\n按任意键退出...");
            Console.Read();
        }
    }
}