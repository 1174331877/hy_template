using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LT_CachePool
{
    /// <summary>
    /// 对象池条目行为
    /// </summary>
    public interface IPoolEntry
    {
        /// <summary>
        /// 当对象池条目取出
        /// </summary>
        public void OnTake();

        /// <summary>
        /// 当对象池条目回收
        /// </summary>
        public void OnRecycle();
    }

    public abstract class AbsPoolEntry : IPoolEntry
    {
        public abstract void OnTake();

        public abstract void OnRecycle();
    }

    public abstract class AbsPool<T> : ILifecycle where T : IPoolEntry
    {
        //缓存队列
        private Queue<T> m_CacheEntrys = new Queue<T>();

        //默认生长长度
        private int m_GrowthCount;

        /// <summary>
        /// 生成缓存条目的HOOK方法
        /// </summary>
        public event Func<T> GeneratePoolEntryHook;

        public AbsPool(int growthCount)
        {
            m_GrowthCount = growthCount;
        }

        protected void Growth()
        {
            for (int i = 0; i < m_GrowthCount; i++)
            {
                var entry = GeneratePoolEntryHook.Invoke();
                entry.OnRecycle();
                m_CacheEntrys.Enqueue(entry);
            }
        }

        public T Take()
        {
            if (m_CacheEntrys.Count == 0)
            {
                Growth();
            }
            var entry = m_CacheEntrys.Dequeue();
            entry.OnTake();
            return entry;
        }

        public void Recycle(T entry)
        {
            if (!m_CacheEntrys.Contains(entry))
            {
                entry.OnRecycle();
                m_CacheEntrys.Enqueue(entry);
            }
        }

        public virtual void OnInit(ITuple tuple = null)
        {
        }

        public virtual void OnRemove()
        {
            m_CacheEntrys.Clear();
        }

        public virtual void OnInitFinish()
        {
        }
    }
}