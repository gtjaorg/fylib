namespace FyLib;

public static class BytesExtension
{
    /// <summary>
    /// 提供字节数组的扩展方法
    /// </summary>
    extension(byte[] source)
    {
        /// <summary>
        /// 获取字节数组的MD5哈希值的十六进制字符串表示形式，结果转换为小写格式
        /// </summary>
        /// <returns>MD5哈希值的十六进制字符串，格式为小写</returns>
        /// <remarks>
        /// 该扩展方法通过对字节数组计算MD5哈希值，然后将其转换为十六进制字符串表示形式。
        /// 结果字符串采用小写格式，便于在需要时进行比较或显示。
        /// </remarks>
        public string md5 => source.Md5().ToHex().ToLower();

        /// <summary>
        /// 获取字节数组的MD5哈希值的十六进制字符串表示形式
        /// </summary>
        /// <returns>MD5哈希值的十六进制字符串，格式为大写</returns>
        /// <remarks>
        /// 该扩展方法通过对字节数组计算MD5哈希值，然后将其转换为十六进制字符串表示形式。
        /// 结果字符串采用大写格式，便于在需要时进行比较或显示。
        /// </remarks>
        public string MD5 => source.Md5().ToHex();

        /// <summary>
        /// 获取字节数组转换为十六进制字符串表示形式的结果
        /// </summary>
        /// <returns>字节数组的十六进制字符串表示形式，各字节以两位十六进制数表示并连接</returns>
        /// <remarks>
        /// 该扩展方法将字节数组中的每个字节转换为两位的十六进制字符串表示形式，
        /// 并将所有字节的结果按顺序连接成一个完整的十六进制字符串。
        /// 结果字符串中的每个字节都使用大写格式表示。
        /// </remarks>
        public string Hex => source.ToHex();
        /// <summary>
        /// 获取字节数组格式化后的十六进制字符串表示形式，按每行16个字节进行换行分组
        /// </summary>
        /// <returns>格式化后的十六进制字符串，每行显示16个字节，每个字节用两个大写十六进制字符表示</returns>
        /// <remarks>
        /// 该扩展方法将字节数组转换为十六进制字符串表示形式，并按照每16个字节进行换行分组，
        /// 便于查看和调试。每个字节使用两个大写十六进制字符表示，各字节之间用空格分隔。
        /// </remarks>
        public string FormatHex => source.Format();

        /// <summary>
        /// 获取字节数组转换为Base64格式的字符串表示形式
        /// </summary>
        /// <returns>表示字节数组的Base64格式字符串</returns>
        /// <remarks>
        /// 该扩展方法将字节数组转换为Base64编码的字符串，便于在文本环境中传输或存储二进制数据。
        /// Base64编码是一种常用的编码方式，能够将二进制数据转换为ASCII字符集中的可打印字符。
        /// </remarks>
        public string Base64 => source.ToBase64();
    }
}