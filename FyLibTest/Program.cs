using FyLib;
using FyLib.FyLib;

using Newtonsoft.Json.Linq;

using System;
using System.Diagnostics;
using System.Formats.Tar;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FyLibTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var t = Task.Run(async () => {

                string str = "12.3";
                Debug.WriteLine(str.IsDecimal);
                Debug.WriteLine(TimeHelper.TimeStamp());
                Debug.WriteLine(TimeHelper.LocalTimeStamp());

                Console.WriteLine($"OS Description: {RuntimeInformation.OSDescription}");
                Console.WriteLine($"OS Architecture: {RuntimeInformation.OSArchitecture}");
                Console.WriteLine($"Is 64-bit OS: {Environment.Is64BitOperatingSystem}");
                Console.WriteLine($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
                Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
                Console.WriteLine($"Is Windows: {RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
                Console.WriteLine($"Is Linux: {RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
                Console.WriteLine($"Is macOS: {RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
            });
            t.Wait();
            Console.Read();
        }
    }
}