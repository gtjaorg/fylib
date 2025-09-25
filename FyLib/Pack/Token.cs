using System;
using System.Collections.Generic;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// 表示一个数据单元，包含字节数组和可选的标记对象。
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 数据体
        /// </summary>
        public byte[] bin { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int leng => bin.Length;

        /// <summary>
        /// 标记
        /// </summary>
        public object? tag { get; set; }

        /// <summary>
        /// 初始化Token
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="Tag">自定义标记</param>
        public Token(byte[] value, object? Tag = null)
        {
            bin = value;
            tag = Tag;
        }

        /// <summary>
        /// 初始化Token
        /// </summary>
        /// <param name="value">文本</param>
        /// <param name="Tag">自定义标记</param>
        public Token(string value, object? Tag = null)
        {
            bin = value.GetBytes();
            tag = Tag;
        }

        /// <summary>
        /// 初始化Token
        /// </summary>
        /// <param name="value">Hex</param>
        /// <param name="Tag">自定义标记</param>
        public Token(Hex value, object? Tag = null)
        {
            bin = value.bin;
            tag = Tag;
        }
    }
}
