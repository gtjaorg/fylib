using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FyLib.API
{
    public static class user32
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessageA(IntPtr hwnd, int wMsg, int wParam, string lParam);

        /// <summary>
        /// 获取控件文本长度
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLengthA(IntPtr handle);

        /// <summary>
        /// 置控件文本
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowTextA(IntPtr handle, string str);
    }
}
