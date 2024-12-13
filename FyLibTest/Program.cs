using FyLib;
using FyLib.NicControl;

using Newtonsoft.Json.Linq;

using System.Diagnostics;
using System.Management;
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
        static void Main(string[] args)
        {
            for (int i = 0; i < 30; i++)
            {
                var http = "http://aldsidle.agiso.com/Oauth/AcprLogin?code=8a39c6e22fa44b1dbe6e97aa7f523daae815d56a73904a77b072c622d6c3c91d".AsQuickHttp().setSslProtocols(System.Security.Authentication.SslProtocols.Tls13).setAutoRedirect(false);
                var task = http.GetAsStringAsync();

            }
            Console.ReadKey();
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



    }
}