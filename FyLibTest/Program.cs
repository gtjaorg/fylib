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
            DateTime dt = DateTime.Now;
            Debug.WriteLine(dt.DateTimeToUnixTimeStamp());
            Debug.WriteLine(dt.DateTimeToUnixTimeStampX());
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