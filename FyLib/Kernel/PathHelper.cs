using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FyLib
{
    public static class PathHelper
    {
        /// <summary>
        /// 是否为常见的媒体格式:{mp3|mp4|aac|wav|avi|mov|mkv|flv|wmv|jpg|jpeg|png|bmp|gif}
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsMediaFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension == ".mp3" || extension == ".aac" || extension == ".wav" ||
                extension == ".mp4" || extension == ".avi" || extension == ".mov" ||
                extension == ".mkv" || extension == ".flv" || extension == ".wmv" ||
                extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                extension == ".bmp" || extension == ".gif")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
