

// StringHelper
using FyLib.Http;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using HttpClient = FyLib.Http.HttpClient;

/// <summary>
/// String扩展类
/// </summary>
public static class StringHelper
{

    public static FyLib.Http.HttpClient asHttpClient(this string str)
    {
        HttpClient client = new HttpClient(str);
        return client;
    }
    /// <summary>
    /// 判断文本是否为IP地址
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsIP(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return false;
        }
        if (!Regex.IsMatch(str, @"\d{1,3}(\.\d{1,3}){3}"))
        {
            return false;
        }
        string[] array = str.Split('.');
        if (array.Length != 4)
        {
            return false;
        }
        for (int i = 0; i < array.Length; i++)
        {
            if (!int.TryParse(array[i], out int value) || value < 0 || value > 255)
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
        string text = str.Substring(0, tag.Length);
        checked
        {
            string text2 = str.Substring(str.Length - tag.Length, tag.Length);
            if (text == tag)
            {
                str = str.Substring(tag.Length, str.Length - tag.Length);
            }
            if (text2 == tag)
            {
                str = str.Substring(0, str.Length - tag.Length);
            }
            return str;
        }
    }

    /// <summary>
    /// 获取URL通配
    /// </summary>
    /// <param name="str"></param>
    /// <param name="Regex"></param>
    /// <returns></returns>
    public static string GetUrl(this string str, string Regex = "http[s]?://(?:(?!http[s]?://)[a-zA-Z]|[0-9]|[$\\-_@.&+/]|[!*\\(\\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+")
    {
        IEnumerator enumerator = new Regex(Regex).Matches(str).GetEnumerator();
        try
        {
            if (enumerator.MoveNext())
            {
                return ((Match)enumerator.Current).Value;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
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
                return new byte[0];
            }
            byte[] array = new byte[hex.Length / 2];
            int num = 0;
            checked
            {
                for (int i = 0; i < hex.Length; i += 2)
                {
                    array[num++] = Convert.ToByte(hex.Substring(i, 2), 16);
                }
                return array;
            }
        }
        catch
        {
            return new byte[0];
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

        foreach (char c in str)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 是否为空或NULL
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string? str)
    {
        if (str == null) return true;
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 转换为整数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int result = 0;
        if (!str.IsNullOrEmpty() && str.IndexOf(".") > -1)
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
        uint result = 0u;
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
        string[] array = new string[1];
        int num = strSource.IndexOf(strSplit, 0);
        if (num < 0)
        {
            array[0] = strSource;
            return array;
        }
        array[0] = strSource.Substring(0, num);
        return Split(strSource.Substring(checked(num + strSplit.Length)), strSplit, array);
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
        checked
        {
            string[] array = new string[attachArray.Length + 1];
            attachArray.CopyTo(array, 0);
            int num = strSource.IndexOf(strSplit, 0);
            if (num < 0)
            {
                array[attachArray.Length] = strSource;
                return array;
            }
            array[attachArray.Length] = strSource.Substring(0, num);
            return Split(strSource.Substring(num + strSplit.Length), strSplit, array);
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
        int num = str.IndexOf(left);
        if (num < 0)
        {
            return "";
        }
        return str.Substring(0, num);
    }

    /// <summary>
    /// 取文本左边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static string Left(this string str, int len)
    {
        if (len > str.Length)
        {
            return "";
        }
        return str[..len];
    }

    /// <summary>
    /// 取文本右边
    /// </summary>
    /// <param name="str"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string Right(this string str, string right)
    {
        int num = str.IndexOf(right);
        if (num < 0)
        {
            return "";
        }
        int length = str.Length;
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
        int startIndex = checked(str.Length - len);
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
        int num = str.IndexOf(left);
        checked
        {
            int num2 = str.IndexOf(right, num + left.Length);
            if (num < 0 || num2 < 0)
            {
                return "";
            }
            num += left.Length;
            num2 -= num;
            if (num < 0 || num2 < 0)
            {
                return "";
            }
            return str.Substring(num, num2);
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
        List<string> list = new List<string>();
        int num = str.IndexOf(left);
        int length = left.Length;
        checked
        {
            while (num >= 0)
            {
                int num2 = str.IndexOf(right, num + length);
                if (num2 < 0)
                {
                    break;
                }
                string item = str.Substring(num + length, num2 - num - length);
                list.Add(item);
                num = str.IndexOf(left, num2 + right.Length);
            }
            return list;
        }
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(this string str)
    {
        MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
        byte[] bytes = Encoding.Default.GetBytes(str);
        byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
        string text = "";
        for (int i = 0; i < array.Length; i = checked(i + 1))
        {
            text += array[i].ToString("x").PadLeft(2, '0');
        }
        return text;
    }

    /// <summary>
    /// 取时间戳
    /// </summary>
    /// <returns></returns>
    public static string TimeStamp()
    {
        DateTime dateTime = new DateTime(1970, 1, 1);
        return Convert.ToString(checked(DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000000);
    }
}
