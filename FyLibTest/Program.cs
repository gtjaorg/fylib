using FyLib;
using FyLib.NicControl;

using Newtonsoft.Json.Linq;

using System.Diagnostics;
using System.Management;

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
           var t =  demo();
            t.Wait();
        }
        static async Task demo()
        {
            Winhttp winhttp = new Winhttp();
            winhttp.HttpVersion2 = true;
            winhttp.Redirect = false;
            winhttp.Domain = "http://baidu.com";
            var result = winhttp.PostAsString("/", null);
            Debug.WriteLine(result);
            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject( winhttp.Response.RequestMessage));
            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(winhttp.Response));
        }
    }
}