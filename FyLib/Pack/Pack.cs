using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// 组包服务
    /// </summary>
    public class Pack
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
        /// 最大长度
        /// </summary>
        private int PackLen;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="len">默认长度1024</param>
        public Pack(int len = 1024)
        {
            PackLen = len;
            MS = new MemoryStream(len);
            BW = new BinaryWriter(MS);
        }

        /// <summary>
        /// 压入数据
        /// </summary>
        /// <typeparam name="T">byte short int uint long float double string byte[] hex token</typeparam>
        /// <param name="value"></param>
        public void push<T>(object value)
        {
            if (value != null)
            {
                if (typeof(T) == typeof(byte))
                {
                    push(byte.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(short))
                {
                    push(short.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(int))
                {
                    push(int.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(uint))
                {
                    push(uint.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(long))
                {
                    push(long.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(float))
                {
                    push(float.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(double))
                {
                    push(double.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(string))
                {
                    push((string)value);
                }
                else if (typeof(T) == typeof(byte[]))
                {
                    push((byte[])value);
                }
                else if (typeof(T) == typeof(Hex))
                {
                    var hex = (Hex)value;
                    push(hex.bin);
                }
                else if (typeof(T) == typeof(Token))
                {
                    var token = (Token)value;
                    push(checked((short)token.leng));
                    push(token.bin);
                }
            }
        }

        /// <summary>
        /// 压入int
        /// </summary>
        /// <param name="i"></param>
        public void push(int i)
        {
            BW.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入byte
        /// </summary>
        /// <param name="i"></param>
        public void push(byte i)
        {
            BW.Write(i);
        }

        /// <summary>
        /// 压入uint
        /// </summary>
        /// <param name="i"></param>
        public void push(uint i)
        {
            BW.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入short
        /// </summary>
        /// <param name="i"></param>
        public void push(short i)
        {
            BW.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入string
        /// </summary>
        /// <param name="value"></param>
        public void push(string value)
        {
            BW.Write(Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// 压入byte[]
        /// </summary>
        /// <param name="value"></param>
        public void push(byte[] value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 压入long
        /// </summary>
        /// <param name="value"></param>
        public void push(long value)
        {
            BW.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入float
        /// </summary>
        /// <param name="value"></param>
        public void push(float value)
        {
            BW.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入double
        /// </summary>
        /// <param name="value"></param>
        public void push(double value)
        {
            BW.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入 hex
        /// </summary>
        /// <param name="hex"></param>
        public void pushHex(string hex)
        {
            var value = hex.ToBytes();
            push(value);
        }

        /// <summary>
        /// 压入token
        /// </summary>
        /// <param name="value"></param>
        public void pushToken(byte[] value)
        {
            push(checked((short)value.Length));
            push(value);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="Clear">是否清除</param>
        /// <returns></returns>
        public byte[] Get(bool Clear = true)
        {
            if (MS == null)
            {
                return new byte[0];
            }
            var result = MS.ToArray();
            if (Clear)
            {
                MS.Position = 0L;
                MS.SetLength(0L);
            }
            return result;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Pack()
        {
            if (MS != null)
            {
                MS.Position = 0L;
                MS.SetLength(0L);
                MS.Dispose();
            }
        }
    }

}
