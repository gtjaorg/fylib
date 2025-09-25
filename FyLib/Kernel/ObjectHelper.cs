﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Object扩展类
/// </summary>
public static class ObjectHelper
{
    /// <summary>
    /// 尝试将对象转换为int类型
    /// </summary>
    /// <param name="input"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryConvertToInt(this object input, out int result)
    {
        result = 0;
        if (input == null)
            return false;
        if (input is int intValue)
        {
            result = intValue;
            return true;
        }
        if (input is string strValue)
        {
            return int.TryParse(strValue, out result);
        }
        if (input is double doubleValue)
        {
            if (doubleValue >= int.MinValue && doubleValue <= int.MaxValue)
            {
                result = (int)doubleValue;
                return true;
            }
        }
        if (input is long longValue)
        {
            if (longValue >= int.MinValue && longValue <= int.MaxValue)
            {
                result = (int)longValue;
                return true;
            }
        }

        // 可以继续为其他数据类型添加类似的检查...

        return false;
    }
    /// <summary>
    /// 尝试将对象转换为List类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryConvertToListT<T>(object input, out List<T> result)
    {
        result = new List<T>();

        if (input is IEnumerable<T> enumerable)
        {
            result.AddRange(enumerable);
            return true;
        }

        if (input is IEnumerable enumerableNonGeneric)
        {
            foreach (var item in enumerableNonGeneric)
            {
                if (item is T castItem)
                {
                    result.Add(castItem);
                }
                else
                {
                    return false; // 子项类型不匹配
                }
            }
            return true;
        }

        return false;
    }
}

