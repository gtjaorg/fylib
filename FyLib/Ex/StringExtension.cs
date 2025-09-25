using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using FyLib.Http;
using Newtonsoft.Json.Linq;


/// <summary>
/// String扩展
/// </summary>
public static class StringExtension
{

    /// <summary>
    /// string 扩展类
    /// </summary>
    extension(string source)
    {
        /// <summary>
        /// 判断是否为整数格式
        /// </summary>
        public bool IsNumeric
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                {
                    return false;
                }

                return source.All(char.IsDigit);
            }

        }
        /// <summary>
        /// MD5 小写
        /// </summary>
        public string md5 => source.MD5.ToLower();

        /// <summary>
        /// MD5 大写
        /// </summary>
        public string MD5
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(source);
                var array = System.Security.Cryptography.MD5.HashData(bytes);
                var text = Convert.ToHexString(array);
                return text;
            }
        }
        /// <summary>
        /// 是否为空或空字符串,同效IsNullOrEmpty
        /// </summary>
        public bool IsNull => string.IsNullOrEmpty(source);

        /// <summary>
        /// 是否为空或空白字符串
        /// </summary>
        public bool IsNullOrWhiteSpace_ => string.IsNullOrWhiteSpace(source);

        /// <summary>
        /// 是否为IP地址
        /// </summary>
        public bool IsIp
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                {
                    return false;
                }

                var parts = source.Split('.');
                if (parts.Length != 4)
                {
                    return false;
                }

                foreach (var part in parts)
                {
                    if (!int.TryParse(part, out var value) || value < 0 || value > 255)
                    {
                        return false;
                    }
                    // 不允许前导0，除非就是"0"
                    if (part.Length > 1 && part[0] == '0')
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 是否为整数
        /// </summary>
        public bool IsInt
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                {
                    return false;
                }
                return int.TryParse(source, out _);
            }
        }

        /// <summary>
        /// source是否为数字(整数或小数)
        /// </summary>
        public bool IsDecimal => decimal.TryParse(source, out _);

        /// <summary>
        /// BASE64 编码
        /// </summary>
        public string Base64
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
            }
        }

        /// <summary>
        /// 判断字符串是否可以转换为布尔值
        /// </summary>
        public bool IsBool => bool.TryParse(source, out _);

        /// <summary>
        /// 判断字符串是否为有效的日期时间格式
        /// </summary>
        public bool IsDateTime => DateTime.TryParse(source, out _);

        /// <summary>
        /// 将字符串解析为JObject对象
        /// </summary>
        /// <returns>解析后的JObject对象</returns>
        public JObject? ToJson()
        {
            try
            {
                var obj = JObject.Parse(source);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>转换后的对象，如果转换失败则返回null</returns>
        public T? ToObject<T>() where T : class
        {
            var obj = source.ToJson();
            if (obj == null) return null;
            try
            {
                return obj.ToObject<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换为 QuickHttp 对象
        /// </summary>
        /// <returns></returns>
        public QuickHttp ToQuickHttp()
        {
            var client = new QuickHttp(source);
            return client;
        }

        /// <summary>
        /// 过滤所有空格与换行符
        /// </summary>
        public string Fillter
        {
            get
            {
                var str = source;
                str = Regex.Replace(str, "\\s+", "");
                str = Regex.Replace(str, "[\n\r]", "");
                return str;
            }
        }

        /// <summary>
        /// 删除文本首尾特征字
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string RemoveTag(string tag)
        {
            if (string.IsNullOrEmpty(tag) || source.Length <= tag.Length)
            {
                return source;
            }

            var text = source[..tag.Length];
            checked
            {
                var text2 = source.Substring(source.Length - tag.Length, tag.Length);
                var result = source;
                if (text == tag)
                {
                    result = result.Substring(tag.Length, result.Length - tag.Length);
                }

                if (text2 == tag)
                {
                    result = result[..^tag.Length];
                }

                return result;
            }
        }

        /// <summary>
        /// 获取URL通配
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public string? GetUrl(string regex = "http[s]?://(?:(?!http[s]?://)[a-zA-Z]|[0-9]|[$\\-_@.&+/]|[!*\\(\\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+")
        {
            var enumerator = new System.Text.RegularExpressions.Regex(regex).Matches(source).GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    var temp = enumerator.Current;
                    if (temp == null) return null;
                    return ((System.Text.RegularExpressions.Match)temp)?.Value;
                }
            }
            finally
            {
                var disposable = enumerator as System.IDisposable;
                disposable?.Dispose();
            }

            return null;
        }

        /// <summary>
        /// 十六进制文本到字节集
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            try
            {
                var str = source;
                str = System.Text.RegularExpressions.Regex.Replace(str, "\\s+", "");
                str = System.Text.RegularExpressions.Regex.Replace(str, "[\n\r]", "");
                var hex = str;

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
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(source);
        }

        /// <summary>
        /// 文本到字节集
        /// </summary>
        /// <param name="encoding">编码模式</param>
        /// <returns></returns>
        public byte[] GetBytes(Encoding encoding)
        {
            return encoding.GetBytes(source);
        }

        /// <summary>
        /// 转换为无符号整数
        /// </summary>
        /// <returns></returns>
        public uint ToUint()
        {
            var result = 0u;
            if (!string.IsNullOrEmpty(source) && source.All(char.IsDigit))
            {
                result = uint.Parse(source);
            }

            return result;
        }

        /// <summary>
        /// 分割字符串到数组
        /// </summary>
        /// <param name="strSplit">用作分割的字符串</param>
        /// <returns></returns>
        public string[] Split(string strSplit)
        {
            var array = new string[1];
            var num = source.IndexOf(strSplit, 0, StringComparison.Ordinal);
            if (num < 0)
            {
                array[0] = source;
                return array;
            }

            array[0] = source[..num];
            return StringExtension.SplitRecursive(source[checked(num + strSplit.Length)..], strSplit, array);
        }

        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        private static string[] SplitRecursive(string strSource, string strSplit, string[] attachArray)
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
        /// <param name="left"></param>
        /// <returns></returns>
        public string Left(string left)
        {
            var num = source.IndexOf(left, StringComparison.Ordinal);
            if (num < 0)
            {
                return "";
            }

            return source[..num];
        }

        /// <summary>
        /// 取文本左边
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string Left(int len)
        {
            return len > source.Length ? "" : source[..len];
        }

        /// <summary>
        /// 取文本右边
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public string Right(string right)
        {
            var num = source.IndexOf(right, StringComparison.Ordinal);
            if (num < 0)
            {
                return "";
            }

            var length = source.Length;
            checked
            {
                if (length - num - right.Length <= 0)
                {
                    return "";
                }

                return source.Substring(num + right.Length, length - (num + right.Length));
            }
        }

        /// <summary>
        /// 取文本右边
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string Right(int len)
        {
            if (len > source.Length)
            {
                return "";
            }

            var startIndex = checked(source.Length - len);
            return source.Substring(startIndex, len);
        }

        /// <summary>
        /// 取文本中间
        /// </summary>
        /// <param name="left">左边标记</param>
        /// <param name="right">右边标记</param>
        /// <returns></returns>
        public string Between(string left, string right)
        {
            var num = source.IndexOf(left, StringComparison.Ordinal);
            checked
            {
                var num2 = source.IndexOf(right, num + left.Length, StringComparison.Ordinal);
                if (num < 0 || num2 < 0)
                {
                    return "";
                }

                num += left.Length;
                num2 -= num;
                return num2 < 0 ? "" : source.Substring(num, num2);
            }
        }

        /// <summary>
        /// 取文本中间数组
        /// </summary>
        /// <param name="left">左边标记</param>
        /// <param name="right">右边标记</param>
        /// <returns>得到的字符串数组</returns>
        public List<string> BetweenEx(string left, string right)
        {
            var list = new List<string>();
            var num = source.IndexOf(left, StringComparison.Ordinal);
            var length = left.Length;
            checked
            {
                while (num >= 0)
                {
                    var num2 = source.IndexOf(right, num + length, StringComparison.Ordinal);
                    if (num2 < 0)
                    {
                        break;
                    }

                    var item = source.Substring(num + length, num2 - num - length);
                    list.Add(item);
                    num = source.IndexOf(left, num2 + right.Length, StringComparison.Ordinal);
                }

                return list;
            }
        }

        /// <summary>
        /// 十六进制文本转十进制
        /// </summary>
        /// <returns></returns>
        public int HexToInt()
        {
            return Convert.ToInt32(source, 16);
        }

        /// <summary>
        /// 转换为整数
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            var result = 0;
            var str = source;
            if (!string.IsNullOrEmpty(str) && str.IndexOf('.') > -1)
            {
                var num = str.IndexOf(".", StringComparison.Ordinal);
                if (num >= 0)
                {
                    str = str[..num];
                }
            }

            if (!string.IsNullOrEmpty(str) && str.All(char.IsDigit))
            {
                result = int.Parse(str);
            }

            return result;
        }
    }
}
