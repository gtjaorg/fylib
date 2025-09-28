using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 字节数组扩展方法
/// </summary>
public static class BytesExtension
{
    /// <summary>
    /// 短整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">短整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(short value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 无符号短整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">无符号短整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(int value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 无符号整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">无符号整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 长整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">长整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(long value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 无符号长整数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">无符号长整数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 单精度浮点数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">单精度浮点数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(float value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 双精度浮点数到字节数组（带字节序反转）
    /// </summary>
    /// <param name="value">双精度浮点数值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(double value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 字符串转换为字节数组（使用默认编码）
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <returns>字节数组</returns>
    public static byte[] ToBin(string value)
    {
        return Encoding.Default.GetBytes(value);
    }

    /// <summary>
    /// IP地址字符串转换为字节数组
    /// </summary>
    /// <param name="ipStr">IP地址字符串，格式为"x.x.x.x"</param>
    /// <returns>4字节的字节数组，如果格式无效返回null</returns>
    public static byte[]? IP2Bin(string ipStr)
    {
        if (string.IsNullOrEmpty(ipStr))
            return null;

        var parts = ipStr.Split('.');
        if (parts.Length != 4)
            return null;

        var result = new byte[4];
        for (var i = 0; i < 4; i++)
        {
            if (!byte.TryParse(parts[i], out var value))
                return null;
            result[3 - i] = value; // 反向存储以匹配原逻辑
        }

        return result;
    }

    /// <summary>
    /// 提供字节数组的扩展方法
    /// </summary>
    extension(byte[] source)
    {
        /// <summary>
        /// 获取字节数组的MD5哈希值的十六进制字符串表示形式，结果转换为小写格式
        /// </summary>
        /// <returns>MD5哈希值的十六进制字符串，格式为小写</returns>
        /// <remarks>
        /// 该扩展方法通过对字节数组计算MD5哈希值，然后将其转换为十六进制字符串表示形式。
        /// 结果字符串采用小写格式，便于在需要时进行比较或显示。
        /// </remarks>
        public string md5 => source.MD5.ToLower();

        /// <summary>
        /// 获取字节数组的MD5哈希值的十六进制字符串表示形式
        /// </summary>
        /// <returns>MD5哈希值的十六进制字符串，格式为大写</returns>
        /// <remarks>
        /// 该扩展方法通过对字节数组计算MD5哈希值，然后将其转换为十六进制字符串表示形式。
        /// 结果字符串采用大写格式，便于在需要时进行比较或显示。
        /// </remarks>
        public string MD5
        {
            get
            {
                using var md5 = MD5.Create();
                var hash = md5.ComputeHash(source);
                return Convert.ToHexString(hash);
            }
        }
        /// <summary>
        /// 获取字节数组的MD5哈希值的字节数组表示形式
        /// </summary>
        /// <returns>MD5哈希值的字节数组</returns>
        /// <remarks>
        /// 该扩展方法通过对字节数组计算MD5哈希值，并返回其字节数组表示形式。
        /// 结果字节数组长度为16个字节，适用于需要原始哈希值的场景。
        /// </remarks>
        public byte[] Md5()
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(source);
        }

