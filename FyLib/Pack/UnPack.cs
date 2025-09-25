using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// 解包服务-输入
    /// </summary>
    public class Unpack
    {
        /// <summary>
        /// 内存流
        /// </summary>
        private MemoryStream MS;

        /// <summary>
        /// 写内存流
        /// </summary>
        private BinaryWriter BW;

        /// <summary>
        /// 当前位置
        /// </summary>
        public long Position => MS.Position;

        /// <summary>
        /// 数据流长度
        /// </summary>
        public long Length => MS.Length;

        /// <summary>
        /// 初始化一个PackInputStream类的新实例
        /// </summary>
        public Unpack()
        {
            if (MS != null)
            {
                Close();
            }
            MS = new MemoryStream();
            BW = new BinaryWriter(MS);
        }

        /// <summary>
        /// 初始化一个PackInputStream类的新实例
        /// </summary>
        public Unpack(byte[] buff)
        {
            MS = new MemoryStream(buff.Length);
            BW = new BinaryWriter(MS);
            BW.Write(buff);
            MS.Position = 0L;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">16进制文本</param>
        public Unpack(string value)
        {
            var array = value.ToBytes();
            MS = new MemoryStream(array.Length);
            BW = new BinaryWriter(MS);
            BW.Write(array);
            MS.Position = 0L;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~Unpack()
        {
            Clear();
            MS.Close();
            BW.Close();
        }

        /// <summary>
        /// 设置数据流
        /// </summary>
        /// <param name="buff"></param>
        public void SetData(byte[] buff)
        {
            Close();
            MS = new MemoryStream(buff.Length);
            BW = new BinaryWriter(MS);
            BW.Write(buff);
            MS.Position = 0L;
        }

        /// <summary>
        /// 获取剩余数据
        /// </summary>
        /// <param name="type">是否忽略03</param>
        /// <returns></returns>
        public byte[] GetData(bool type = false)
        {
            checked
            {
                if (type)
                {
                    return GetBin((int)(Length - Position - 1));
                }
                return GetBin((int)(Length - Position));
            }
        }

        /// <summary>
        /// 读取byte
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
        {
            var array = new byte[1];
            MS.Read(array, 0, 1);
            return array[0];
        }

        /// <summary>
        /// 读取Short
        /// </summary>
        /// <returns></returns>
        public short GetShort()
        {
            var array = new byte[2];
            MS.Read(array, 0, 2);
            return BytesHelper.ToShort(array);
        }

        /// <summary>
        /// 读取Uint
        /// </summary>
        /// <returns></returns>
        public uint GetUint()
        {
            var array = new byte[4];
            MS.Read(array, 0, 4);
            return array.ToUInt();
        }

        /// <summary>
        /// 读取Long
        /// </summary>
        /// <returns></returns>
        public long GetLong()
        {
            var array = new byte[8];
            MS.Read(array, 0, 8);
            return array.ToLong();
        }

        /// <summary>
        /// 读取Int
        /// </summary>
        /// <returns></returns>
        public int GetInt()
        {
            var array = new byte[4];
            MS.Read(array, 0, 4);
            return array.ToInt();
        }

        /// <summary>
        /// 读取bytes
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public byte[] GetBin(int len)
        {
            if (len > checked(MS.Length - MS.Position))
            {
                return new byte[0];
            }
            var array = new byte[len];
            MS.Read(array, 0, len);
            return array;
        }

        /// <summary>
        /// 获取一个token, token格式为[short]Length+data
        /// </summary>
        /// <returns></returns>
        public byte[] GetToken()
        {
            var @short = GetShort();
            return GetBin(@short);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">数据类型: byte short int uint Token byte[]</typeparam>
        /// <param name="len">当类型为byte[]时 必填</param>
        /// <returns></returns>
        public T? Get<T>(int len = 0)
        {
            var result = default(T);
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)GetByte();
            }
            if (typeof(T) == typeof(short))
            {
                return (T)(object)GetShort();
            }
            if (typeof(T) == typeof(int))
            {
                return (T)(object)GetInt();
            }
            if (typeof(T) == typeof(uint))
            {
                return (T)(object)GetUint();
            }
            if (typeof(T) == typeof(long))
            {
                return (T)(object)GetLong();
            }
            if (typeof(T) == typeof(byte[]))
            {
                return (T)(object)GetBin(len);
            }
            if (typeof(T) == typeof(Token))
            {
                len = GetShort();
                return (T)(object)new Token(GetBin(len));
            }
            return result;
        }

        /// <summary>
        /// 清空Pack
        /// </summary>
        public void Clear()
        {
            MS.Position = 0L;
            MS.SetLength(0L);
        }

        /// <summary>
        /// 关闭，并释放所有占用的资源
        /// </summary>
        private void Close()
        {
            Clear();
            if (MS != null)
            {
                MS.Dispose();
            }
            if (BW != null)
            {
                BW.Dispose();
            }
        }
    }
}
