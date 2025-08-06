using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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
        }
    }
}
