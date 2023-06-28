using System;

public static class CLRExtention
{
    /// <summary>
    /// 合并两个委托到一个新的委托对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="frist"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static T CombineDelegate<T>(this T frist, T second) where T : Delegate
    {
        return Delegate.Combine(frist, second) as T;
    }

    /// <summary>
    /// 源委托中是否包换目标委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool HasContainDelegate<T>(this T source, T target) where T : Delegate
    {
        bool ret = false;
        if (source == null)
        {
            throw new NullReferenceException("source delegate is null !");
        }
        else if (target == null)
        {
            throw new NullReferenceException("target delegate is null !");
        }
        else
        {
            var actions = source.GetInvocationList();
            foreach (var item in actions)
            {
                if (object.ReferenceEquals(item, target))
                {
                    ret = true;
                    break;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// 在源委托中移除目标委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T RemoveDelegate<T>(this T source, T target) where T : Delegate
    {
        T dirty = source;
        if (source.HasContainDelegate(target))
        {
            dirty = Delegate.Remove(source, target) as T;
        }
        return dirty;
    }
}
