

// BytesHelper
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Bytes扩展操作
/// </summary>
public static class BytesHelper
{
    /// <summary>
    /// byte[] 相加
    /// </summary>
    /// <param name="bin"></param>
    /// <param name="bin2"></param>
    /// <returns></returns>
    public static byte[] add(byte[] bin, byte[] bin2)
    {
        List<byte> list = new List<byte>();
        checked
        {
            for (int i = 0; i < bin.Length; i++)
            {
                list.Add(bin[i]);
            }
            for (int j = 0; j < bin2.Length; j++)
            {
                list.Add(bin2[j]);
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// 获取字节数组指针
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static IntPtr getPtr(this byte[] bin)
    {
        return Marshal.UnsafeAddrOfPinnedArrayElement(bin, 0);
    }
    /// <summary>
    /// 转换为Span
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static Span<byte> asSpan( this byte[] bin)
    {
        return new Span<byte>(bin);
    }
    /// <summary>
    /// MD5
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Md5(this byte[] data)
    {
        return new MD5CryptoServiceProvider().ComputeHash(data);
    }

    /// <summary>
    /// 判断两个字节集是否相等
    /// </summary>
    /// <returns></returns>
    public static bool Equal(this byte[] bin1, byte[] data)
    {
        if (bin1 == null || data == null)
        {
            return false;
        }
        if (bin1.Length != data.Length)
        {
            return false;
        }
        for (int i = 0; i < bin1.Length; i = checked(i + 1))
        {
            if (bin1[i] != data[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 查找字节集
    /// </summary>
    /// <param name="data"></param>
    /// <param name="searchBytes"></param>
    /// <returns></returns>
    public static int IndexOf(this byte[] data, byte[] searchBytes)
    {
        if (data == null)
        {
            return -1;
        }
        if (searchBytes == null)
        {
            return -1;
        }
        if (data.Length == 0)
        {
            return -1;
        }
        if (searchBytes.Length == 0)
        {
            return -1;
        }
        if (data.Length < searchBytes.Length)
        {
            return -1;
        }
        checked
        {
            for (int i = 0; i < data.Length - searchBytes.Length; i++)
            {
                if (data[i] != searchBytes[0])
                {
                    continue;
                }
                if (searchBytes.Length == 1)
                {
                    return i;
                }
                bool flag = true;
                for (int j = 1; j < searchBytes.Length; j++)
                {
                    if (data[i + j] != searchBytes[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    /// <summary>
    /// 随机字节集
    /// </summary>
    /// <param name="len"></param>
    /// <returns></returns>
    public static byte[] Rand(int len = 16)
    {
        byte[] array = new byte[len];
        new Random(Guid.NewGuid().GetHashCode()).NextBytes(array);
        return array;
    }

    /// <summary>
    /// 字节集反转
    /// </summary>
    public static byte[] Flip(this byte[] bytes)
    {
        Array.Reverse(bytes);
        return bytes;
    }

    /// <summary>
    /// 转换为Base64
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static string ToBase64(this byte[] bin)
    {
        return Convert.ToBase64String(bin);
    }

    /// <summary>
    /// 字节集到十六进制文本
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static string ToHex(this byte[] bin)
    {
        StringBuilder stringBuilder = new StringBuilder(checked(bin.Length * 2));
        try
        {
            foreach (byte b in bin)
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

    /// <summary>
    /// 格式化字节集
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static string Format(this byte[] bin)
    {
        string text = bin.ToHex();
        int length = text.Length;
        string text2 = "";
        for (int i = 0; i < length / 2; i = checked(i + 1))
        {
            if (i % 16 == 0 && i != 0)
            {
                text2 += "\r\n";
            }
            text2 = text2 + text.Substring(checked(i * 2), 2) + " ";
        }
        return text2;
    }

    /// <summary>
    /// 短整数到字节数组
    /// </summary>
    public static byte[] ToBin(short s)
    {
        return BitConverter.GetBytes(s).Flip();
    }

    /// <summary>
    /// 无符号短整数到字节数组
    /// </summary>
    public static byte[] ToBin(ushort s)
    {
        return BitConverter.GetBytes(s).Flip();
    }

    /// <summary>
    /// 整数到字节数组
    /// </summary>
    public static byte[] ToBin(int i)
    {
        return BitConverter.GetBytes(i).Flip();
    }

    /// <summary>
    /// 无符号整数到字节数组
    /// </summary>
    public static byte[] ToBin(uint i)
    {
        return BitConverter.GetBytes(i).Flip();
    }

    /// <summary>
    /// 长整数到字节数组
    /// </summary>
    public static byte[] ToBin(long l)
    {
        return BitConverter.GetBytes(l).Flip();
    }

    /// <summary>
    /// 无符号长整数到字节数组
    /// </summary>
    public static byte[] ToBin(ulong l)
    {
        return BitConverter.GetBytes(l).Flip();
    }

    /// <summary>
    /// 单精度浮点型到字节数组
    /// </summary>
    public static byte[] ToBin(float f)
    {
        return BitConverter.GetBytes(f).Flip();
    }

    /// <summary>
    /// 双精度浮点型到字节数组
    /// </summary>
    public static byte[] ToBin(double d)
    {
        return BitConverter.GetBytes(d).Flip();
    }

    /// <summary>
    /// 字符串转到字节数组
    /// </summary>
    public static byte[] ToBin(string str)
    {
        return Encoding.Default.GetBytes(str);
    }

    /// <summary>
    /// 2位字节集到短整数
    /// </summary>
    public static short ToShort(byte[] b)
    {
        return BitConverter.ToInt16(b.Flip(), 0);
    }

    /// <summary>
    /// 2位字节集到无符号短整数
    /// </summary>
    public static ushort ToUShort(byte[] b)
    {
        return BitConverter.ToUInt16(b.Flip(), 0);
    }

    /// <summary>
    /// 4为字节集到整形
    /// </summary>
    public static int ToInt(this byte[] bin, bool IsFlip = true)
    {
        byte[] array = bin.ToArray();
        if (IsFlip)
        {
            array = array.Flip();
        }
        if (array.Length == 2)
        {
            return BitConverter.ToUInt16(array, 0);
        }
        return BitConverter.ToInt32(array, 0);
    }

    /// <summary>
    /// 4为字节集到无符号整形
    /// </summary>
    public static uint ToUInt(this byte[] bin)
    {
        return BitConverter.ToUInt32(bin.ToArray().Flip(), 0);
    }

    /// <summary>
    /// 8位字节到长整形
    /// </summary>
    public static long ToLong(this byte[] b)
    {
        b.ToArray();
        return BitConverter.ToInt64(b.Flip(), 0);
    }

    /// <summary>
    /// IP地址转换为字节集
    /// </summary>
    /// <param name="ipStr"></param>
    /// <returns></returns>
    public static byte[] IP2Bin(string ipStr)
    {
        if (ipStr == null)
        {
            return null;
        }
        string[] array = ipStr.Split(new char[1] { '.' });
        if (array.Length != 4)
        {
            return null;
        }
        string[] array2 = array;
        foreach (string str in array2)
        {
            if ((str.ToInt() > 255) | (str.ToInt() < 0))
            {
                return null;
            }
        }
        byte[] array3 = new byte[4];
        checked
        {
            array3[3] = (byte)uint.Parse(array[3]);
            array3[2] = (byte)uint.Parse(array[2]);
            array3[1] = (byte)uint.Parse(array[1]);
            array3[0] = (byte)uint.Parse(array[0]);
            return array3;
        }
    }

    /// <summary>
    /// 转换为IP地址
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static string ToIP(this byte[] bin)
    {
        if (bin.Length != 4)
        {
            return "";
        }
        return string.Concat(string.Concat(bin[0] + "." + bin[1], ".", bin[2].ToString()), ".", bin[3].ToString());
    }

    /// <summary>
    /// 8位字节到无符号长整形
    /// </summary>
    public static ulong ToULong(byte[] b)
    {
        return BitConverter.ToUInt64(b.Flip(), 0);
    }

    /// <summary>
    /// 4位字节到单精度浮点型
    /// </summary>
    public static float ToFloat(byte[] b)
    {
        return BitConverter.ToSingle(b.Flip(), 0);
    }

    /// <summary>
    /// 8位字节到双精度浮点型
    /// </summary>
    public static double ToDouble(byte[] b)
    {
        return BitConverter.ToDouble(b.Flip(), 0);
    }

    /// <summary>
    /// 转换为文本
    /// </summary>
    /// <param name="b"></param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static string ToStr(this byte[] b, Encoding encoding)
    {
        return encoding.GetString(b);
    }

    /// <summary>
    /// 转换为文本
    /// </summary>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static string ToStr(this byte[] bin)
    {
        return Encoding.Default.GetString(bin);
    }
}
