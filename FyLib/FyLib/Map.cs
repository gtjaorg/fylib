using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 线程安全字典
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class Map<TKey, TValue>
{
    private ConcurrentDictionary<TKey, TValue> maps = new ConcurrentDictionary<TKey, TValue>();

    /// <summary>
    /// 所有的Key
    /// </summary>
    public List<TKey> keys => maps.Keys.ToList();

    /// <summary>
    /// 所有的Value
    /// </summary>
    public List<TValue> values => maps.Values.ToList();

    /// <summary>
    /// 数量
    /// </summary>
    public int count => maps.Count;

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
        TValue value2 = value;
        maps.AddOrUpdate(key, value2, (TKey k, TValue v) => value2);
    }

    /// <summary>
    /// 删除指定键
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Remove(TKey key)
    {
        TValue value;
        return maps.TryRemove(key, out value);
    }

    /// <summary>
    /// 获取Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue GetValue(TKey key)
    {
        TValue value = default;
        maps.TryGetValue(key, out value);
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
        TValue value2 = value;
        return maps.AddOrUpdate(key, value2, (TKey k, TValue v) => value2)?.Equals(value2) ?? false;
    }

    /// <summary>
    /// 转换为LIST
    /// </summary>
    /// <returns></returns>
    public List<KeyValuePair<TKey, TValue>> ToList()
    {
        return [.. maps];
    }

    /// <summary>
    /// 清空MAP
    /// </summary>
    public void Clear()
    {
        maps.Clear();
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~Map()
    {
        maps.Clear();
    }
}
