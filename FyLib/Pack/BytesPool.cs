using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// ByteArray内存池
    /// </summary>
    public class ByteArrayPool
    {
        private ArrayBufferWriter<byte>? writer;

        private int pos;

        /// <summary>
        /// 内存池数据长度
        /// </summary>
        public int Length
        {
            get
            {
                if (writer == null)
                {
                    return 0;
                }
                return writer.WrittenCount;
            }
        }

        /// <summary>
        /// 可读长度
        /// </summary>
        public int CanReadLength
        {
            get
            {
                if (writer == null)
                {
                    return 0;
                }
                return checked(writer.WrittenCount - pos);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="len">默认长度 1024*1024</param>
        public void Init(int len = 1048576)
        {
            if (writer != null)
            {
                writer.Clear();
            }
            writer = new ArrayBufferWriter<byte>(len);
            pos = 0;
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(Span<byte> value)
        {
            if (writer == null)
            {
                Init();
            }
            if (writer == null)
            {
                return false;
            }
            if (writer.FreeCapacity < value.Length)
            {
                var writtenSpan = writer.WrittenSpan;
                var num = pos;
                var array = writtenSpan.Slice(num, writer.WrittenCount - num).ToArray();
                Init();
                writer.Write(array.AsSpan());
                pos = 0;
            }
            try
            {
                writer?.Write(value);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(byte[] value)
        {
            return Write(value.AsSpan());
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="len"></param>
        public void Back(int len)
        {
            checked
            {
                pos -= len;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public ReadOnlySpan<byte> Read(int len)
        {
            var num = checked(pos + len);
            if (writer == null)
            {
                Init();
                return null;
            }
            if (num > writer.WrittenCount)
            {
                return null;
            }
            try
            {
                var result = writer.WrittenSpan;
                var num2 = pos;
                var readOnlySpan = result.Slice(num2, num - num2);
                pos = num;
                result = readOnlySpan;
                return result;
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// 读取byte[]
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public byte[] ReadBytes(int len)
        {
            var readOnlySpan = Read(len);
            if (readOnlySpan.IsEmpty)
            {
                return [];
            }
            return readOnlySpan.ToArray();
        }

        /// <summary>
        /// 释放内存池
        /// </summary>
        public void Free()
        {
            writer?.Clear();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ByteArrayPool()
        {
            Free();
        }
    }

}
