using FyLib;

using System.Diagnostics;

namespace FyLibTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Winhttp winhttp = new Winhttp();
            //winhttp.HttpVersion2 = true;
            //winhttp.Domain = "https://www.qq.com";
            //winhttp.GetAsString();
            //Debug.WriteLine(winhttp.Response.Version);
            demo();
        }
        static async void demo()
        {
            Debug.WriteLine(IPHelper.GetNetIP());
            Debug.WriteLine(IPHelper.GetLocalIP());
            string ip = IPHelper.GetLocalIP();
            var ls = IPHelper.GetAllAddress(ip);
            foreach (var item in ls)
            {
                //Debug.WriteLine(item);
            }
            ls = IPHelper.GetAllAddress("192.168.3.21","192.168.3.200");
            foreach (var item in ls)
            {
                Debug.WriteLine(item);
            }
            Debug.WriteLine("c:\\1.png".GetLength());
        }
    }
}