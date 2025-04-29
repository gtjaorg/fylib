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
            StringTest();
        }

        private static void StringTest()
        {
            //生成对StringHelper类的所有方法的测试
            var str = "127.0.0.0";
            Debug.WriteLine(str.IsIp());
            str = "^hello world^";
            Debug.WriteLine(str.Fillter());
            Debug.WriteLine(str.RemoveTag("^"));
            str = "asdkahsdjhttps://www.baidu.com";
            Debug.WriteLine(str.GetUrl());
            str = "02FE";
            Debug.WriteLine(str.ToBytes().Format());
            Debug.WriteLine(str.GetBytes().Format());
            Debug.WriteLine(str.Base64());
            Debug.WriteLine(str.IsNumeric());
            Debug.WriteLine("878990".IsNumeric());
        }
    }
}