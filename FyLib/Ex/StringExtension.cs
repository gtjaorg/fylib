using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FyLib
{
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
            /// MD5 小写
            /// </summary>
            public string md5 => source.Md5().ToLower();

            /// <summary>
            /// MD5 大写
            /// </summary>
            public string MD5 => source.Md5();

            /// <summary>
            /// 是否为空或空字符串
            /// </summary>
            public bool IsNullOrEmpty => string.IsNullOrEmpty(source);

            /// <summary>
            /// 是否为空或空白字符串
            /// </summary>
            public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(source);

            /// <summary>
            /// 是否为IP地址
            /// </summary>
            public bool IsIp => source.IsIp();

            /// <summary>
            /// 是否为整数
            /// </summary>
            public bool IsInt => source.IsNumeric();

            /// <summary>
            /// source是否为数字(整数或小数)
            /// </summary>
            public bool IsDecimal => decimal.TryParse(source, out _);

            /// <summary>
            /// BASE64 编码
            /// </summary>
            public string Base64 => source.Base64();

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
        }
    }
}