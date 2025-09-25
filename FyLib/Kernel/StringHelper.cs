// StringHelper

using FyLib.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


/// <summary>
/// String扩展类
/// </summary>
public static partial class StringHelper
{
    /// <summary>
    /// 转换为QuickHttp
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static FyLib.Http.QuickHttp AsQuickHttp(this string str)
    {
        var client = new QuickHttp(str);
        return client;
    }

    /// <summary>
    /// 判断文本是否为IP地址
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsIp(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return false;
        }

        if (!MyRegex().IsMatch(str))
        {
            return false;
        }

        var array = str.Split('.');
        if (array[3].ToInt() <= 0 || array[3].ToInt() > 255) return false;
        if (array.Length != 4)
        {
            return false;
        }

        foreach (var t in array)
        {
            if (!int.TryParse(t, out var value) || value < 0 || value > 255)
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// 过滤所有空格与换行符
    /// </summary>
    public static string Fillter(this string str)
    {
        str = Regex.Replace(str, "\\s+", "");
        str = Regex.Replace(str, "[\n\r]", "");
        return str;
    }

    /// <summary>
    /// 删除文本首尾特征字
    /// </summary>
    /// <param name="str"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static string RemoveTag(this string str, string tag)
    {
        if (str.Length <= tag.Length)
        {
            return str;
        }

        var text = str[..tag.Length];
        checked
        {
            var text2 = str.Substring(str.Length - tag.Length, tag.Length);
            if (text == tag)
            {
                str = str.Substring(tag.Length, str.Length - tag.Length);
            }

            if (text2 == tag)
            {
                str = str[..^tag.Length];
            }

            return str;
        }
    }

    /// <summary>
    /// 获取URL通配
    /// </summary>
    /// <param name="str"></param>
    /// <param name="regex"></param>
    /// <returns></returns>
    public static string? GetUrl(this string str,
        string regex =
            "http[s]?://(?:(?!http[s]?://)[a-zA-Z]|[0-9]|[$\\-_@.&+/]|[!*\\(\\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+")
    {
        
        var enumerator = new Regex(regex).Matches(str).GetEnumerator();
        try
        {
            if (enumerator.MoveNext())
            {
                var temp = enumerator.Current;
                if (temp == null) return null;
                return ((Match)temp)?.Value;
            }
        }
        finally
        {
            var disposable = enumerator as IDisposable;
            disposable?.Dispose();
        }

        return null;
    }

    /// <summary>
    /// 十六进制文本到字节集
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] ToBytes(this string hex)
    {
        try
        {
            hex = hex.Fillter();
            if (hex.Length % 2 != 0)
            {
                return [];
            }

            var array = new byte[hex.Length / 2];
            var num = 0;
            checked
            {
                for (var i = 0; i < hex.Length; i += 2)
                {
                    array[num++] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

                return array;
            }
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// 文本到字节集
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] GetBytes(this string str)
    {
        return Encoding.Default.GetBytes(str);
    }

    /// <summary>
    /// 文本到字节集
    /// </summary>
    /// <param name="str"></param>
    /// <param name="encoding">编码模式</param>
    /// <returns></returns>
    public static byte[] GetBytes(this string str, Encoding encoding)
    {
        return encoding.GetBytes(str);
    }

    /// <summary>
    /// Base64转码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Base64(this string str)
    {
        return Convert.ToBase64String(Encoding.Default.GetBytes(str));
    }

    /// <summary>
    /// 判断是否为整数格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        return str.All(char.IsDigit);
    }

    /// <summary>
    /// 是否为空或NULL
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string? str)
    {
        return str == null || string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 转换为整数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        var result = 0;
        if (!str.IsNullOrEmpty() && str.IndexOf('.') > -1)
        {
            str = str.Left(".");
        }

        if (!str.IsNullOrEmpty() && str.IsNumeric())
        {
            result = int.Parse(str);
        }

        return result;
    }

    /// <summary>
    /// 转换为无符号整数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static uint ToUint(this string str)
    {
        var result = 0u;
        if (!str.IsNullOrEmpty() && str.IsNumeric())
        {
            result = uint.Parse(str);
        }

        return result;
    }

