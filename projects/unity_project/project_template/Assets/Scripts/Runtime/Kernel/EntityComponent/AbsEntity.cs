using LT_CachePool;
using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_EntityComponent
{
    /// <summary>
    /// GameObject游戏对象抽象实体类
    /// </summary>
    public abstract class AbsEntity : AutoCancleToken, ILifecycle, IIsUpdate, IFixedUpdate, IUpdate, ILateUpdate, IEntityMessage, IActive, IPoolEntry
    {
        /// <summary>
        /// 组件包装器
        /// </summary>
        private sealed class AbsComponentWrap
        {
            /// <summary>
            /// 组件类型
            /// </summary>
            public Type ComType { get; private set; }

            /// <summary>
            /// 组件列表
            /// </summary>
            public List<AbsComponent> components { get; } = new List<AbsComponent>();

            public AbsComponentWrap(Type type, AbsComponent absComponent)
            {
                ComType = type;
                components.Add(absComponent);
            }
        }

        private List<AbsComponentWrap> absComponentWraps = new List<AbsComponentWrap>();

        /// <summary>
        /// 该实体所映射的GameObject游戏对象的Transform组件
        /// </summary>
        public readonly Transform transform;

        /// <summary>
        /// 实体的tag
        /// </summary>
        public readonly string entityTag;

        /// <summary>
        /// 实体的id索引
        /// </summary>
        public readonly long id;

        /// <summary>
        /// 实体参数
        /// </summary>
        protected ITuple EntityParam;

        /// <summary>
        /// 该实体对象对应的GameObject资源key
        /// </summary>
        public string ResKey = string.Empty;

        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="entityTag">entityTag必须要在UnityEditor中定义过</param>
        /// <param name="id"></param>
        /// <param name="tuple">实体所带参数</param>
        public AbsEntity(Transform transform, string entityTag, long id, ITuple tuple = null)
        {
            this.transform = transform;
            this.entityTag = entityTag;
            this.id = id;
            EntityParam = tuple;
            if (transform == null) return;
            transform.tag = entityTag;
        }

        public override void OnInit(ITuple tuple = null)
        {
            AddComponents();
        }

        public virtual void OnInitFinish()
        {
        }

        public override void OnRemove()
        {
            IsActive = false;
            RemoveAllCom();
            if (transform != null)
            {
                GameObject.Destroy(transform.gameObject);
            }
            base.OnRemove();
        }

        public bool IsUpdate { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public virtual void OnFixedUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                for (int j = 0; j < item.components.Count; j++)
                {
                    var com = item.components[j];
                    if (IsActive && com != null && com.IsActive)
                    {
                        (com as IFixedUpdate)?.OnFixedUpdate(delta);
                    }
                }
            }
        }

        public virtual void OnUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                for (int j = 0; j < item.components.Count; j++)
                {
                    var com = item.components[j];
                    if (IsActive && com != null && com.IsActive)
                    {
                        (com as IUpdate)?.OnUpdate(delta);
                    }
                }
            }
        }

        public virtual void OnLateUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                for (int j = 0; j < item.components.Count; j++)
                {
                    var com = item.components[j];
                    if (IsActive && com != null && com.IsActive)
                    {
                        (com as ILateUpdate)?.OnLateUpdate(delta);
                    }
                }
            }
        }

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="component"></param>
        /// <param name="componentParam">组件参数</param>
        /// <returns></returns>
        public T AddCom<T>(Component component = null, ITuple componentParam = null) where T : AbsComponent
        {
            Type comType = typeof(T);
            var attribute = comType.GetCustomAttribute(typeof(DisallowMultipleComponent));
            T ret;
            if (attribute != null)
            {
                if (HasCom<T>())
                {
                    throw new Exception($"{this.GetType().FullName} has add one {comType.FullName} and exist DisallowMultipleComponentAttribute constraint");
                }
                else
                {
                    ret = Activator.CreateInstance(comType, this, component, componentParam) as T;
                    absComponentWraps.Add(new AbsComponentWrap(comType, ret));
                    ret.OnInit();
                }
            }
            else
            {
                ret = Activator.CreateInstance(comType, this, component, componentParam) as T;

                if (HasCom<T>())
                {
                    var absComponentWrap = GetComponentWrap(comType);
                    absComponentWrap.components.Add(ret);
                }
                else
                {
                    absComponentWraps.Add(new AbsComponentWrap(comType, ret));
                }
                ret.OnInit();
            }
            return ret;
        }

        private AbsComponentWrap GetComponentWrap(Type type)
        {
            AbsComponentWrap ret = default;
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                if (item.ComType == type)
                {
                    ret = item;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 移除一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        public void RemoveCom<T>(T com) where T : AbsComponent
        {
            if (HasCom<T>())
            {
                Type comType = typeof(T);
                var absComponentWrap = GetComponentWrap(comType);
                for (int i = 0; i < absComponentWrap.components.Count; i++)
                {
                    var com_ = absComponentWrap.components[i];
                    if (com_ == com)
                    {
                        absComponentWrap.components.Remove(com_);
                        com_.OnRemove();
                        if (absComponentWrap.components.Count == 0)
                        {
                            absComponentWraps.Remove(absComponentWrap);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除该类型的所有组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveAllCom<T>() where T : AbsComponent
        {
            if (HasCom<T>())
            {
                Type comType = typeof(T);
                var absComponentWrap = GetComponentWrap(comType);
                for (int i = 0; i < absComponentWrap.components.Count; i++)
                {
                    var com = absComponentWrap.components[i];
                    com.OnRemove();
                }
                absComponentWraps.Remove(absComponentWrap);
            }
        }

        /// <summary>
        /// 移除全部组件
        /// </summary>
        public void RemoveAllCom()
        {
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                for (int j = 0; j < item.components.Count; j++)
                {
                    var com = item.components[j];
                    com?.OnRemove();
                }
            }
            absComponentWraps.Clear();
        }

        /// <summary>
        /// 获取一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCom<T>() where T : AbsComponent
        {
            T ret = default;
            if (HasCom<T>())
            {
                Type comType = typeof(T);
                var absComponentWrap = GetComponentWrap(comType);
                if (absComponentWrap.components.Count > 0)
                {
                    ret = absComponentWrap.components[absComponentWrap.components.Count - 1] as T;
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取一个父类组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBaseCom<T>() where T : AbsComponent
        {
            T ret = null;
            foreach (var item in absComponentWraps)
            {
                var lastCom = item.components[item.components.Count - 1];
                if (lastCom is T)
                {
                    ret = lastCom as T;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 是否存在该类型组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasCom<T>() where T : AbsComponent
        {
            Type comType = typeof(T);
            bool ret = false;
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                if (item.ComType == comType)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public bool TryGetCom<T>(out T com) where T : AbsComponent
        {
            bool ret = false;
            Type comType = typeof(T);
            com = null;
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                if (item.ComType == comType)
                {
                    ret = true;
                    com = item.components[item.components.Count - 1] as T;
                    break;
                }
            }
            return ret;
        }

        public void OnMsg(Enum eventMsg, ITuple tuple = null)
        {
            for (int i = 0; i < absComponentWraps.Count; i++)
            {
                var item = absComponentWraps[i];
                for (int j = 0; j < item.components.Count; j++)
                {
                    var com = item.components[j];
                    if (com != null && com.IsActive)
                    {
                        com.OnMsg(eventMsg, tuple);
                    }
                }
            }
        }

        public void OnMsg<T>(Enum eventMsg, ITuple tuple = null) where T : AbsComponent
        {
            if (HasCom<T>())
            {
                Type comType = typeof(T);
                var absComponentWrap = GetComponentWrap(comType);
                for (int i = 0; i < absComponentWrap.components.Count; i++)
                {
                    var com = absComponentWrap.components[i];
                    if (com != null && com.IsActive)
                    {
                        com.OnMsg(eventMsg, tuple);
                    }
                }
            }
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        protected abstract void AddComponents();

        /// <summary>
        /// 设置该实体对象是否激活
        /// </summary>
        /// <param name="isActive"></param>
        public virtual void SetActive(bool isActive)
        {
            if (transform.gameObject.activeSelf != isActive)
            {
                transform.gameObject.SetActive(isActive);
                IsActive = isActive;
            }
        }

        public virtual void OnTake()
        {
        }

        public virtual void OnRecycle()
        {
        }
    }
}