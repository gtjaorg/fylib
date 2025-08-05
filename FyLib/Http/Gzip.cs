using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyLib
{
    /// <summary>
    /// Gzip压缩类
    /// </summary>
    public class Gzip
    {
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decode(byte[] bytes)
        {
            using (var compressedStream = new MemoryStream(bytes))
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                // 将解压缩的数据写入 decompressedStream
                gzipStream.CopyTo(decompressedStream);
                // 将解压缩后的字节数组转换为字符串
                var decompressedData = decompressedStream.ToArray();
                return decompressedData;
            }
        }
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] Encode(byte[] inputBytes)
        {
            byte[] compressedBytes;
            using (var compressedStream = new MemoryStream())
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                // 将原始数据写入 GZipStream 进行压缩
                gzipStream.Write(inputBytes, 0, inputBytes.Length);
                // 确保所有数据都被写入压缩流
                gzipStream.Close();
                // 返回压缩后的字节数组
                compressedBytes = compressedStream.ToArray();
            }
            return compressedBytes;
        }
    }
}
