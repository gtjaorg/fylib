

// HashHelper
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 哈希和加密算法帮助类，提供各种常用的哈希计算和加密解密功能
/// </summary>
/// <remarks>
/// 该类包含以下主要功能模块：
/// 
/// 1. **CRC32校验算法**
///    - 提供标准IEEE 802.3 CRC32校验值计算
///    - 支持字节数组和字符串输入
///    - 可输出字节数组或十六进制字符串格式
/// 
/// 2. **MD5哈希算法**
///    - 计算MD5哈希值（注意：不推荐用于安全敏感场景）
///    - 支持字节数组和十六进制字符串输出
/// 
/// 3. **SHA系列哈希算法**
///    - SHA-1（已过时，不推荐安全使用）
///    - SHA-256（推荐用于安全应用）
///    - SHA-512（最高安全级别）
/// 
/// 4. **QQ TEA/XTEA加密算法**
///    - QQ协议专用的TEA加密算法
///    - 扩展TEA（XTEA）算法
///    - 支持加密和解密操作
/// 
/// 5. **QQ协议专用功能**
///    - QQ官方认证签名生成
///    - 复杂的多层加密处理
/// 
/// 使用建议：
/// - 安全应用请使用SHA-256或SHA-512
/// - 数据完整性检查可使用CRC32
/// - QQ相关开发使用专门的QQ TEA算法
/// </remarks>
public static class HashHelper
{
    /// <summary>
    /// 提供CRC32校验算法的实现
    /// </summary>
    /// <remarks>
    /// 该类使用标准的CRC32多项式0xEDB88320进行校验值计算
    /// </remarks>
    public class CRC32
    {
        /// <summary>
        /// CRC32查找表，用于加速CRC32计算
        /// </summary>
        private static uint[] crcTable = null!;

        /// <summary>
        /// 初始化CRC32类的实例，生成CRC32校验表
        /// </summary>
        /// <remarks>
        /// 构造函数将创建一个包含256个元素的CRC32查找表，用于快速计算CRC32校验值。
        /// 使用标准的CRC32多项式0xEDB88320（IEEE 802.3标准）。
        /// 查找表的生成基于多项式除法原理，预先计算所有可能的8位值对应的CRC32值。
        /// </remarks>
        public CRC32()
        {
            const uint polynomial = 0xedb88320;
            crcTable = new uint[256];

            for (var i = 0; i < 256; i++)
            {
                var crc = (uint)i;
                for (var j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                crcTable[i] = crc;
            }
        }
        /// <summary>
        /// 计算字节数组的CRC32校验值
        /// </summary>
        /// <param name="bytes">要计算CRC32校验值的字节数组</param>
        /// <returns>返回32位无符号整数形式的CRC32校验值</returns>
        /// <remarks>
        /// CRC32算法是一种循环冗余检查码，广泛用于数据完整性验证。
        /// 该方法使用IEEE 802.3标准的CRC32算法，初始值为0xFFFFFFFF，最终结果取反。
        /// 计算过程：
        /// 1. 初始化CRC值为0xFFFFFFFF
        /// 2. 对每个输入字节，与CRC的低8位异或作为查表索引
        /// 3. 用查表结果与CRC右移8位的结果异或，更新CRC值
        /// 4. 处理完所有字节后，对最终CRC值取反得到结果
        /// </remarks>
        public uint GetCRC32(byte[] bytes)
        {
            var crcValue = 0xffffffff;

            for (var i = 0; i < bytes.Length; i++)
            {
                var index = (byte)(((crcValue) & 0xff) ^ bytes[i]);
                crcValue = crcTable[index] ^ (crcValue >> 8);
            }
            return ~crcValue;
        }

        /// <summary>
        /// 计算字符串的CRC32校验值
        /// </summary>
        /// <param name="str">要计算CRC32校验值的字符串</param>
        /// <returns>返回32位无符号整数形式的CRC32校验值</returns>
        /// <remarks>
        /// 该方法首先将输入字符串使用系统默认编码（通常是UTF-8或GBK）转换为字节数组，
        /// 然后调用GetCRC32(byte[])方法计算CRC32校验值。
        /// 注意：不同的字符编码可能产生不同的CRC32值，因此在跨平台或多语言环境中需要注意编码一致性。
        /// </remarks>
        public uint GetCRC32(string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            return GetCRC32(bytes);
        }
    }

    private class QQCrypter
    {
        private long contextStart;

        private long Crypt;

        private bool Header;

        private byte[] Key;

        private byte[] Out = [];

        private long padding;

        private byte[] Plain = [];

        private long Pos;

        private long preCrypt;

        private byte[] prePlain = [];

        private Random rd;

        public QQCrypter()
        {
            Key = new byte[16];
            rd = new Random();
        }