        /// <summary>
        /// 获取字节数组转换为十六进制字符串表示形式的结果
        /// </summary>
        /// <returns>字节数组的十六进制字符串表示形式，各字节以两位十六进制数表示并连接</returns>
        /// <remarks>
        /// 该扩展方法将字节数组中的每个字节转换为两位的十六进制字符串表示形式，
        /// 并将所有字节的结果按顺序连接成一个完整的十六进制字符串。
        /// 结果字符串中的每个字节都使用大写格式表示。
        /// </remarks>
        public string Hex
        {
            get
            {
                var stringBuilder = new StringBuilder(checked(source.Length * 2));
                try
                {
                    foreach (var b in source)
                    {
                        stringBuilder.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return stringBuilder.ToString().ToUpper();
            }
        }
        /// <summary>
        /// 获取字节数组格式化后的十六进制字符串表示形式，按每行16个字节进行换行分组
        /// </summary>
        /// <returns>格式化后的十六进制字符串，每行显示16个字节，每个字节用两个大写十六进制字符表示</returns>
        /// <remarks>
        /// 该扩展方法将字节数组转换为十六进制字符串表示形式，并按照每16个字节进行换行分组，
        /// 便于查看和调试。每个字节使用两个大写十六进制字符表示，各字节之间用空格分隔。
        /// </remarks>
        public string FormatHex
        {
            get
            {
                var text = source.Hex;
                var length = text.Length;
                var text2 = "";
                for (var i = 0; i < length / 2; i = checked(i + 1))
                {
                    if (i % 16 == 0 && i != 0)
                    {
                        text2 += "\r\n";
                    }
                    text2 = string.Concat(text2, text.AsSpan(checked(i * 2), 2), " ");
                }
                return text2;
            }
        }

        /// <summary>
        /// 获取字节数组转换为Base64格式的字符串表示形式
        /// </summary>
        /// <returns>表示字节数组的Base64格式字符串</returns>
        /// <remarks>
        /// 该扩展方法将字节数组转换为Base64编码的字符串，便于在文本环境中传输或存储二进制数据。
        /// Base64编码是一种常用的编码方式，能够将二进制数据转换为ASCII字符集中的可打印字符。
        /// </remarks>
        public string Base64
        {
            get
            {
                return Convert.ToBase64String(source);
            }
        }

        /// <summary>
        /// 获取SHA-1哈希值的十六进制字符串表示形式
        /// </summary>
        /// <returns>SHA-1哈希值的十六进制字符串，格式为大写</returns>
        public string SHA1
        {
            get
            {
                using var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(source);
                return Convert.ToHexString(hash);
            }
        }

        /// <summary>
        /// 获取SHA-256哈希值的十六进制字符串表示形式
        /// </summary>
        /// <returns>SHA-256哈希值的十六进制字符串，格式为大写</returns>
        public string SHA256
        {
            get
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var hash = sha256.ComputeHash(source);
                return Convert.ToHexString(hash);
            }
        }

        /// <summary>
        /// 获取SHA-512哈希值的十六进制字符串表示形式
        /// </summary>
        /// <returns>SHA-512哈希值的十六进制字符串，格式为大写</returns>
        public string SHA512
        {
            get
            {
                using var sha512 = System.Security.Cryptography.SHA512.Create();
                var hash = sha512.ComputeHash(source);
                return Convert.ToHexString(hash);
            }
        }

        /// <summary>
        /// 使用UTF-8编码将字节数组转换为字符串
        /// </summary>
        /// <returns>UTF-8编码的字符串</returns>
        public string ToUtf8String => Encoding.UTF8.GetString(source);

        /// <summary>
        /// 使用指定编码将字节数组转换为字符串
        /// </summary>
        /// <param name="encoding">字符编码</param>
        /// <returns>指定编码的字符串</returns>
        public string ToString(Encoding encoding) => encoding.GetString(source);

        /// <summary>
        /// 使用Gzip算法压缩字节数组
        /// </summary>
        /// <returns>压缩后的字节数组</returns>
        public byte[] GzipCompress()
        {
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(source, 0, source.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 使用Gzip算法解压缩字节数组
        /// </summary>
        /// <returns>解压缩后的字节数组</returns>
        public byte[] GzipDecompress()
        {
            using var compressedStream = new MemoryStream(source);
            using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            gzipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        /// <summary>
        /// 使用Deflate算法压缩字节数组
        /// </summary>
        /// <returns>压缩后的字节数组</returns>
        public byte[] DeflateCompress()
        {
            using var memoryStream = new MemoryStream();
            using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
            {
                deflateStream.Write(source, 0, source.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 使用Deflate算法解压缩字节数组
        /// </summary>
        /// <returns>解压缩后的字节数组</returns>
        public byte[] DeflateDecompress()
        {
            using var compressedStream = new MemoryStream(source);
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            deflateStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        /// <summary>
        /// 计算CRC32校验值
        /// </summary>
        /// <returns>CRC32校验值</returns>
        public uint CRC32
        {
            get
            {
                const uint polynomial = 0xEDB88320;
                var table = new uint[256];

                // 初始化CRC表
                for (uint i = 0; i < 256; i++)
                {
                    var entry = i;
                    for (var j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ polynomial;
                        else
                            entry >>= 1;
                    }
                    table[i] = entry;
                }

                // 计算CRC32
                uint crc = 0xFFFFFFFF;
                foreach (var b in source)
                {
                    crc = (crc >> 8) ^ table[(crc ^ b) & 0xFF];
                }
                return crc ^ 0xFFFFFFFF;
            }
        }

        /// <summary>
        /// 获取字节数组的长度
        /// </summary>
        /// <returns>字节数组长度</returns>
        public int Length => source.Length;

        /// <summary>
        /// 判断字节数组是否为空
        /// </summary>
        /// <returns>如果为空返回true，否则返回false</returns>
        public bool IsEmpty => source == null || source.Length == 0;

        /// <summary>
        /// 判断字节数组是否不为空
        /// </summary>
        /// <returns>如果不为空返回true，否则返回false</returns>
        public bool IsNotEmpty => source != null && source.Length > 0;

        /// <summary>
        /// 获取字节数组的子数组
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>子字节数组</returns>
        public byte[] Slice(int start, int length)
        {
            if (start < 0 || start >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (length < 0 || start + length > source.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            var result = new byte[length];
            Array.Copy(source, start, result, 0, length);
            return result;
        }

        /// <summary>
        /// 获取字节数组的子数组（从指定位置到末尾）
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <returns>子字节数组</returns>
        public byte[] SliceFrom(int start)
        {
            if (start < 0 || start >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(start));

            var length = source.Length - start;
            var result = new byte[length];
            Array.Copy(source, start, result, 0, length);
            return result;
        }

        /// <summary>
        /// 连接两个字节数组
        /// </summary>
        /// <param name="other">要连接的字节数组</param>
        /// <returns>连接后的字节数组</returns>
        public byte[] Concat(byte[] other)
        {
            if (other == null)
                return source;

            var result = new byte[source.Length + other.Length];
            Array.Copy(source, 0, result, 0, source.Length);
            Array.Copy(other, 0, result, source.Length, other.Length);
            return result;
        }

        /// <summary>
        /// 查找指定字节的第一个索引位置
        /// </summary>
        /// <param name="value">要查找的字节值</param>
        /// <returns>第一个匹配的索引位置，如果未找到返回-1</returns>
        public int IndexOf(byte value)
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] == value)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 查找指定字节序列的第一个索引位置
        /// </summary>
        /// <param name="pattern">要查找的字节序列</param>
        /// <returns>第一个匹配的索引位置，如果未找到返回-1</returns>
        public int IndexOf(byte[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
                return -1;

            for (var i = 0; i <= source.Length - pattern.Length; i++)
            {
                var match = true;
                for (var j = 0; j < pattern.Length; j++)
                {
                    if (source[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 判断是否包含指定的字节值
        /// </summary>
        /// <param name="value">要检查的字节值</param>
        /// <returns>如果包含返回true，否则返回false</returns>
        public bool Contains(byte value)
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否包含指定的字节序列
        /// </summary>
        /// <param name="pattern">要检查的字节序列</param>
        /// <returns>如果包含返回true，否则返回false</returns>
        public bool Contains(byte[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
                return false;

            for (var i = 0; i <= source.Length - pattern.Length; i++)
            {
                var match = true;
                for (var j = 0; j < pattern.Length; j++)
                {
                    if (source[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 反转字节数组
        /// </summary>
        /// <returns>反转后的字节数组</returns>
        public byte[] Reverse()
        {
            var result = new byte[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                result[i] = source[source.Length - 1 - i];
            }
            return result;
        }

        /// <summary>
        /// 将字节数组保存到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void SaveToFile(string filePath)
        {
            File.WriteAllBytes(filePath, source);
        }

        /// <summary>
        /// 异步将字节数组保存到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>异步任务</returns>
        public async System.Threading.Tasks.Task SaveToFileAsync(string filePath)
        {
            await File.WriteAllBytesAsync(filePath, source);
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="other">要比较的字节数组</param>
        /// <returns>如果相等返回true，否则返回false</returns>
        public bool SequenceEqual(byte[] other)
        {
            if (other == null || source.Length != other.Length)
                return false;

            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] != other[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 使用XOR操作对字节数组进行异或运算
        /// </summary>
        /// <param name="key">异或密钥</param>
        /// <returns>异或运算后的字节数组</returns>
        public byte[] Xor(byte[] key)
        {
            if (key == null || key.Length == 0)
                return source;

            var result = new byte[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                result[i] = (byte)(source[i] ^ key[i % key.Length]);
            }
            return result;
        }

        /// <summary>
        /// 使用XOR操作对字节数组进行异或运算（单字节密钥）
        /// </summary>
        /// <param name="key">异或密钥</param>
        /// <returns>异或运算后的字节数组</returns>
        public byte[] Xor(byte key)
        {
            var result = new byte[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                result[i] = (byte)(source[i] ^ key);
            }
            return result;
        }

        /// <summary>
        /// 将2位字节数组转换为短整数（带字节序反转）
        /// </summary>
        /// <returns>短整数值</returns>
        public short ToShort()
        {
            if (source.Length != 2)
                throw new ArgumentException("字节数组长度必须为2", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToInt16(reversed, 0);
        }

        /// <summary>
        /// 将2位字节数组转换为无符号短整数（带字节序反转）
        /// </summary>
        /// <returns>无符号短整数值</returns>
        public ushort ToUShort()
        {
            if (source.Length != 2)
                throw new ArgumentException("字节数组长度必须为2", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToUInt16(reversed, 0);
        }

        /// <summary>
        /// 将字节数组转换为整数（支持字节序选择）
        /// </summary>
        /// <param name="isFlip">是否反转字节序，默认为true</param>
        /// <returns>整数值</returns>
        public int ToInt(bool isFlip = true)
        {
            var array = new byte[source.Length];
            Array.Copy(source, array, source.Length);

            if (isFlip)
            {
                Array.Reverse(array);
            }

            return array.Length switch
            {
                2 => BitConverter.ToUInt16(array, 0),
                4 => BitConverter.ToInt32(array, 0),
                _ => throw new ArgumentException($"不支持的字节数组长度: {array.Length}", nameof(source))
            };
        }

        /// <summary>
        /// 将4位字节数组转换为无符号整数（带字节序反转）
        /// </summary>
        /// <returns>无符号整数值</returns>
        public uint ToUInt()
        {
            if (source.Length != 4)
                throw new ArgumentException("字节数组长度必须为4", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToUInt32(reversed, 0);
        }

        /// <summary>
        /// 将8位字节数组转换为长整数（带字节序反转）
        /// </summary>
        /// <returns>长整数值</returns>
        public long ToLong()
        {
            if (source.Length != 8)
                throw new ArgumentException("字节数组长度必须为8", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToInt64(reversed, 0);
        }

        /// <summary>
        /// 将8位字节数组转换为无符号长整数（带字节序反转）
        /// </summary>
        /// <returns>无符号长整数值</returns>
        public ulong ToULong()
        {
            if (source.Length != 8)
                throw new ArgumentException("字节数组长度必须为8", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToUInt64(reversed, 0);
        }

        /// <summary>
        /// 将4位字节数组转换为单精度浮点数（带字节序反转）
        /// </summary>
        /// <returns>单精度浮点数值</returns>
        public float ToFloat()
        {
            if (source.Length != 4)
                throw new ArgumentException("字节数组长度必须为4", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToSingle(reversed, 0);
        }

        /// <summary>
        /// 将8位字节数组转换为双精度浮点数（带字节序反转）
        /// </summary>
        /// <returns>双精度浮点数值</returns>
        public double ToDouble()
        {
            if (source.Length != 8)
                throw new ArgumentException("字节数组长度必须为8", nameof(source));

            var reversed = source.Reverse();
            return BitConverter.ToDouble(reversed, 0);
        }

        /// <summary>
        /// 使用默认编码将字节数组转换为字符串
        /// </summary>
        /// <returns>转换后的字符串</returns>
        public string ToStr() => Encoding.Default.GetString(source);

        /// <summary>
        /// 使用指定编码将字节数组转换为字符串
        /// </summary>
        /// <param name="encoding">字符编码</param>
        /// <returns>转换后的字符串</returns>
        public string ToStr(Encoding encoding) => encoding.GetString(source);

        /// <summary>
        /// 将4位字节数组转换为IP地址字符串
        /// </summary>
        /// <returns>IP地址字符串，格式为"x.x.x.x"</returns>
        public string ToIP()
        {
            if (source.Length != 4)
                return string.Empty;

            return $"{source[0]}.{source[1]}.{source[2]}.{source[3]}";
        }
    }
}