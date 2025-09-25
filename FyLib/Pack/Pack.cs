using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// 组包服务 - 用于将各种数据类型打包为字节流
    /// </summary>
    /// <remarks>
    /// 该类提供了一种便捷的方式将不同类型的数据（如基本数据类型、字符串、字节数组等）
    /// 按顺序打包到一个字节流中，常用于网络通信或文件存储场景。
    /// 
    /// 支持的数据类型：
    /// - 基本数值类型：byte, short, int, uint, long, float, double
    /// - 字符串：string
    /// - 字节数组：byte[]
    /// - 自定义类型：Hex, Token
    /// 
    /// 使用示例：
    /// <code>
    /// using (var pack = new Pack())
    /// {
    ///     pack.push(123);           // 压入int
    ///     pack.push("hello");       // 压入字符串
    ///     pack.push&lt;byte&gt;(255);     // 压入byte
    ///     var data = pack.Get();    // 获取打包数据
    /// }
    /// </code>
    /// </remarks>
    public class Pack : IDisposable
    {
        /// <summary>
        /// 内存流
        /// </summary>
        private MemoryStream? MS;

        /// <summary>
        /// 写内存流
        /// </summary>
        private BinaryWriter? BW;

        /// <summary>
        /// 最大长度
        /// </summary>
        private readonly int PackLen;

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
        /// 压入数据（泛型版本）
        /// </summary>
        /// <typeparam name="T">支持的数据类型：byte, short, int, uint, long, float, double, string, byte[], Hex, Token</typeparam>
        /// <param name="value">要压入的数据</param>
        /// <exception cref="ArgumentException">当类型T不受支持时抛出</exception>
        /// <exception cref="FormatException">当字符串无法解析为指定类型时抛出</exception>
        public void push<T>(T value)
        {
            if (value is null)
                return;

            // 使用模式匹配优化类型检查和处理
            switch (value)
            {
                case byte b:
                    push(b);
                    break;
                case short s:
                    push(s);
                    break;
                case int i:
                    push(i);
                    break;
                case uint ui:
                    push(ui);
                    break;
                case long l:
                    push(l);
                    break;
                case float f:
                    push(f);
                    break;
                case double d:
                    push(d);
                    break;
                case string str:
                    push(str);
                    break;
                case byte[] bytes:
                    push(bytes);
                    break;
                case Hex hex:
                    push(hex.bin);
                    break;
                case Token token:
                    push(checked((short)token.leng));
                    push(token.bin);
                    break;
                default:
                    // 尝试从字符串解析
                    if (TryParseFromString<T>(value, out var parsedValue))
                    {
                        push(parsedValue);
                    }
                    else
                    {
                        throw new ArgumentException($"不支持的数据类型: {typeof(T).Name}", nameof(value));
                    }
                    break;
            }
        }

        /// <summary>
        /// 尝试从对象的字符串表示解析为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="value">源对象</param>
        /// <param name="parsedValue">解析结果</param>
        /// <returns>是否解析成功</returns>
        private bool TryParseFromString<T>(object value, out object parsedValue)
        {
            parsedValue = null!;
            var str = value.ToString();

            if (string.IsNullOrEmpty(str))
                return false;

            var targetType = typeof(T);

            try
            {
                if (targetType == typeof(byte))
                {
                    parsedValue = byte.Parse(str);
                    return true;
                }
                else if (targetType == typeof(short))
                {
                    parsedValue = short.Parse(str);
                    return true;
                }
                else if (targetType == typeof(int))
                {
                    parsedValue = int.Parse(str);
                    return true;
                }
                else if (targetType == typeof(uint))
                {
                    parsedValue = uint.Parse(str);
                    return true;
                }
                else if (targetType == typeof(long))
                {
                    parsedValue = long.Parse(str);
                    return true;
                }
                else if (targetType == typeof(float))
                {
                    parsedValue = float.Parse(str);
                    return true;
                }
                else if (targetType == typeof(double))
                {
                    parsedValue = double.Parse(str);
                    return true;
                }

                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查对象是否已被释放
        /// </summary>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        private void ThrowIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Pack));

            if (BW == null || MS == null)
                throw new InvalidOperationException("Pack对象未正确初始化");
        }

        /// <summary>
        /// 压入数据（非泛型版本，自动检测类型）
        /// </summary>
        /// <param name="value">要压入的数据</param>
        /// <exception cref="ArgumentException">当数据类型不受支持时抛出</exception>
        /// <remarks>
        /// 该方法提供更简单的API，无需指定泛型参数，会自动根据传入对象的实际类型进行处理。
        /// 支持的类型与泛型版本完全相同。
        /// </remarks>
        public void Push(object value)
        {
            if (value is null)
                return;

            switch (value)
            {
                case byte b:
                    push(b);
                    break;
                case short s:
                    push(s);
                    break;
                case int i:
                    push(i);
                    break;
                case uint ui:
                    push(ui);
                    break;
                case long l:
                    push(l);
                    break;
                case float f:
                    push(f);
                    break;
                case double d:
                    push(d);
                    break;
                case string str:
                    push(str);
                    break;
                case byte[] bytes:
                    push(bytes);
                    break;
                case Hex hex:
                    push(hex.bin);
                    break;
                case Token token:
                    push(checked((short)token.leng));
                    push(token.bin);
                    break;
                default:
                    throw new ArgumentException($"不支持的数据类型: {value.GetType().Name}", nameof(value));
            }
        }

        /// <summary>
        /// 压入int
        /// </summary>
        /// <param name="i">要压入的整数值</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(int i)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入byte
        /// </summary>
        /// <param name="i">要压入的字节值</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(byte i)
        {
            ThrowIfDisposed();
            BW!.Write(i);
        }

        /// <summary>
        /// 压入uint
        /// </summary>
        /// <param name="i">要压入的无符号整数值</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(uint i)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入short
        /// </summary>
        /// <param name="i">要压入的短整数值</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(short i)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(i));
        }

        /// <summary>
        /// 压入string
        /// </summary>
        /// <param name="value">要压入的字符串</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(string value)
        {
            ThrowIfDisposed();
            BW!.Write(Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// 压入byte[]
        /// </summary>
        /// <param name="value">要压入的字节数组</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(byte[] value)
        {
            ThrowIfDisposed();
            BW!.Write(value);
        }

        /// <summary>
        /// 压入long
        /// </summary>
        /// <param name="value">要压入的长整数值</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(long value)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入float
        /// </summary>
        /// <param name="value">要压入的单精度浮点数</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(float value)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入double
        /// </summary>
        /// <param name="value">要压入的双精度浮点数</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void push(double value)
        {
            ThrowIfDisposed();
            BW!.Write(BytesHelper.ToBin(value));
        }

        /// <summary>
        /// 压入hex字符串
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        public void pushHex(string hex)
        {
            ThrowIfDisposed();
            var value = hex.ToBytes();
            push(value);
        }

        /// <summary>
        /// 压入token（带长度前缀的字节数组）
        /// </summary>
        /// <param name="value">要压入的字节数组</param>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        /// <remarks>
        /// 该方法会先压入数组长度（作为short），然后压入数组内容。
        /// 这种格式常用于网络协议中表示变长数据。
        /// </remarks>
        public void pushToken(byte[] value)
        {
            ThrowIfDisposed();
            push(checked((short)value.Length));
            push(value);
        }

        /// <summary>
        /// 获取全部打包数据
        /// </summary>
        /// <param name="Clear">是否在获取后清除缓冲区，默认为true</param>
        /// <returns>返回打包的字节数组</returns>
        /// <exception cref="ObjectDisposedException">当对象已被释放时抛出</exception>
        /// <remarks>
        /// 该方法返回当前缓冲区中的所有数据。如果Clear参数为true（默认值），
        /// 调用后会清空缓冲区，准备下次打包。如果为false，则保留缓冲区内容。
        /// </remarks>
        public byte[] Get(bool Clear = true)
        {
            ThrowIfDisposed();

            if (MS == null)
            {
                return [];
            }

            var result = MS.ToArray();
            if (Clear)
            {
                MS.Position = 0L;
                MS.SetLength(0L);
            }
            return result;
        }

        #region IDisposable 实现

        private bool disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        /// <param name="disposing">是否正在处置托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    BW?.Dispose();
                    if (MS != null)
                    {
                        MS.Position = 0L;
                        MS.SetLength(0L);
                        MS.Dispose();
                    }
                }

                // 释放非托管资源（如果有的话）
                // ...

                disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Pack()
        {
            Dispose(false);
        }

        #endregion

    }

}
