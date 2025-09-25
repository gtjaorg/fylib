using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FyLib.FyLib
{
    /// <summary>
    /// 远程进程管理
    /// </summary>
    public class RemoteProcess
    {
        /// <summary>
        /// 进程信息
        /// </summary>
        public required Process Process { get; set; }
        /// <summary>
        /// 进程句柄
        /// </summary>
        public nint ProcessHandle { get; private set; }


        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProcessModule? GetModule(string name)
        {
            var module = Process.Modules.Cast<ProcessModule>().Where(a => a.ModuleName == "WeChatWin.dll").FirstOrDefault();
            return module;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~RemoteProcess()
        {
            if (this.ProcessHandle != IntPtr.Zero)
            {
                kernel32.CloseHandle(this.ProcessHandle);
            }
        }
        /// <summary>
        /// 读取权限
        /// </summary>
        public const int PROCESS_VM_READ = 0x0010;
        /// <summary>
        /// 写入权限
        /// </summary>
        public const int PROCESS_VM_WRITE = 0x0020;
        /// <summary>
        /// 执行操作
        /// </summary>
        public const int PROCESS_VM_OPERATION = 0x0008;
        /// <summary>
        /// 打开进程, 必须先打开进程才能进行内存读写操作
        /// </summary>
        /// <param name="DesiredAccess"></param>
        /// <param name="InheritHandle"></param>
        /// <returns></returns>
        public bool OpenProcess(int DesiredAccess = PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, bool InheritHandle = false)
        {
            this.ProcessHandle = kernel32.OpenProcess(DesiredAccess, InheritHandle, Process.Id);
            return this.ProcessHandle != IntPtr.Zero;
        }
        /// <summary>
        /// 读取内存整数
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ReadMemory(nint address)
        {
            var buffer = new byte[4]; // 读取的数据存放在这里
            var bytesRead = 0;
            if (kernel32.ReadProcessMemory(this.ProcessHandle, address, buffer, buffer.Length, out bytesRead))
            {
                var value = BitConverter.ToInt32(buffer, 0); // 将读取的字节转换为整数
                return value;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 读取内存数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[]? ReadMemory(nint address, int len)
        {
            var buffer = new byte[len]; // 读取的数据存放在这里
            var bytesRead = 0;
            if (kernel32.ReadProcessMemory(this.ProcessHandle, address, buffer, buffer.Length, out bytesRead))
            {
                return buffer;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 读取Unicode文本
        /// </summary>
        /// <param name="add"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string ReadUnicode(nint add, int len = 100)
        {
            var data = ReadMemory(add, len);
            if (data == null) return "";
            int FindIndex(byte[] array)
            {
                for (var i = 0; i < array.Length - 1; i += 2)
                {
                    if (array[i] == 0x00 && i + 1 < array.Length && array[i + 1] == 0x00)
                    {
                        return i;
                    }
                }
                return array.Length; // 如果未找到连续的 00 00 序列，则返回数组的长度
            }
            var index = FindIndex(data);// 查找第一个 00 00 字节的索引
            var truncatedData = new byte[index]; // 创建一个新的字节数组，长度为第一个 00 字节的索引
            Array.Copy(data, truncatedData, index);

            var str = Encoding.Unicode.GetString(truncatedData);
            return str;
        }

        /// <summary>
        /// 申请内存
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public nint VirtualAllocEx(uint len = 1024)
        {
            var add = kernel32.VirtualAllocEx(this.ProcessHandle, 0, len, kernel32.MEM_COMMIT, 64);
            return add;
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="add"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public bool FreeAllocEx(nint add, uint len = 0)
        {
            return kernel32.VirtualFreeEx(ProcessHandle, add, len, kernel32.MEM_RELEASE);
        }
        /// <summary>
        /// 写内存整数型
        /// </summary>
        /// <param name="add"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteMemory(nint add, int value)
        {
            var buff = BitConverter.GetBytes(value);
            var b = kernel32.WriteProcessMemory(ProcessHandle, add, buff, 4, out var len);
            return b;
        }
        /// <summary>
        /// 写内存字节集
        /// </summary>
        /// <param name="add"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteMemory(nint add, byte[] value)
        {
            var b = kernel32.WriteProcessMemory(ProcessHandle, add, value, (uint)value.Length, out var ok);
            return b;
        }
        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="address"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public nint CreadThread(nint address, nint param = 0)
        {
            return kernel32.CreateRemoteThread(ProcessHandle, IntPtr.Zero, 0, address, param, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 关闭句柄
        /// </summary>
        /// <returns></returns>
        public int CloseHandle()
        {
            return kernel32.CloseHandle(this.ProcessHandle);
        }
    }
}
