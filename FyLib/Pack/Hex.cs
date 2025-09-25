using System;
using System.Collections.Generic;
using System.Text;

namespace FyLib.Pack
{
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
