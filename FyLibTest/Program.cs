using FyLib;
using FyLib.NicControl;

using Newtonsoft.Json.Linq;

using System.Diagnostics;
using System.Management;
using System.Net.Sockets;
using System.Net;

using static System.Net.WebRequestMethods;
using System.Formats.Tar;

namespace FyLibTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Winhttp winhttp = new Winhttp();
            //winhttp.HttpVersion2 = true;
            //winhttp.Domain = "https://www.qq.com";
            //winhttp.GetAsString();
            //Debug.WriteLine(winhttp.Response.Version);
            var t = IPHelper.GetLocalIPAddressBase();
            Debug.WriteLine(t);
            Debug.WriteLine(IPHelper.GetLocalIP());
            Debug.WriteLine(await IPHelper.IsHostPingedAsync("192.168.3.3", 300));
            Debug.WriteLine(Other.GetMachineCode());
            return;


            string localIpBase = IPHelper.GetLocalIPAddressBase();
            int port = 5555;
            int timeout = 1000; // Timeout in milliseconds
            List<Task<bool>> tasks = new List<Task<bool>>();
            List<string> IPs = new List<string>();
            List<string> SuccessIP = new List<string>();

            Console.WriteLine($"Starting scan for devices with open port {port}...");

            for (int i = 1; i <= 254; i++)
            {
                string ip = $"{localIpBase}.{i}";
                tasks.Add(IPHelper.IsHostPingedAsync(ip, 300));
                IPs.Add(ip);
            }
            bool[] results = await Task.WhenAll(tasks);
            tasks.Clear();
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i])
                {
                    SuccessIP.Add(IPs[i]);
                    tasks.Add(IPHelper.IsPortOpenAsync(IPs[i], port, timeout));
                    Console.WriteLine($"{IPs[i]} is online");
                }

            }
            results = await Task.WhenAll(tasks);
            // 等待所有任务完成

            // 输出开放端口的IP地址
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i])
                {
                    Console.WriteLine($"Device found with open port {port} at: {localIpBase}.{i + 1}");
                }
                else
                {
                    Console.WriteLine($"Device not found with open port {port} at: {localIpBase}.{i + 1}");
                }
            }

            Console.WriteLine("Scan complete.");
            Console.Read();
        }
        static async Task demo()
        {
            Winhttp http = new Winhttp();
            http.Domain = "https://book.lu.ink";
            http.HttpVersion2 = false;
            http.ContentType = "application/x-www-form-urlencoded";
            http.Accept = "text/html, application/xhtml+xml, */*";
            http.Headers.Add("Version", "1.1.3");
            var body = "version=1.1.3&appid=23628&secretkey=FC9B7E979AA9118AB6EBBE0D939B3B2F&wtype=1&sign=17997b93114327168d39f807841a509d&timestamp=1698816071076&data=141F11C6AB584BE5DCA8FE6174EA8B96FC72ACD8BBF99BDFC1AF3FAABB089671F0315920B5F6007D022A32947CEDF8D0A693FC520618122567C427308D6EA2DBEEC2951A99CD9A142E3A5F9EAADEA16A4D31135F73903231761FFF4E6A12D352FCC7260BFE1F18275A44A565F4ACAA3FD1250E18CBA73E826529784CA06DDE71086592C3C9C395CCEF91AEC847CC45AD58C51CE74FCEEF56E23CBD744E105AA8";
            var result = http.PostAsJson("/webgateway.html", body);
            Debug.WriteLine(result);

            await Task.Delay(3000);

            http = new Winhttp();
            http.Domain = "https://book.lu.ink";
            http.HttpVersion2 = false;
            http.ContentType = "application/x-www-form-urlencoded";
            http.Accept = "text/html, application/xhtml+xml, */*";
            http.Headers.Add("Version", "1.1.3");
            body = "version=1.1.3&appid=23628&secretkey=FC9B7E979AA9118AB6EBBE0D939B3B2F&wtype=3&sign=368e39bd4746fbe148c8a457d83c46b0&timestamp=1698815904028&data=3E68A08A5CBA05B711DD0084C0842DDBB5C0459ED8AFAF98FF491BBDB6B92A24A5A1948447866EE2F5721E1684CEE088856DF93195D7B467DFFB52808865CC9BBFB2FFB7DC748780C348BB9DD04D85B9F745F2D1EA58D3C67A7BAE02B72DDC48122020AFE156947A78538424B2C734B95E79C87DA7D74D9DF38690C5D583991DA8749201FEB5AC95A5F71483021276F1A0B053F1A2B5585548DB7F744E853D32A03E85D4961AD4A7432B90F0322B937A3F22661E5ECACE494D9C067B73994E76C1EF3BFC8CD95EBE1864D54141DFEEF3FD6B38C7CDD116BF9CA59CDDF69417DD1500B5185E9C97D6653EC7D4F19DA086293D9BB5B5A55E96B5C55B8AEEE2310DC9541887258DBB040295A58BBE4FAF605B5F4013A7630EA87A913B98A723E2E281AAD991C91E20A3AF30888F742C08BF11B030F81D9FD4A9A6A33EAA60769BD02FD7BC698353BD72F439720EC2EDEA411A42D8E753E1F48AB0C8E5A7DC8357AE4E09CFACB80343976D7A9D3E85738A8AAE0AC8DC22FCC07BDC745083FBA1EC88";
            result = http.PostAsJson("/webgateway.html", body);
            Debug.WriteLine(result);

        }

    }
}