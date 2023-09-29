using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FyLib.API
{
    public static class shlwapi
    {
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr PathFindExtension(string filename);

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr PathFindFileName(string filename);

        /// <summary>
        /// 删除文件扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void PathRemoveExtension(string filename);
    }
}
