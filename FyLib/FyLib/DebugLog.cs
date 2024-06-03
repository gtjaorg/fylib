
// FyLib.DebugLog
using System;
using System.IO;
using System.Text;
using System.Threading;

using FyLib;
using FyLib.API;

/// <summary>
/// 调试日志
/// </summary>
public class DebugLog
{
    /// <summary>
    /// 编辑框句柄
    /// </summary>
    public IntPtr m_iHandle;

    /// <summary>
    /// 互斥量
    /// </summary>
    private object m_pLock = new object();

    /// <summary>
    /// 输出标识 默认为Debug
    /// </summary>
    public string m_szTag = "debug";

    /// <summary>
    /// 是否显示时间
    /// </summary>
    public bool m_bShowTime = true;

    /// <summary>
    /// 是否显示线程
    /// </summary>
    public bool m_bShowThread = true;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="handle"></param>
    public void Init(IntPtr handle)
    {
        m_iHandle = handle;
    }

    /// <summary>
    /// 输出日志
    /// </summary>
    /// <param name="str"></param>
    /// <param name="tag"></param>
    public void WriteLog(object str, string tag = "")
    {
        if (m_iHandle == IntPtr.Zero)
        {
            return;
        }
        string text = "";
        StringBuilder stringBuilder = new StringBuilder();
        if (m_bShowTime)
        {
            stringBuilder.Append(DateTime.Now.ToString());
            stringBuilder.Append(" ");
        }
        if (m_bShowThread)
        {
            stringBuilder.Append("线程:" + kernel32.GetCurrentThreadId());
            stringBuilder.Append(" ");
        }
        if (tag == "")
        {
            tag = m_szTag;
        }
        if (tag != "")
        {
            tag = "[" + tag + "]";
            stringBuilder.Append(tag);
            stringBuilder.Append(" ");
        }
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Append("-> ");
        }
        if (typeof(byte[]) == str.GetType())
        {
            stringBuilder.Append(((byte[])str).Format());
        }
        else
        {
            stringBuilder.Append(str.ToString());
        }
        stringBuilder.Append("\r\n");
        text = stringBuilder.ToString();
        lock (m_pLock)
        {
            int windowTextLengthA = user32.GetWindowTextLengthA(m_iHandle);
            if (windowTextLengthA >= checked(65535 - windowTextLengthA))
            {
                user32.SetWindowTextA(m_iHandle, "");
            }
            user32.SendMessage(m_iHandle, 177, -2, -1);
            user32.SendMessageA(m_iHandle, 194, 1, text);
        }
    }

    /// <summary>
    /// 清除日志
    /// </summary>
    public void ClearEdit()
    {
        user32.SetWindowTextA(m_iHandle, "");
    }

    /// <summary>
    /// 保存日志
    /// </summary>
    /// <param name="log">文本</param>
    /// <param name="filename">文件名 默认后缀.txt</param>
    public void SaveLog(string log, string filename)
    {
        string text = Thread.GetDomain().BaseDirectory + "\\save";
        lock (m_pLock)
        {
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            if (filename.Right(".").IsNullOrEmpty())
            {
                filename += ".txt";
            }
            new WirteLog(text + "\\" + filename).WriteLine(log);
        }
    }
}
