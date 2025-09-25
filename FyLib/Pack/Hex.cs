using System;
using System.Collections.Generic;
using System.Text;

namespace FyLib.Pack
{
    /// <summary>
    /// 表示一段十六进制字符串，可以方便地转换为字节数组。
    /// </summary>
    public class Hex
    {
        /// <summary>
        /// byte[] 数据
        /// </summary>
        public byte[] bin => text.ToBytes();

        /// <summary>
        /// 16进制文本
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">16进制文本</param>
        public Hex(string value)
        {
            text = value;
        }
    }
}
