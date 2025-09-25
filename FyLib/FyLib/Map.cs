using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FyLib
{
    /// <summary>
/// 线程安全字典
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class Map<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _maps = new ConcurrentDictionary<TKey, TValue>();

    /// <summary>
    /// 所有的Key
    /// </summary>
    public List<TKey> Keys => _maps.Keys.ToList();

    /// <summary>
    /// 所有的Value
    /// </summary>
    public List<TValue> Values => _maps.Values.ToList();

    /// <summary>
    /// 数量
    /// </summary>
    public int Count => _maps.Count;

    /// <summary>
    /// 初始化
    /// </summary>
    public Map()
    {
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKey key, TValue value)
    {
        var value2 = value;
        _maps.AddOrUpdate(key, value2, (TKey k, TValue v) => value2);
    }

    /// <summary>
    /// 删除指定键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Remove(TKey key)
    {
        return _maps.TryRemove(key, out var value);
    }

    /// <summary>
    /// 获取Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue? GetValue(TKey key)
    {
        _maps.TryGetValue(key, out var value);
        return value;
    }

    /// <summary>
    /// 设置指定键值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool SetValue(TKey key, TValue value)
    {
        var value2 = value;
        return _maps.AddOrUpdate(key, value2, (TKey k, TValue v) => value2)?.Equals(value2) ?? false;
    }

    /// <summary>
    /// 转换为LIST
    /// </summary>
    /// <returns></returns>
    public List<KeyValuePair<TKey, TValue>> ToList()
    {
        return [.. _maps];
    }

    /// <summary>
    /// 清空MAP
    /// </summary>
    public void Clear()
    {
        _maps.Clear();
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~Map()
    {
        _maps.Clear();
    }
}
}

