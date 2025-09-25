namespace FyLib.API
{
    /// <summary>
    /// 指定 <c>GetComputerNameEx</c> 等 Windows API 返回的计算机名称格式。
    /// </summary>
    public enum COMPUTER_NAME_FORMAT
    {
        /// <summary>
        /// NetBIOS 名称，例如 <c>WORKSTATION</c>。
        /// </summary>
        ComputerNameNetBIOS,
        /// <summary>
        /// DNS 主机名（不包含域部分），例如 <c>machine</c>。
        /// </summary>
        ComputerNameDnsHostname,
        /// <summary>
        /// DNS 域名（不包含主机名部分），例如 <c>example.com</c>。
        /// </summary>
        ComputerNameDnsDomain,
        /// <summary>
        /// 完整限定的 DNS 名称，例如 <c>machine.example.com</c>。
        /// </summary>
        ComputerNameDnsFullyQualified,
        /// <summary>
        /// 物理（未加入域之前的）NetBIOS 名称。
        /// </summary>
        ComputerNamePhysicalNetBIOS,
        /// <summary>
        /// 物理 DNS 主机名（反映底层物理主机），不含域。
        /// </summary>
        ComputerNamePhysicalDnsHostname,
        /// <summary>
        /// 物理 DNS 域名，不含主机名部分。
        /// </summary>
        ComputerNamePhysicalDnsDomain,
        /// <summary>
        /// 物理完整限定 DNS 名称。
        /// </summary>
        ComputerNamePhysicalDnsFullyQualified
    }
}
