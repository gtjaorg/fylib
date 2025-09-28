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
        /// 判断是否为数值
        /// </summary>
        public bool IsNumeric
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                {
                    return false;
                }
                return double.TryParse(source,out _);
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
        /// 检查当前字符串是否全部由数字字符（0-9）组成，且不为空。
        /// </summary>
        /// <value>当字符串只包含数字时返回 <c>true</c>；否则返回 <c>false</c>。</value>
        public bool IsAllDigits => !string.IsNullOrEmpty(source) && source.AsSpan().IndexOfAnyExcept('0','9') < 0;

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
        public JToken? ToJson()
        {
            try
            {
                var obj = JToken.Parse(source);
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
            var math = Regex.Match(source, regex);
            return math.Success ? math.Value : null;
        }

        /// <summary>
        /// 十六进制文本到字节集
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() =>
            string.IsNullOrWhiteSpace(source) ? [] :
                Convert.FromHexString(string.Concat(source.Where(c => !char.IsWhiteSpace(c))));


        /// <summary>
        /// 文本到字节集
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()=>Encoding.UTF8.GetBytes(source);
        
        /// <summary>
        /// 文本到字节集
        /// </summary>
        /// <param name="encoding">编码模式</param>
        /// <returns></returns>
        public byte[] GetBytes(Encoding encoding)=>encoding.GetBytes(source);
        
        /// <summary>
        /// 转换为无符号整数
        /// </summary>
        /// <returns></returns>
        public uint? ToUint() => uint.TryParse(source, out var result) ? result : null;

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
        public string Left(int len)=>len > source.Length ? "" : source[..len];


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
        public string Right(int len) => len > source.Length ? "" : source[..len];
 

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
        public int HexToInt()=> Convert.ToInt32(source, 16);


        /// <summary>
        /// 将字符串转换为整数。如果字符串包含小数点，则仅取小数点前的部分进行解析。
        /// 当输入为 null、空字符串或不完全由数字组成时，返回 0。
        /// </summary>
        /// <returns>解析得到的整数值；若无法转换则返回 0。</returns>
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
