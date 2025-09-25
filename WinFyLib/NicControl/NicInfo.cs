using System;
using System.Collections.Generic;
using System.Text;

namespace FyLib.NicControl
{
    public class NicInfo
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string? Caption { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 接口索引
        /// </summary>
        public int InterfaceIndex { get; set; }

        /// <summary>
        /// 网卡地址
        /// </summary>
        public string? MACAddress { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string[]? IPAddress { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public NetConnectionStatus? ConnectionStatus { get; set; }

        /// <summary>
        /// 活跃点
        /// </summary>
        public int IPConnectionMetric { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        public string? GUID { get; set; }
    }
    public enum NetConnectionStatus
    {
        断开的链接,
        连接中,
        已连接,
        断开,
        硬件不存在,
        硬件禁用,
        硬件故障,
        媒体断开连接,
        进行身份验证,
        身份验证成功,
        身份验证失败,
        无效地址,
        所需的凭据
    }
}



