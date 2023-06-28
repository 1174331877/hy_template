using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LT_Kernel
{
    /// <summary>
    /// 模块化的抽象实现
    /// </summary>
    public abstract class AbsModule : AutoCancleToken, ILifecycle, IFixedUpdate, IUpdate, ILateUpdate, IIsUpdate
    {
        #region 基础生命周期

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
            InitMgrs();
        }

        public override void OnRemove()
        {
            var valueEnumerator = mgrs.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var current = valueEnumerator.Current;
                if (current != null)
                {
                    current.OnRemove();
                }
            }
            mgrs.Clear();
            base.OnRemove();
        }

        #endregion 基础生命周期

        #region 生命周期更新行为

        public bool IsUpdate { get; set; } = true;

        public virtual void OnFixedUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            var valueEnumerator = mgrs.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var current = valueEnumerator.Current;
                if (current != null && current.IsUpdate)
                {
                    current.OnFixedUpdate(delta);
                }
            }
        }

        public virtual void OnUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            var valueEnumerator = mgrs.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var current = valueEnumerator.Current;
                if (current != null && current.IsUpdate)
                {
                    current.OnUpdate(delta);
                }
            }
        }

        public virtual void OnLateUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            var valueEnumerator = mgrs.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var current = valueEnumerator.Current;
                if (current != null && current.IsUpdate)
                {
                    current.OnLateUpdate(delta);
                }
            }
        }

        #endregion 生命周期更新行为

        #region 控制管理器行为

        /// <summary>
        /// 管理器缓存容器
        /// </summary>
        private Dictionary<Type, AbsMgr> mgrs = new Dictionary<Type, AbsMgr>();

        /// <summary>
        /// 初始化该模块下所有的管理器
        /// </summary>
        protected abstract void InitMgrs();

        /// <summary>
        /// 注册管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T RegisterMgr<T>() where T : AbsMgr, new()
        {
            Type mgrType = typeof(T);
            T mgr;
            if (mgrs.ContainsKey(mgrType))
            {
                throw new Exception($"{GetType().FullName} has register {mgrType.FullName}!");
            }
            else
            {
                mgr = new T();
                mgrs.Add(mgrType, mgr);
                mgr.OnInit();
            }
            return mgr;
        }

        /// <summary>
        /// 注销管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterMgr<T>() where T : AbsMgr
        {
            Type mgrType = typeof(T);
            if (mgrs.TryGetValue(mgrType, out AbsMgr mgr))
            {
                if (mgr != null)
                {
                    mgrs.Remove(mgrType);
                    mgr.OnRemove();
                }
            }
        }

        /// <summary>
        /// 检索一个管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T RetriveMgr<T>() where T : AbsMgr
        {
            Type mgrType = typeof(T);
            mgrs.TryGetValue(mgrType, out AbsMgr ret);
            if (ret == null)
            {
                throw new Exception($"{GetType().FullName} not register {mgrType.FullName}!");
            }
            return ret as T;
        }

        /// <summary>
        /// 检索一种基类管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T RetriveBaseMgr<T>() where T : AbsMgr
        {
            T ret = default;
            foreach (var item in mgrs.Values)
            {
                if (item is T)
                {
                    ret = item as T;
                    break;
                }
            }
            return ret;
        }

        #endregion 控制管理器行为
    }
}