    /// <summary>
    /// 分割字符串到数组
    /// </summary>
    /// <param name="strSource">欲分割的字符串</param>
    /// <param name="strSplit">用作分割的字符串</param>
    /// <returns></returns>
    public static string[] Split(this string strSource, string strSplit)
    {
        var array = new string[1];
        var num = strSource.IndexOf(strSplit, 0, StringComparison.Ordinal);
        if (num < 0)
        {
            array[0] = strSource;
            return array;
        }

        array[0] = strSource[..num];
        return Split(strSource[checked(num + strSplit.Length)..], strSplit, array);
    }

    /// <summary>
    /// 采用递归将字符串分割成数组
    /// </summary>
    /// <param name="strSource"></param>
    /// <param name="strSplit"></param>
    /// <param name="attachArray"></param>
    /// <returns></returns>
    private static string[] Split(string strSource, string strSplit, string[] attachArray)
    {
        while (true)
        {
            checked
            {
                var array = new string[attachArray.Length + 1];
                attachArray.CopyTo(array, 0);
                var num = strSource.IndexOf(strSplit, 0, StringComparison.Ordinal);
                if (num < 0)
                {
                    array[attachArray.Length] = strSource;
                    return array;
                }

                array[attachArray.Length] = strSource[..num];
                strSource = strSource[(num + strSplit.Length)..];
                attachArray = array;
            }
        }
    }

    /// <summary>
    /// 取文本左边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="left"></param>
    /// <returns></returns>
    public static string Left(this string str, string left)
    {
        var num = str.IndexOf(left, StringComparison.Ordinal);
        if (num < 0)
        {
            return "";
        }

        return str[..num];
    }

    /// <summary>
    /// 取文本左边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static string Left(this string str, int len)
    {
        return len > str.Length ? "" : str[..len];
    }

    /// <summary>
    /// 取文本右边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string Right(this string str, string right)
    {
        var num = str.IndexOf(right, StringComparison.Ordinal);
        if (num < 0)
        {
            return "";
        }

        var length = str.Length;
        checked
        {
            if (length - num - right.Length <= 0)
            {
                return "";
            }

            return str.Substring(num + right.Length, length - (num + right.Length));
        }
    }

    /// <summary>
    /// 取文本右边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static string Right(this string str, int len)
    {
        if (len > str.Length)
        {
            return "";
        }

        var startIndex = checked(str.Length - len);
        return str.Substring(startIndex, len);
    }

    /// <summary>
    /// 取文本中间
    /// </summary>
    /// <param name="str">欲取出的文本</param>
    /// <param name="left">左边标记</param>
    /// <param name="right">右边标记</param>
    /// <returns></returns>
    public static string Between(this string str, string left, string right)
    {
        var num = str.IndexOf(left, StringComparison.Ordinal);
        checked
        {
            var num2 = str.IndexOf(right, num + left.Length, StringComparison.Ordinal);
            if (num < 0 || num2 < 0)
            {
                return "";
            }

            num += left.Length;
            num2 -= num;
            return num2 < 0 ? "" : str.Substring(num, num2);
        }
    }

    /// <summary>
    /// 取文本中间数组
    /// </summary>
    /// <param name="str">欲取出的文本</param>
    /// <param name="left">左边标记</param>
    /// <param name="right">右边标记</param>
    /// <returns>得到的字符串数组</returns>
    public static List<string> BetweenEx(this string str, string left, string right)
    {
        var list = new List<string>();
        var num = str.IndexOf(left, StringComparison.Ordinal);
        var length = left.Length;
        checked
        {
            while (num >= 0)
            {
                var num2 = str.IndexOf(right, num + length, StringComparison.Ordinal);
                if (num2 < 0)
                {
                    break;
                }

                var item = str.Substring(num + length, num2 - num - length);
                list.Add(item);
                num = str.IndexOf(left, num2 + right.Length, StringComparison.Ordinal);
            }

            return list;
        }
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Md5(this string str)
    {
        var bytes = Encoding.Default.GetBytes(str);
        var array = System.Security.Cryptography.MD5.HashData(bytes);
        var text = Convert.ToHexString(array);
        return text;
    }

    /// <summary>
    /// 十六进制文本转十进制
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int HexToInt(this string str)
    {
        return Convert.ToInt32(str, 16);
    }

    [GeneratedRegex(@"\d{1,3}(\.\d{1,3}){3}")]
    private static partial Regex MyRegex();
}