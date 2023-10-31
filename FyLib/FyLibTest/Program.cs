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
            Debug.WriteLine(Other.RandInt());
            Debug.WriteLine(Other.RandInt());
            Debug.WriteLine(Other.RandInt());
            Debug.WriteLine(Other.RandInt());
            Debug.WriteLine(Other.RandInt());
            Debug.WriteLine(Other.RandInt());
        }
    }
}