
// FyLib.Project
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using FyLib.Http;

using Newtonsoft.Json;

/// <summary>
/// 程序帮助类
/// </summary>
public static class Project
{
    /// <summary>
    /// 运行目录
    /// </summary>
    public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// 配置目录
    /// </summary>
    public static string ConfigDirectory => AppDomain.CurrentDomain.BaseDirectory + "config\\";

    /// <summary>
    /// 项目名称
    /// </summary>
    public static string? Name => Assembly.GetEntryAssembly()?.GetName().Name;

    /// <summary>
    /// 保存配置
    /// </summary>
    /// <returns></returns>
    public static bool SaveConfig(object config)
    {
        if (config == null)
        {
            return false;
        }
        try
        {
            string text = AppDomain.CurrentDomain.BaseDirectory + "config\\";
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            string path = text + "App.Config";
            string contents = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, contents);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 读取配置
    /// </summary>
    /// <returns></returns>
    public static T? ReadConfig<T>()
    {
        string text = AppDomain.CurrentDomain.BaseDirectory + "config\\";
        if (!Directory.Exists(text))
        {
            return default;
        }
        string path = text + "App.Config";
        if (!File.Exists(path))
        {
            return default;
        }
        try
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        catch (Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// 强制结束项目
    /// </summary>
    public static void ShutDown()
    {
        Process.GetCurrentProcess().Kill();
    }
    /// <summary>
    /// HttpClientPool
    /// </summary>
    public static HttpClientPool HttpPool { get; private set; } = new HttpClientPool();
}
