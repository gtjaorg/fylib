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
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var t = Task.Run(async () => {

                var http = "https://www.baidu.com".AsQuickHttp();
                http = http.SetProxy("218.95.39.53", 11508);
                var str = await http.GetAsStringAsync();
                Debug.WriteLine(str);
            });
            t.Wait();
        }

        private static void StringTest()
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



    }
}