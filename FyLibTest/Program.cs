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
        static async Task Main(string[] args)
        {
            //Winhttp winhttp = new Winhttp();
            //winhttp.HttpVersion2 = true;
            //winhttp.Domain = "https://www.qq.com";
            //winhttp.GetAsString();
            //Debug.WriteLine(winhttp.Response.Version);
            //var t = IPHelper.GetLocalIPAddressBase();
            //Debug.WriteLine(t);
            //Debug.WriteLine(IPHelper.GetLocalIP());
            //Debug.WriteLine(await IPHelper.IsHostPingedAsync("192.168.3.3", 300));
            //Debug.WriteLine(Other.GetMachineCode());

            var ls =  Process.GetProcessesByName("WeChat");
            if (ls.Length == 0) return;
            ls = ls.OrderByDescending(a=>a.Id).ToArray();
            var p = ls[0];
            var pid = ls[0].Id;
            var module = p.Modules.Cast<ProcessModule>().Where(a => a.ModuleName == "WeChatWin.dll").FirstOrDefault();
            if(module == null) return;
            var baseAddress = module.BaseAddress;

            
            Debug.WriteLine(baseAddress);
            RemoteProcess remoteProcess = new() { Process = p};
            var b = remoteProcess.openProcess();
            if (!b)
            {
                Debug.WriteLine("打开进程失败!");
                return;
            }

            #region 获取好友
            var move = baseAddress + "222F3BC".HexToInt();
            var endAddress = remoteProcess.readMemory(move);
            endAddress = endAddress + "48".HexToInt();
            endAddress = endAddress + "4".HexToInt();
            var enterAddress = remoteProcess.readMemory(endAddress);
            Debug.WriteLine(endAddress);
            var friendCount = remoteProcess.readMemory(endAddress + 4);
            Debug.WriteLine(friendCount);
            var leftAddress = remoteProcess.readMemory(enterAddress);
            var rightAddress = remoteProcess.readMemory(enterAddress + 4);
            Debug.WriteLine($"{leftAddress} -> {rightAddress}");



            void readFriend(int left, int right)
            {
                WxFriendInfo info = new WxFriendInfo();
                var add = remoteProcess.readMemory(left + "30".HexToInt());
                info.id = remoteProcess.readUnicode(add);

                add = remoteProcess.readMemory(left + "44".HexToInt());
                info.name = remoteProcess.readUnicode(add);
                add = remoteProcess.readMemory(left + "58".HexToInt());
                info.v3 = remoteProcess.readUnicode(add);
                add = remoteProcess.readMemory(left + "78".HexToInt());
                info.tag = remoteProcess.readUnicode(add);
                add = remoteProcess.readMemory(left + "8C".HexToInt());
                info.nick = remoteProcess.readUnicode(add);
                add = remoteProcess.readMemory(left + "11C".HexToInt());
                info.head = remoteProcess.readUnicode(add);
                Debug.WriteLine(info);
                left = remoteProcess.readMemory(left);
                if (left != right)
                {
                    readFriend(left, right);
                }
            }
            readFriend(leftAddress, rightAddress);
            #endregion
        

            #region 打开内置浏览器

            
            var call1 = baseAddress + "701DC0".HexToInt();
            var call2 = baseAddress + "C34EC0".HexToInt();
            var callAddress = remoteProcess.VirtualAllocEx();
            remoteProcess.writeMemory(callAddress, (int)call1);
            remoteProcess.writeMemory(callAddress+4, (int)call2);

            var urlAddress = remoteProcess.VirtualAllocEx();
            string url = "https://mp.weixin.qq.com/s?__biz=MzI2OTA3MTE3OQ==&mid=2651439404&idx=1&sn=b4cbfddca0622305a9f39107175f9793&scene=0#wechat_redirect";
            var bin = url.GetBytes(Encoding.Unicode);
            remoteProcess.writeMemory(urlAddress , (int)urlAddress+24);
            remoteProcess.writeMemory(urlAddress + 4, url.Length);
            remoteProcess.writeMemory(urlAddress + 8, url.Length);
            remoteProcess.writeMemory(urlAddress +12, 0);
            remoteProcess.writeMemory(urlAddress + 16, 0);
            remoteProcess.writeMemory(urlAddress+24, bin);

            Debug.WriteLine(BytesHelper.ToBin((int)call1).Flip().Format ());
            Debug.WriteLine(BytesHelper.ToBin((int)call1 + 4).Flip().Format());
            Debug.WriteLine(BytesHelper.ToBin((int)urlAddress).Flip().Format());

            string code = $"""
                                60 83 EC 14 8B CC B8 {BytesHelper.ToBin((int)urlAddress).Flip().Format()} 50 FF 15 {BytesHelper.ToBin((int)callAddress).Flip().Format()} FF 15 {BytesHelper.ToBin((int)callAddress + 4).Flip().Format()} 83 C4 14 61 C3 
                """;
            var memAddress = remoteProcess.VirtualAllocEx(4096);
            remoteProcess.writeMemory(memAddress,code.ToBytes());

            var h =  remoteProcess.creadThread(memAddress, 0);
            kernel32.WaitForSingleObject(h, 2000);
            kernel32.CloseHandle(h);
            remoteProcess.FreeAllocEx(memAddress);
            remoteProcess.FreeAllocEx(urlAddress);
            remoteProcess.FreeAllocEx(callAddress);
            #endregion


            #region 发送消息
            var msg = "caonima123";
            var recvID = "q3231308";
            var msgAddress = remoteProcess.VirtualAllocEx(4096);
            code = $"""
                B8 
                { BytesHelper.ToBin ((int)msgAddress+80).Flip().Format()}
                6A 01 50 BF 
                {BytesHelper.ToBin((int)msgAddress + 100).Flip().Format()}
                57 BA 
                {BytesHelper.ToBin((int)msgAddress + 1000).Flip().Format()}
                B9 
                {BytesHelper.ToBin((int)msgAddress + 3000).Flip().Format()}
                BB B0 E7 40 7A FF D3 83 C4 0C C3
                """;
            bin = code.ToBytes();
            remoteProcess.writeMemory(msgAddress, bin);
            remoteProcess.writeMemory(msgAddress + 80, 0);
            remoteProcess.writeMemory(msgAddress + 84, 0);
            remoteProcess.writeMemory(msgAddress + 88, 0);

            bin = msg.GetBytes(Encoding.Unicode);
            remoteProcess.writeMemory(msgAddress + 100, (int)msgAddress + 200);
            remoteProcess.writeMemory(msgAddress + 104, bin.Length);
            remoteProcess.writeMemory(msgAddress + 108, bin.Length);
            remoteProcess.writeMemory(msgAddress + 112, 0);
            remoteProcess.writeMemory(msgAddress + 116, 0);
            remoteProcess.writeMemory(msgAddress + 200, bin);


            bin = recvID.GetBytes(Encoding.Unicode);
            remoteProcess.writeMemory(msgAddress + 1000, (int)msgAddress + 2000);
            remoteProcess.writeMemory(msgAddress + 1004, bin.Length);
            remoteProcess.writeMemory(msgAddress + 1008, bin.Length);
            remoteProcess.writeMemory(msgAddress + 1012, 0);
            remoteProcess.writeMemory(msgAddress + 1016, 0);

            remoteProcess.writeMemory(msgAddress + 2000, bin);
            remoteProcess.writeMemory(msgAddress + 3000, 2);

            h= remoteProcess.creadThread(msgAddress);
            kernel32.WaitForSingleObject(h, 2000);
            kernel32.CloseHandle(h);

            remoteProcess.FreeAllocEx(msgAddress);
            #endregion
            return;
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