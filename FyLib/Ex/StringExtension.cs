using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                return double.TryParse(source, out _);
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
        public bool IsAllDigits => !string.IsNullOrEmpty(source) && source.AsSpan().IndexOfAnyExcept("0123456789") < 0;

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
        /// SHA256 加密
        /// </summary>
        public string Sha256
        {
            get
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        /// <summary>
        /// SHA1 加密
        /// </summary>
        public string Sha1
        {
            get
            {
                using var sha1 = System.Security.Cryptography.SHA1.Create();
                var bytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = sha1.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        /// <summary>
        /// SHA512 加密
        /// </summary>
        public string Sha512
        {
            get
            {
                using var sha512 = System.Security.Cryptography.SHA512.Create();
                var bytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = sha512.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        /// <summary>
        /// CRC32 校验
        /// </summary>
        public string Crc32
        {
            get
            {
                var bytes = Encoding.UTF8.GetBytes(source);
                return bytes.Crc32.ToString("X8");
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
        /// 将字符串转换为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>转换后的对象，如果转换失败则返回null</returns>
        public T? ToObject<T>() where T : class
        {
            var obj = source.ToJToken();
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
        /// 转换为十进制
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal()
        {
            if (decimal.TryParse(source, out var result))
            {
                return result;
            }
            return 0;
        }
        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ToDouble()
        {
            if (double.TryParse(source, out var result))
            {
                return result;
            }
            return 0;
        }
        /// <summary>
        /// 转换为长整数
        /// </summary>
        /// <returns></returns>
        public long ToLong()
        {
            if (long.TryParse(source, out var result))
            {
                return result;
            }
            return 0;
        }
        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <returns></returns>
        public bool ToBool()
        {
            if (bool.TryParse(source, out var result))
            {
                return result;
            }
            return false;
        }
        /// <summary>
        /// 转换为日期时间
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            if (DateTime.TryParse(source, out var result))
            {
                return result;
            }
            return DateTime.MinValue;
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
        public string Filter
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
        public byte[] GetBytes() => Encoding.UTF8.GetBytes(source);

        /// <summary>
        /// 文本到字节集
        /// </summary>
        /// <param name="encoding">编码模式</param>
        /// <returns></returns>
        public byte[] GetBytes(Encoding encoding) => encoding.GetBytes(source);

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
        public string Left(int len) => len > source.Length ? "" : source[..len];


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
        public string Right(int len) => len > source.Length ? "" : source[^len..];


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
        public int HexToInt() => Convert.ToInt32(source, 16);


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

        /// <summary>
        /// BASE64 解码
        /// </summary>
        public string FromBase64
        {
            get
            {
                try
                {
                    var bytes = Convert.FromBase64String(source);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// URL 编码
        /// </summary>
        public string UrlEncode => Uri.EscapeDataString(source);

        /// <summary>
        /// URL 解码
        /// </summary>
        public string UrlDecode => Uri.UnescapeDataString(source);

        /// <summary>
        /// HTML 编码
        /// </summary>
        public string HtmlEncode => WebUtility.HtmlEncode(source);

        /// <summary>
        /// HTML 解码
        /// </summary>
        public string HtmlDecode => WebUtility.HtmlDecode(source);

        /// <summary>
        /// 判断是否为有效的电子邮箱地址
        /// </summary>
        public bool IsEmail
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return false;

                try
                {
                    var addr = new System.Net.Mail.MailAddress(source);
                    return addr.Address == source;
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断是否为有效的QQ号码
        /// </summary>
        public bool IsQQ
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return false;
                if (source.Length < 5 || source.Length > 12)
                    return false;
                return Regex.IsMatch(source, "^[1-9][0-9]{4,}$");
            }
        }

        /// <summary>
        /// 判断是否为有效的手机号码（中国大陆）
        /// </summary>
        public bool IsPhone
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return false;

                return Regex.IsMatch(source, @"^1[3-9]\d{9}$");
            }
        }

        /// <summary>
        /// 判断是否为有效的身份证号码（中国大陆）
        /// </summary>
        public bool IsIdCard
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return false;

                // 18位身份证号码正则
                if (!Regex.IsMatch(source, @"^\d{17}[\dXx]$"))
                    return false;

                // 验证校验码
                var weights = new[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                var checkCodes = new[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

                var sum = 0;
                for (var i = 0; i < 17; i++)
                {
                    sum += (source[i] - '0') * weights[i];
                }

                var checkCode = checkCodes[sum % 11];
                return char.ToUpper(source[17]) == checkCode;
            }
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        public string Capitalize
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                if (source.Length == 1)
                    return source.ToUpper();

                return char.ToUpper(source[0]) + source[1..];
            }
        }

        /// <summary>
        /// 驼峰命名转换（首字母小写）
        /// </summary>
        public string ToCamelCase
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                var words = source.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0)
                    return source;

                var result = new StringBuilder();
                result.Append(words[0].ToLower());

                for (var i = 1; i < words.Length; i++)
                {
                    if (words[i].Length > 0)
                    {
                        result.Append(char.ToUpper(words[i][0]));
                        if (words[i].Length > 1)
                            result.Append(words[i][1..].ToLower());
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 帕斯卡命名转换（首字母大写）
        /// </summary>
        public string ToPascalCase
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                var words = source.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0)
                    return source;

                var result = new StringBuilder();
                foreach (var word in words)
                {
                    if (word.Length > 0)
                    {
                        result.Append(char.ToUpper(word[0]));
                        if (word.Length > 1)
                            result.Append(word[1..].ToLower());
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 下划线命名转换
        /// </summary>
        public string ToSnakeCase
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                var result = new StringBuilder();
                for (var i = 0; i < source.Length; i++)
                {
                    var c = source[i];
                    if (char.IsUpper(c) && i > 0 && char.IsLower(source[i - 1]))
                    {
                        result.Append('_');
                    }
                    result.Append(char.ToLower(c));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 中划线命名转换
        /// </summary>
        public string ToKebabCase
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                var result = new StringBuilder();
                for (var i = 0; i < source.Length; i++)
                {
                    var c = source[i];
                    if (char.IsUpper(c) && i > 0 && char.IsLower(source[i - 1]))
                    {
                        result.Append('-');
                    }
                    result.Append(char.ToLower(c));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        public string Reverse
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return source;

                var chars = source.ToCharArray();
                Array.Reverse(chars);
                return new string(chars);
            }
        }

        /// <summary>
        /// 重复字符串指定次数
        /// </summary>
        /// <param name="count">重复次数</param>
        /// <returns></returns>
        public string Repeat(int count)
        {
            if (count <= 0 || string.IsNullOrEmpty(source))
                return string.Empty;

            var result = new StringBuilder(source.Length * count);
            for (var i = 0; i < count; i++)
            {
                result.Append(source);
            }
            return result.ToString();
        }

        /// <summary>
        /// 移除所有指定的字符
        /// </summary>
        /// <param name="chars">要移除的字符数组</param>
        /// <returns></returns>
        public string RemoveChars(params char[] chars)
        {
            if (string.IsNullOrEmpty(source) || chars == null || chars.Length == 0)
                return source;

            return string.Concat(source.Where(c => !chars.Contains(c)));
        }

        /// <summary>
        /// 只保留数字字符
        /// </summary>
        public string OnlyDigits => string.IsNullOrEmpty(source) ? source : string.Concat(source.Where(char.IsDigit));

        /// <summary>
        /// 只保留字母字符
        /// </summary>
        public string OnlyLetters => string.IsNullOrEmpty(source) ? source : string.Concat(source.Where(char.IsLetter));

        /// <summary>
        /// 只保留字母和数字字符
        /// </summary>
        public string OnlyAlphaNumeric => string.IsNullOrEmpty(source) ? source : string.Concat(source.Where(char.IsLetterOrDigit));

        /// <summary>
        /// 安全截取字符串，超出长度则添加省略号
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        /// <param name="ellipsis">省略号，默认为"..."</param>
        /// <returns></returns>
        public string Truncate(int maxLength, string ellipsis = "...")
        {
            if (string.IsNullOrEmpty(source) || maxLength <= 0)
                return source;

            if (source.Length <= maxLength)
                return source;

            var truncateLength = maxLength - ellipsis.Length;
            if (truncateLength <= 0)
                return ellipsis[..maxLength];

            return source[..truncateLength] + ellipsis;
        }

        /// <summary>
        /// 计算字符串的字节长度（UTF-8编码）
        /// </summary>
        public int ByteLength => string.IsNullOrEmpty(source) ? 0 : Encoding.UTF8.GetByteCount(source);

        /// <summary>
        /// 判断是否包含中文字符
        /// </summary>
        public bool ContainsChinese
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return false;

                return source.Any(c => c >= 0x4e00 && c <= 0x9fbb);
            }
        }

        /// <summary>
        /// 获取中文字符数量
        /// </summary>
        public int ChineseCount
        {
            get
            {
                if (string.IsNullOrEmpty(source))
                    return 0;

                return source.Count(c => c >= 0x4e00 && c <= 0x9fbb);
            }
        }

        /// <summary>
        /// 脱敏处理 - 手机号
        /// </summary>
        public string MaskPhone
        {
            get
            {
                if (string.IsNullOrEmpty(source) || source.Length != 11)
                    return source;

                return $"{source[..3]}****{source[7..]}";
            }
        }

        /// <summary>
        /// 脱敏处理 - 邮箱
        /// </summary>
        public string MaskEmail
        {
            get
            {
                if (string.IsNullOrEmpty(source) || !source.Contains('@'))
                    return source;

                var parts = source.Split('@');
                if (parts.Length != 2)
                    return source;

                var name = parts[0];
                var domain = parts[1];

                if (name.Length <= 2)
                    return $"{name[0]}***@{domain}";

                var maskedName = name.Length <= 4
                    ? $"{name[0]}***{name[^1]}"
                    : $"{name[..2]}***{name[^2..]}";

                return $"{maskedName}@{domain}";
            }
        }

        /// <summary>
        /// 脱敏处理 - 身份证
        /// </summary>
        public string MaskIdCard
        {
            get
            {
                if (string.IsNullOrEmpty(source) || source.Length != 18)
                    return source;

                return $"{source[..6]}********{source[^4..]}";
            }
        }
        /// <summary>
        /// 按行分割（自动处理\r\n和\n）
        /// </summary>
        public string[] SplitLines =>
            source.Split(["\r\n", "\n"], StringSplitOptions.None);

        /// <summary>
        /// 按行分割（移除空行）
        /// </summary>
        public string[] SplitLinesRemoveEmpty =>
            source.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// 判断是否在指定范围内的整数
        /// </summary>
        public bool IsIntInRange(int min, int max) =>
            int.TryParse(source, out var value) && value >= min && value <= max;

        /// <summary>
        /// 判断是否为浮点数
        /// </summary>
        public bool IsFloat => float.TryParse(source, out _);
        /// <summary>
        /// 全角转半角
        /// </summary>
        public string ToHalfWidth
        {
            get
            {
                var result = new StringBuilder();
                foreach (var c in source)
                {
                    if (c == '\u3000')
                        result.Append(' ');
                    else if (c > '\uFF00' && c < '\uFF5F')
                        result.Append((char)(c - 0xFEE0));
                    else
                        result.Append(c);
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// 半角转全角
        /// </summary>
        public string ToFullWidth
        {
            get
            {
                var result = new StringBuilder();
                foreach (var c in source)
                {
                    if (c == ' ')
                        result.Append('\u3000');
                    else if (c > '\u0020' && c < '\u007F')
                        result.Append((char)(c + 0xFEE0));
                    else
                        result.Append(c);
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// 判断是否为有效的文件路径
        /// </summary>
        public bool IsValidFilePath
        {
            get
            {
                try
                {
                    return !string.IsNullOrWhiteSpace(source) &&
                           Path.GetFullPath(source) != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取文件扩展名（含点）
        /// </summary>
        public string FileExtension => Path.GetExtension(source);

        /// <summary>
        /// 获取文件名（不含路径）
        /// </summary>
        public string FileName => Path.GetFileName(source);

        /// <summary>
        /// 获取文件名（不含扩展名）
        /// </summary>
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(source);

        /// <summary>
        /// 获取目录名
        /// </summary>
        public string DirectoryName => Path.GetDirectoryName(source) ?? string.Empty;
    }

}