        public byte[] QQTeanDecipher(byte[] arrayIn, List<byte[]> Keys)
        {
            var array = new byte[0];
            for (var i = 0; i < Keys.Count; i = checked(i + 1))
            {
                array = TeanDecipher(arrayIn, Keys[i]);
                if (array.Length != 0)
                {
                    return array;
                }
            }
            return array;
        }

        public byte[] TeanDecipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Decrypt(arrayIn, arrayKey, 0L, 2654435769u, 16u);
        }

        public byte[] TeanEncipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Encrypt(arrayIn, arrayKey, 0L, 2654435769u, 16u);
        }

        public byte[] XTeanEncipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Encrypt(arrayIn, arrayKey, 0L, 1474859335u, 32u);
        }

        public byte[] XTeanDecipher(byte[] arrayIn, byte[] arrayKey)
        {
            return new QQCrypter().QQ_Decrypt(arrayIn, arrayKey, 0L, 1474859335u, 32u);
        }

        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, uint delta, uint round)
        {
            return Decipher(arrayIn, arrayKey, 0L, delta, round);
        }

        private long getUnsignedInt(byte[] arrayIn, int offset, int len)
        {
            var num = 0L;
            var num2 = 0;
            checked
            {
                num2 = ((len <= 8) ? (offset + len) : (offset + 8));
                for (var i = offset; i < num2; i++)
                {
                    num <<= 8;
                    num |= (ushort)(arrayIn[i] & 0xFF);
                }
                return (num & 0xFFFFFFFFu) | (num >> 32);
            }
        }

        private byte[] Decipher(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            var arr = new byte[24];
            var array = new byte[8];
            checked
            {
                if (arrayIn.Length >= 8)
                {
                    if (arrayKey.Length < 16)
                    {
                        return array;
                    }
                    delta &= 0xFFFFFFFFu;
                    long num = delta * round;
                    num &= 0xFFFFFFFFu;
                    var num2 = getUnsignedInt(arrayIn, (int)offset, 4);
                    var num3 = getUnsignedInt(arrayIn, (int)offset + 4, 4);
                    var unsignedInt = getUnsignedInt(arrayKey, 0, 4);
                    var unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
                    var unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
                    var unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
                    for (var i = 1; i <= round; i++)
                    {
                        num3 -= ((num2 << 4) + unsignedInt3) ^ (num2 + num) ^ ((num2 >> 5) + unsignedInt4);
                        num3 &= 0xFFFFFFFFu;
                        num2 -= ((num3 << 4) + unsignedInt) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt2);
                        num2 &= 0xFFFFFFFFu;
                        num -= unchecked((long)delta);
                        num &= 0xFFFFFFFFu;
                    }
                    arr = CopyMemory(arr, 0, num2);
                    arr = CopyMemory(arr, 4, num3);
                    array[0] = arr[3];
                    array[1] = arr[2];
                    array[2] = arr[1];
                    array[3] = arr[0];
                    array[4] = arr[7];
                    array[5] = arr[6];
                    array[6] = arr[5];
                    array[7] = arr[4];
                }
                return array;
            }
        }

        private byte[] CopyMemory(byte[] arr, int arr_index, long input)
        {
            checked
            {
                if (arr_index + 4 <= arr.Length)
                {
                    arr[arr_index + 3] = (byte)((input & 0xFF000000u) >> 24);
                    arr[arr_index + 2] = (byte)((input & 0xFF0000) >> 16);
                    arr[arr_index + 1] = (byte)((input & 0xFF00) >> 8);
                    arr[arr_index] = (byte)(input & 0xFF);
                    arr[arr_index] = (byte)(arr[arr_index] & 0xFF);
                    arr[arr_index + 1] = (byte)(arr[arr_index + 1] & 0xFF);
                    arr[arr_index + 2] = (byte)(arr[arr_index + 2] & 0xFF);
                    arr[arr_index + 3] = (byte)(arr[arr_index + 3] & 0xFF);
                }
                return arr;
            }
        }

        private byte[] QQ_Decrypt(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            var result = new byte[0];
            if (arrayIn.Length < 16 || arrayIn.Length % 8 != 0)
            {
                return result;
            }
            if (arrayKey.Length != 16)
            {
                return result;
            }
            checked
            {
                var array = new byte[offset + 8];
                arrayKey.CopyTo(Key, 0);
                preCrypt = 0L;
                Crypt = 0L;
                prePlain = Decipher(arrayIn, arrayKey, offset, delta, round);
                Pos = prePlain[0] & 7;
                var num = arrayIn.Length - Pos - 10;
                if (num <= 0)
                {
                    return result;
                }
                Out = new byte[num];
                preCrypt = 0L;
                Crypt = 8L;
                contextStart = 8L;
                Pos++;
                padding = 1L;
                while (padding < 3)
                {
                    if (Pos < 8)
                    {
                        Pos++;
                        padding++;
                    }
                    else if (Pos == 8)
                    {
                        for (var i = 0; i < array.Length; i++)
                        {
                            array[i] = arrayIn[i];
                        }
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                var num2 = 0L;
                while (num != 0L)
                {
                    if (Pos < 8)
                    {
                        Out[(int)(IntPtr)num2] = (byte)(array[(int)(IntPtr)(offset + preCrypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]);
                        num2++;
                        num--;
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        array = arrayIn;
                        preCrypt = Crypt - 8;
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                for (padding = 1L; padding <= 7; padding++)
                {
                    if (Pos < 8)
                    {
                        if ((array[(int)(IntPtr)(offset + preCrypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]) != 0)
                        {
                            return result;
                        }
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        for (var j = 0; j < array.Length; j++)
                        {
                            array[j] = arrayIn[j];
                        }
                        preCrypt = Crypt;
                        if (!Decrypt8Bytes(arrayIn, offset, delta, round))
                        {
                            return result;
                        }
                    }
                }
                return Out;
            }
        }

        private bool Decrypt8Bytes(byte[] arrayIn, uint delta, uint round)
        {
            return Decrypt8Bytes(arrayIn, 0L, delta, round);
        }

        private bool Decrypt8Bytes(byte[] arrayIn, long offset, uint delta, uint round)
        {
            checked
            {
                for (Pos = 0L; Pos <= 7; Pos++)
                {
                    if (contextStart + Pos > arrayIn.Length - 1)
                    {
                        return true;
                    }
                    prePlain[(int)(IntPtr)Pos] = (byte)(prePlain[(int)(IntPtr)Pos] ^ arrayIn[(int)(IntPtr)(offset + Crypt + Pos)]);
                }
                try
                {
                    prePlain = Decipher(prePlain, Key, delta, round);
                    contextStart += 8L;
                    Crypt += 8L;
                    Pos = 0L;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private long Rand()
        {
            checked
            {
                return rd.Next() + unchecked(rd.Next() % 1024);
            }
        }

        private void Encrypt8Bytes(uint delta, uint round)
        {
            checked
            {
                try
                {
                    for (Pos = 0L; Pos <= 7; Pos++)
                    {
                        if (Header)
                        {
                            Plain[(int)(IntPtr)Pos] = (byte)(Plain[(int)(IntPtr)Pos] ^ prePlain[(int)(IntPtr)Pos]);
                        }
                        else
                        {
                            Plain[(int)(IntPtr)Pos] = (byte)(Plain[(int)(IntPtr)Pos] ^ Out[(int)(IntPtr)(preCrypt + Pos)]);
                        }
                    }
                    var array = Encipher(Plain, Key, delta, round);
                    for (var i = 0; i <= 7; i++)
                    {
                        Out[(int)(IntPtr)(Crypt + i)] = array[i];
                    }
                    for (Pos = 0L; Pos <= 7; Pos++)
                    {
                        Out[(int)(IntPtr)(Crypt + Pos)] = (byte)(Out[(int)(IntPtr)(Crypt + Pos)] ^ prePlain[(int)(IntPtr)Pos]);
                    }
                    Plain.CopyTo(prePlain, 0);
                    preCrypt = Crypt;
                    Crypt += 8L;
                    Pos = 0L;
                    Header = false;
                }
                catch
                {
                }
            }
        }

        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, uint delta, uint round)
        {
            return Encipher(arrayIn, arrayKey, 0L, delta, round);
        }

        private byte[] Encipher(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            var array = new byte[8];
            var arr = new byte[24];
            checked
            {
                if (arrayIn.Length >= 8)
                {
                    if (arrayKey.Length < 16)
                    {
                        return array;
                    }
                    var num = 0L;
                    delta &= 0xFFFFFFFFu;
                    var num2 = getUnsignedInt(arrayIn, (int)offset, 4);
                    var num3 = getUnsignedInt(arrayIn, (int)offset + 4, 4);
                    var unsignedInt = getUnsignedInt(arrayKey, 0, 4);
                    var unsignedInt2 = getUnsignedInt(arrayKey, 4, 4);
                    var unsignedInt3 = getUnsignedInt(arrayKey, 8, 4);
                    var unsignedInt4 = getUnsignedInt(arrayKey, 12, 4);
                    for (var i = 1; i <= round; i++)
                    {
                        num += unchecked((long)delta);
                        num &= 0xFFFFFFFFu;
                        num2 += ((num3 << 4) + unsignedInt) ^ (num3 + num) ^ ((num3 >> 5) + unsignedInt2);
                        num2 &= 0xFFFFFFFFu;
                        num3 += ((num2 << 4) + unsignedInt3) ^ (num2 + num) ^ ((num2 >> 5) + unsignedInt4);
                        num3 &= 0xFFFFFFFFu;
                    }
                    arr = CopyMemory(arr, 0, num2);
                    arr = CopyMemory(arr, 4, num3);
                    array[0] = arr[3];
                    array[1] = arr[2];
                    array[2] = arr[1];
                    array[3] = arr[0];
                    array[4] = arr[7];
                    array[5] = arr[6];
                    array[6] = arr[5];
                    array[7] = arr[4];
                }
                return array;
            }
        }

        private byte[] QQ_Encrypt(byte[] arrayIn, byte[] arrayKey, long offset, uint delta, uint round)
        {
            Plain = new byte[8];
            prePlain = new byte[8];
            Pos = 1L;
            padding = 0L;
            preCrypt = 0L;
            Crypt = 0L;
            arrayKey.CopyTo(Key, 0);
            Header = true;
            Pos = 2L;
            Pos = checked(arrayIn.Length + 10) % 8;
            checked
            {
                if (Pos != 0L)
                {
                    Pos = 8 - Pos;
                }
                Out = new byte[arrayIn.Length + Pos + 10];
                Plain[0] = (byte)((Rand() & 0xF8) | Pos);
                for (var i = 1; i <= Pos; i++)
                {
                    Plain[i] = (byte)(Rand() & 0xFF);
                }
                Pos++;
                padding = 1L;
                while (padding < 3)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = (byte)(Rand() & 0xFF);
                        padding++;
                        Pos++;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                var num = (int)offset;
                var num2 = 0L;
                num2 = arrayIn.Length;
                while (num2 > 0)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = arrayIn[num];
                        num++;
                        Pos++;
                        num2--;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                padding = 1L;
                while (padding < 9)
                {
                    if (Pos < 8)
                    {
                        Plain[(int)(IntPtr)Pos] = 0;
                        Pos++;
                        padding++;
                    }
                    else if (Pos == 8)
                    {
                        Encrypt8Bytes(delta, round);
                    }
                }
                return Out;
            }
        }

        public static byte[] TeaEncipher(byte[] encrypt_data, byte[] key)
        {
            var list = new List<byte>(encrypt_data);
            var list2 = new List<byte>(key);
            list.Reverse(0, 4);
            list.Reverse(4, 4);
            var num = smethod_0(list.GetRange(0, 4).ToArray());
            var num2 = smethod_0(list.GetRange(4, 4).ToArray());
            var num3 = smethod_0(list2.GetRange(0, 4).ToArray());
            var num4 = smethod_0(list2.GetRange(4, 4).ToArray());
            var num5 = smethod_0(list2.GetRange(8, 4).ToArray());
            var num6 = smethod_0(list2.GetRange(12, 4).ToArray());
            var num7 = 16u;
            var num8 = 0u;
            var num9 = 2654435769u;
            checked
            {
                while (num7-- != 0)
                {
                    num8 += num9;
                    num += ((num2 << 4) + num3) ^ (num2 + num8) ^ ((num2 >> 5) + num4);
                    num2 += ((num << 4) + num5) ^ (num + num8) ^ ((num >> 5) + num6);
                }
                return ToByteArray(new uint[2]
                {
                    byteswap_ulong(num),
                    byteswap_ulong(num2)
                }, IncludeLength: false);
            }
        }

        private static uint byteswap_ulong(uint i)
        {
            return checked((i << 24) + ((i << 8) & 0xFF0000) + ((i >> 8) & 0xFF00) + (i >> 24));
        }

        private static byte[] ToByteArray(uint[] Data, bool IncludeLength)
        {
            checked
            {
                var num = ((!IncludeLength) ? (Data.Length << 2) : ((int)Data[Data.Length - 1]));
                var array = new byte[num];
                for (var i = 0; i < num; i++)
                {
                    array[i] = (byte)(Data[i >> 2] >> ((i & 3) << 3));
                }
                return array;
            }
        }

        private static uint smethod_0(byte[] data)
        {
            try
            {
                return checked((uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)));
            }
            catch
            {
                return 0u;
            }
        }
    }
    /// <summary>
    /// 计算字符串的MD5哈希值，返回字节数组
    /// </summary>
    /// <param name="str">要计算MD5哈希值的字符串</param>
    /// <returns>返回16字节的MD5哈希值数组</returns>
    /// <remarks>
    /// MD5是一种广泛使用的密码学哈希函数，产生128位（16字节）的哈希值。
    /// 该方法使用系统默认编码将字符串转换为字节数组后计算MD5值。
    /// 注意：MD5已被认为在密码学上不安全，建议在安全敏感的应用中使用SHA-256或更强的哈希算法。
    /// 但MD5仍可用于数据完整性检查等非安全场景。
    /// </remarks>
    public static byte[] MD5(string str)
    {
        var bytes = Encoding.Default.GetBytes(str);
        return MD5(bytes);
    }
    /// <summary>
    /// 计算字符串的MD5哈希值，返回十六进制字符串
    /// </summary>
    /// <param name="str">要计算MD5哈希值的字符串</param>
    /// <returns>返回32位小写十六进制字符串形式的MD5哈希值</returns>
    /// <remarks>
    /// 该方法首先计算输入字符串的MD5哈希值，然后将结果转换为十六进制字符串格式。
    /// 返回的字符串长度固定为32个字符，每个字节用两位十六进制数表示。
    /// 十六进制字符使用小写字母（a-f），如果需要大写可以调用ToUpper()方法。
    /// 这种格式便于存储、传输和比较，是最常见的MD5值表示方式。
    /// </remarks>
    public static string MD5_(string str)
    {
        var bytes = Encoding.Default.GetBytes(str);
        var array = MD5(bytes);
        var text = "";
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            text += array[i].ToString("x").PadLeft(2, '0');
        }
        return text;
    }
    /// <summary>
    /// 计算字节数组的MD5哈希值，返回十六进制字符串
    /// </summary>
    /// <param name="value">要计算MD5哈希值的字节数组</param>
    /// <returns>返回32位小写十六进制字符串形式的MD5哈希值</returns>
    /// <remarks>
    /// 该方法直接对输入的字节数组计算MD5哈希值，然后转换为十六进制字符串。
    /// 相比字符串版本，该方法避免了字符编码转换的影响，适用于二进制数据的哈希计算。
    /// 返回格式与MD5_(string)方法完全相同，都是32位小写十六进制字符串。
    /// </remarks>
    public static string MD5_(byte[] value)
    {
        var array = MD5(value);
        var text = "";
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            text += array[i].ToString("x").PadLeft(2, '0');
        }
        return text;
    }
    /// <summary>
    /// 计算字节数组的MD5哈希值，返回字节数组
    /// </summary>
    /// <param name="value">要计算MD5哈希值的字节数组</param>
    /// <returns>返回16字节的MD5哈希值数组</returns>
    /// <remarks>
    /// 这是MD5计算的核心方法，直接调用.NET的MD5.HashData方法。
    /// MD5算法将任意长度的输入数据转换为固定长度的128位（16字节）摘要。
    /// 该方法适用于需要原始字节形式MD5值的场景，如进一步的加密操作或二进制比较。
    /// 使用了.NET 6+的新API MD5.HashData，相比传统方法更简洁高效。
    /// </remarks>
    public static byte[] MD5(byte[] value)
    {
        return System.Security.Cryptography.MD5.HashData(value);

    }
    /// <summary>
    /// 使用QQ TEA算法加密数据
    /// </summary>
    /// <param name="value">要加密的字节数组数据</param>
    /// <param name="key">16字节的加密密钥</param>
    /// <returns>加密后的字节数组，如果密钥长度不正确则返回null</returns>
    /// <remarks>
    /// QQ TEA是腾讯QQ协议中使用的一种加密算法，基于标准TEA（Tiny Encryption Algorithm）算法改进。
    /// 该算法的特点：
    /// 1. 密钥长度必须是16字节（128位）
    /// 2. 使用特定的轮数（16轮）和增量值进行加密
    /// 3. 在QQ协议中广泛用于消息和数据包的加密
    /// 4. 相比标准TEA，增加了额外的安全措施
    /// 注意：如果密钥长度不是16字节，方法将返回null。
    /// </remarks>
    public static byte[] QQTEAEncrypt(byte[] value, byte[] key)
    {
        if (key.Length != 16)
        {
            return [];
        }
        return new QQCrypter().TeanEncipher(value, key);
    }
    /// <summary>
    /// 使用QQ TEA算法解密数据
    /// </summary>
    /// <param name="value">要解密的字节数组数据</param>
    /// <param name="key">16字节的解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的字节数组，如果密钥长度不正确则返回null</returns>
    /// <remarks>
    /// 这是QQTEAEncrypt方法的逆操作，用于解密使用QQ TEA算法加密的数据。
    /// 解密过程：
    /// 1. 验证密钥长度是否为16字节
    /// 2. 使用与加密相反的操作序列进行解密
    /// 3. 恢复原始数据内容
    /// 重要提醒：
    /// - 解密密钥必须与加密密钥完全相同
    /// - 如果数据已被篡改或密钥错误，解密结果可能是无意义的数据
    /// - 密钥长度不正确时返回null
    /// </remarks>
    public static byte[] QQTEADecrypt(byte[] value, byte[] key)
    {
        if (key.Length != 16)
        {
            return [];
        }
        return new QQCrypter().TeanDecipher(value, key);
    }
    /// <summary>
    /// 计算字节数组的CRC32校验值，返回字节数组形式
    /// </summary>
    /// <param name="value">要计算CRC32校验值的字节数组</param>
    /// <returns>返回4字节的CRC32校验值数组（小端序）</returns>
    /// <remarks>
    /// 该方法是CRC32类的静态包装器，提供更便捷的调用方式。
    /// 内部流程：
    /// 1. 创建CRC32实例并计算校验值
    /// 2. 将32位无符号整数转换为4字节数组
    /// 3. 使用小端序（Little-Endian）字节顺序
    /// 适用于需要字节形式CRC32值的场景，如网络协议或二进制文件格式。
    /// </remarks>
    public static byte[] Crc32(byte[] value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value));
    }
    /// <summary>
    /// 计算字符串的CRC32校验值，返回字节数组形式
    /// </summary>
    /// <param name="value">要计算CRC32校验值的字符串</param>
    /// <returns>返回4字节的CRC32校验值数组（小端序）</returns>
    /// <remarks>
    /// 字符串版本的CRC32计算，使用系统默认编码转换字符串。
    /// 与Crc32(byte[])方法相同的处理流程，但需要先进行字符编码转换。
    /// 注意：不同系统的默认编码可能不同，跨平台使用时需要考虑编码一致性。
    /// </remarks>
    public static byte[] Crc32(string value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value));
    }
    /// <summary>
    /// 计算字符串的CRC32校验值，返回大写十六进制字符串
    /// </summary>
    /// <param name="value">要计算CRC32校验值的字符串</param>
    /// <returns>返回8位大写十六进制字符串形式的CRC32校验值</returns>
    /// <remarks>
    /// 该方法提供最常用的CRC32输出格式 - 十六进制字符串。
    /// 处理流程：
    /// 1. 计算字符串的CRC32值
    /// 2. 转换为4字节数组
    /// 3. 转换为十六进制字符串并转为大写
    /// 输出格式固定为8个大写十六进制字符（如：1A2B3C4D）。
    /// 这种格式便于显示、比较和存储，是最常见的CRC32表示方式。
    /// </remarks>
    public static string Crc32_(string value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value)).ToHex().ToUpper();
    }
    /// <summary>
    /// 计算字节数组的CRC32校验值，返回大写十六进制字符串
    /// </summary>
    /// <param name="value">要计算CRC32校验值的字节数组</param>
    /// <returns>返回8位大写十六进制字符串形式的CRC32校验值</returns>
    /// <remarks>
    /// 字节数组版本的CRC32十六进制字符串输出。
    /// 与Crc32_(string)方法输出格式完全相同，但直接处理字节数据。
    /// 适用于二进制数据的CRC32校验，避免了字符编码的影响。
    /// 输出为8位大写十六进制字符串格式。
    /// </remarks>
    public static string Crc32_(byte[] value)
    {
        return BitConverter.GetBytes(new CRC32().GetCRC32(value)).ToHex().ToUpper();
    }
    /// <summary>
    /// 使用XTEA算法加密数据
    /// </summary>
    /// <param name="value">要加密的字节数组数据</param>
    /// <param name="key">16字节的加密密钥</param>
    /// <returns>加密后的字节数组</returns>
    /// <remarks>
    /// XTEA（eXtended TEA）是TEA算法的扩展版本，提供更好的安全性。
    /// 该算法特点：
    /// 1. 密钥长度为16字节（128位）
    /// 2. 使用32轮加密操作（相比TEA的16轮）
    /// 3. 增量值为0x57E47E14，提供更好的随机性
    /// 4. 能够抵抗相关密钥攻击和弱密钥攻击
    /// 5. 比标准TEA具有更高的安全强度
    /// XTEA在需要高效加密且安全性要求较高的场景中使用。
    /// </remarks>
    public static byte[] XTeaEncrypt(byte[] value, byte[] key)
    {
        return new QQCrypter().XTeanEncipher(value, key);
    }
    /// <summary>
    /// 使用XTEA算法解密数据
    /// </summary>
    /// <param name="value">要解密的字节数组数据</param>
    /// <param name="key">16字节的解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的字节数组</returns>
    /// <remarks>
    /// 这是XTeaEncrypt方法的逆操作，用于解密XTEA加密的数据。
    /// 解密过程特点：
    /// 1. 使用与加密相同的密钥和轮数设置
    /// 2. 按相反的顺序执行加密操作
    /// 3. 密钥必须与加密时完全相同
    /// 4. 如果数据被篡改或密钥错误，解密结果将是无效数据
    /// XTEA解密具有良好的错误检测能力，错误的密钥通常会产生明显的无效输出。
    /// </remarks>
    public static byte[] XTeaDecrypt(byte[] value, byte[] key)
    {
        return new QQCrypter().XTeanDecipher(value, key);
    }

    /// <summary>
    /// 创建QQ协议的官方认证签名
    /// </summary>
    /// <param name="tzm">时间戳模数，用于计算哈希轮数</param>
    /// <param name="OffKey">官方密钥</param>
    /// <param name="bufSigPicNew">图片签名数据</param>
    /// <param name="bufTGTGT">TGT票据数据</param>
    /// <returns>返回包含MD5哈希值和CRC32校验值的20字节认证签名</returns>
    /// <remarks>
    /// 这是一个复杂的QQ协议认证签名生成算法，用于QQ客户端与服务器的安全通信。
    /// 算法流程：
    /// 1. 使用MD5对OffKey和bufSigPicNew进行哈希计算
    /// 2. 创建RC4密钥流并进行流加密处理
    /// 3. 基于时间戳计算哈希迭代轮数
    /// 4. 使用TEA算法对数据进行多轮加密
    /// 5. 最终生成16字节MD5哈希值和4字节CRC32校验值
    /// 
    /// 安全特性：
    /// - 使用多种加密算法（MD5、RC4、TEA）的组合
    /// - 时间戳相关的动态密钥生成
    /// - 多层哈希迭代增强安全性
    /// 
    /// 注意：此方法专用于QQ协议，不适用于其他加密场景。
    /// </remarks>
    public static byte[] CreateOfficial(int tzm, byte[] OffKey, byte[] bufSigPicNew, byte[] bufTGTGT)
    {
        var num = 4;
        var num2 = 256;
        var collection = OffKey.Md5();
        var collection2 = MD5(bufSigPicNew);
        var list = new List<byte>(collection);
        list.AddRange(collection2);
        var array = list.ToArray();
        checked
        {
            var num3 = unchecked(tzm % 19) + 5;
            var array2 = new byte[256];
            var array3 = new byte[256];
            for (var i = 0; i < num2; i++)
            {
                array2[i] = (byte)i;
                var num4 = 16 + unchecked(i % 16);
                array3[i] = array[num4];
            }
            var num5 = 0;
            byte b = 0;
            for (var j = 0; j < num2; j++)
            {
                unchecked
                {
                    num5 = checked(num5 + unchecked((int)array2[j]) + unchecked((int)array3[j])) % num2;
                    b = array2[num5];
                    array2[num5] = array2[j];
                    array2[j] = b;
                }
            }
            num5 = 0;
            for (var k = 0; k < 16; k++)
            {
                unchecked
                {
                    num5 = checked(num5 + unchecked((int)array2[checked(k + 1)])) % num2;
                    b = array2[num5];
                }
                array2[num5] = array2[k + 1];
                array2[k + 1] = b;
                int num6;
                unchecked
                {
                    num6 = checked(unchecked((int)array2[num5]) + unchecked((int)array2[checked(k + 1)])) % num2;
                }
                list.Add((byte)(array2[num6] ^ array[k]));
            }
            list.AddRange(MD5(bufTGTGT));
            List<byte> list2 = [.. MD5(list.ToArray())];
            byte[] array4 = [.. list2];
            for (var l = 0; l < num3; l++)
            {
                array4 = MD5(array4);
            }
            list.RemoveRange(0, 16);
            list.InsertRange(0, array4);
            var encrypt_data = list2.GetRange(0, 8).ToArray();
            var encrypt_data2 = list2.GetRange(8, 8).ToArray();
            var array5 = new byte[16];
            for (var m = 0; m < num; m++)
            {
                var index = m * 16;
                var range = list.GetRange(index, 16);
                range.Reverse(0, 4);
                range.Reverse(4, 4);
                range.Reverse(8, 4);
                range.Reverse(12, 4);
                var list3 = new List<byte>(QQCrypter.TeaEncipher(encrypt_data, range.ToArray()));
                list3.AddRange(QQCrypter.TeaEncipher(encrypt_data2, range.ToArray()));
                for (var n = m; n < 16; n++)
                {
                    ref var reference = ref array5[n];
                    reference = (byte)(reference ^ list3[n]);
                }
            }
            array5 = MD5(array5);
            var cRC = (int)new CRC32().GetCRC32(array5);
            var list4 = new List<byte>(array5);
            list4.AddRange(BitConverter.GetBytes(cRC));
            return list4.ToArray();
        }
    }

    /// <summary>
    /// 计算字符串的SHA-256哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字符串数据</param>
    /// <returns>返回64位大写十六进制字符串形式的SHA-256哈希值</returns>
    /// <remarks>
    /// SHA-256是SHA-2算法家族中的一种，产生256位（32字节）的哈希值。
    /// 该算法具有以下特点：
    /// 1. 密码学安全性高，目前被认为是安全的哈希算法
    /// 2. 广泛应用于数字证书、区块链、数字签名等安全领域
    /// 3. 输出长度固定为64个十六进制字符
    /// 4. 使用UTF-8编码将字符串转换为字节数组进行计算
    /// 5. 返回大写形式的十六进制字符串（A-F）
    /// SHA-256相比MD5和SHA-1更安全，推荐在安全敏感的应用中使用。
    /// </remarks>
    public static string sha256(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        var array = SHA256.Create().ComputeHash(bytes);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 计算字节数组的SHA-256哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字节数组数据</param>
    /// <returns>返回64位大写十六进制字符串形式的SHA-256哈希值</returns>
    /// <remarks>
    /// 该方法直接对字节数组进行SHA-256哈希计算，避免了字符编码的影响。
    /// 适用于二进制数据、文件内容或任意字节序列的哈希计算。
    /// 输出格式与字符串版本相同，都是64位大写十六进制字符串。
    /// 在处理文件哈希、数据完整性校验等场景中特别有用。
    /// </remarks>
    public static string sha256(byte[] data)
    {
        var array = SHA256.Create().ComputeHash(data);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 计算字符串的SHA-1哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字符串数据</param>
    /// <returns>返回40位大写十六进制字符串形式的SHA-1哈希值</returns>
    /// <remarks>
    /// SHA-1是较早期的安全哈希算法，产生160位（20字节）的哈希值。
    /// 重要说明：
    /// 1. SHA-1已被发现存在安全漏洞，不建议在新的安全敏感应用中使用
    /// 2. 输出长度固定为40个十六进制字符
    /// 3. 使用UTF-8编码将字符串转换为字节数组
    /// 4. 返回大写形式的十六进制字符串
    /// 5. 仅适用于兼容性要求或非安全场景
    /// 建议在新项目中使用SHA-256或更强的哈希算法替代SHA-1。
    /// </remarks>
    public static string sha1(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        var array = SHA1.Create().ComputeHash(bytes);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 计算字节数组的SHA-1哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字节数组数据</param>
    /// <returns>返回40位大写十六进制字符串形式的SHA-1哈希值</returns>
    /// <remarks>
    /// 该方法直接对字节数组进行SHA-1哈希计算。
    /// 与字符串版本相同的安全性警告：
    /// - SHA-1已被证明存在碰撞攻击漏洞
    /// - 不建议用于安全敏感的新应用
    /// - 仅在需要兼容旧系统或非安全场景中使用
    /// 输出格式为40位大写十六进制字符串。
    /// </remarks>
    public static string sha1(byte[] data)
    {
        var array = SHA1.Create().ComputeHash(data);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 计算字节数组的SHA-512哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字节数组数据</param>
    /// <returns>返回128位大写十六进制字符串形式的SHA-512哈希值</returns>
    /// <remarks>
    /// SHA-512是SHA-2算法家族中最强的变体，产生512位（64字节）的哈希值。
    /// 该算法特点：
    /// 1. 提供极高的密码学安全性
    /// 2. 输出长度固定为128个十六进制字符
    /// 3. 计算速度相对较慢，但安全性最高
    /// 4. 适用于最高安全要求的应用场景
    /// 5. 直接处理字节数组数据，避免编码问题
    /// SHA-512在需要最高安全级别的场合中使用，如高价值数据的完整性保护。
    /// </remarks>
    public static string sha512(byte[] data)
    {
        var array = SHA512.Create().ComputeHash(data);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 计算字符串的SHA-512哈希值
    /// </summary>
    /// <param name="data">要计算哈希值的字符串数据</param>
    /// <returns>返回128位大写十六进制字符串形式的SHA-512哈希值</returns>
    /// <remarks>
    /// SHA-512字符串版本，首先将字符串用UTF-8编码转换为字节数组。
    /// 该方法结合了最高级别的安全性和字符串处理的便利性。
    /// 特点：
    /// 1. 产生512位的超长哈希值，提供最强的碰撞抗性
    /// 2. 输出128个大写十六进制字符
    /// 3. 适用于密码存储、数字签名、区块链等高安全场景
    /// 4. 使用UTF-8编码确保多语言字符的正确处理
    /// 在性能要求不是首要考虑因素且需要最高安全性的场合中推荐使用。
    /// </remarks>
    public static string sha512(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        var array = SHA512.Create().ComputeHash(bytes);
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < array.Length; i = checked(i + 1))
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }
}
