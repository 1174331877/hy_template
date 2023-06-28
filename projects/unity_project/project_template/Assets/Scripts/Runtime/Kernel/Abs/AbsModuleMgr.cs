using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LT_Kernel
{
    public abstract class AbsModuleMgr : AbsMgr
    {
        public override void OnInit(ITuple tuple = null)
        {
            IsUpdate = true;
            base.OnInit();
            InitModules();
        }

        public override void OnRemove()
        {
            for (int i = 0; i < m_TempModules.Count; i++)
            {
                var item = m_TempModules[i];
                item.OnRemove();
            }
            base.OnRemove();
        }

        public override void OnFixedUpdate(float delta)
        {
            base.OnFixedUpdate(delta);
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < m_TempModules.Count; i++)
            {
                var item = m_TempModules[i];
                item.OnFixedUpdate(delta);
            }
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < m_TempModules.Count; i++)
            {
                var item = m_TempModules[i];
                item.OnUpdate(delta);
            }
        }

        public override void OnLateUpdate(float delta)
        {
            base.OnLateUpdate(delta);
            if (!IsUpdate)
            {
                return;
            }
            for (int i = 0; i < m_TempModules.Count; i++)
            {
                var item = m_TempModules[i];
                item.OnLateUpdate(delta);
            }
        }

        /// <summary>
        /// 模块缓存容器
        /// </summary>
        private Dictionary<Type, AbsModule> modules = new Dictionary<Type, AbsModule>();

        private List<AbsModule> m_TempModules = new List<AbsModule>();

        protected abstract void InitModules();

        public AbsModule RegisterModule<T>(ITuple tuple = null) where T : AbsModule
        {
            Type moduleType = typeof(T);
            AbsModule module;
            if (modules.ContainsKey(moduleType))
            {
                throw new Exception($"{GetType().FullName} has register {moduleType.FullName}!");
            }
            else
            {
                module = Activator.CreateInstance(moduleType) as AbsModule;
                m_TempModules.Add(module);
                modules.Add(moduleType, module);
                module.OnInit(tuple);
            }
            return module;
        }

        public void UnRegisterModule<T>() where T : AbsModule
        {
            Type moduleType = typeof(T);
            if (modules.TryGetValue(moduleType, out AbsModule module))
            {
                if (module != null)
                {
                    m_TempModules.Remove(module);
                    modules.Remove(moduleType);
                    module.OnRemove();
                }
            }
        }

        public void UnRegisterModule(AbsModule module)
        {
            if (module != null)
            {
                Type moduleType = module.GetType();
                if (modules.ContainsValue(module))
                {
                    m_TempModules.Remove(module);
                    modules.Remove(moduleType);
                    module.OnRemove();
                }
            }
        }

        public T RetriveModule<T>() where T : AbsModule
        {
            Type moduleType = typeof(T);
            modules.TryGetValue(moduleType, out AbsModule ret);
            if (ret == null)
            {
                throw new Exception($"{GetType().FullName} not register {moduleType.FullName}!");
            }
            return ret as T;
        }

        public T RetriveBaseModule<T>() where T : AbsModule
        {
            T ret = default;
            foreach (var item in modules.Values)
            {
                if (item is T)
                {
                    ret = item as T;
                    break;
                }
            }
            return ret;
        }
    }
}