using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyLib
{
    /// <summary>
    /// 文件扩展类
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// 文件或目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExists(this string path)
        {
            if (IsFile(path)) return true;
            return IsDirectory(path);
        }
        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectory(this string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// 是否为文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFile(this string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// 是否为媒体文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="MediaExtensions"></param>
        /// <returns></returns>
        public static bool IsMediaFile(this string filePath, List<string>? MediaExtensions =null  )
        {
            if (MediaExtensions == null)
            {
                MediaExtensions = new List<string>()
                {
                    ".mp3",".aac",".wav",".mp4",".avi",".mov",".mkv",".flv",".wmv",".jpg",".jpeg",".png", ".bmp",".gif"
                };
            }
            var extension = Path.GetExtension(filePath).ToLower();
            return MediaExtensions.Contains(extension);
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string? GetLength(this string filePath)
        {
            if (filePath.IsFile() == false) return null;
            var fileInfo = new FileInfo(filePath);
            var fileSizeInBytes = fileInfo.Length;
            var fileSizeFormatted = FormatFileSize(fileSizeInBytes);
            return fileSizeFormatted;
        }
        /// <summary>
        /// 将文件大小转换为适合阅读的格式
        /// </summary>
        /// <param name="fileSizeInBytes"></param>
        /// <returns></returns>
        public static string FormatFileSize(this long fileSizeInBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            var order = 0;
            double size = fileSizeInBytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return string.Format("{0:0.##} {1}", size, sizes[order]);
        }
    }
}
