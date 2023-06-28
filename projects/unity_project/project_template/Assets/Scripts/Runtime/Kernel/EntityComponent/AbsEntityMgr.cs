using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_EntityComponent
{
    public abstract class AbsEntityMgr : LT_Kernel.AbsMgr, IMessage
    {
        public override void OnInit(ITuple tuple = null)
        {
            IsUpdate = true;
            base.OnInit();
            InitEntities();
        }

        public override void OnRemove()
        {
            RemoveAllEntity();
            base.OnRemove();
        }

        #region 操作实体对象公共方法

        /// <summary>
        /// 实体id生成器
        /// </summary>
        private IDGenerator iDGenerator = new IDGenerator();

        public void SendMsg(Enum eventName, ITuple tuple = null)
        {
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive)
                {
                    entityWrap.Entity.OnMsg(eventName, tuple);
                }
            }
        }

        public void SendMsg<T>(Enum eventName, ITuple tuple = null) where T : AbsComponent
        {
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive)
                {
                    entityWrap.Entity.OnMsg<T>(eventName, tuple);
                }
            }
        }

        public void SendMsg(string entityTag, Enum eventName, ITuple tuple = null)
        {
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive && entityWrap.Entity.entityTag == entityTag)
                {
                    entityWrap.Entity.OnMsg(eventName, tuple);
                }
            }
        }

        public void SendMsg<T>(string entityTag, Enum eventName, ITuple tuple = null) where T : AbsComponent
        {
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive && entityWrap.Entity.entityTag == entityTag)
                {
                    entityWrap.Entity.OnMsg<T>(eventName, tuple);
                }
            }
        }

        /// <summary>
        /// 所有实体缓存 key: 实体id value:实体对象实例
        /// </summary>
        private Dictionary<long, AbsEntityWrap> absEntityWrapsDic = new Dictionary<long, AbsEntityWrap>();

        private List<AbsEntityWrap> absEntityWrapsList = new List<AbsEntityWrap>();

        /// <summary>
        /// 创建一个实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="transform">关联的GameObject的Transform组件</param>
        /// <param name="tuple">参数</param>
        /// <returns></returns>
        public T CreateEntity<T>(Transform transform, string entityTag, ITuple tuple = null) where T : AbsEntity
        {
            Type entityType = typeof(T);
            long id = iDGenerator.Next;
            T ret = Activator.CreateInstance(entityType, transform, entityTag, id, tuple) as T;
            DisplayEntityId(ret);
            var entityWrap = new AbsEntityWrap(id, ret);
            absEntityWrapsList.Add(entityWrap);
            absEntityWrapsDic.Add(id, entityWrap);
            ret.OnInit(tuple);
            ret.OnInitFinish();
            return ret;
        }

        public AbsEntity CreateEntity(Type entityType, Transform transform, string entityTag, ITuple tuple = null)
        {
            AbsEntity ret = default;
            long id = iDGenerator.Next;
            ret = Activator.CreateInstance(entityType, transform, entityTag, id, tuple) as AbsEntity;
            DisplayEntityId(ret);
            var entityWrap = new AbsEntityWrap(id, ret);
            absEntityWrapsList.Add(entityWrap);
            absEntityWrapsDic.Add(id, entityWrap);
            ret.OnInit(tuple);
            ret.OnInitFinish();
            return ret;
        }

        /// <summary>
        /// 在实体对象的根节点上挂载便是实体Id的组件
        /// </summary>
        /// <param name="entity"></param>
        private void DisplayEntityId(AbsEntity entity)
        {
            if (entity.transform != null)
            {
                var displayCom = entity.transform.gameObject.AddComponent<EntityIdDispaly>();
                displayCom.EntityId = entity.id;
            }
        }

        public AbsEntity GetEntity(long entityId)
        {
            absEntityWrapsDic.TryGetValue(entityId, out AbsEntityWrap ret);
            return ret?.Entity;
        }

        public T GetEntity<T>() where T : AbsEntity
        {
            T ret = null;
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                var entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity is T)
                {
                    ret = entityWrap.Entity as T;
                    break;
                }
            }
            return ret;
        }

        public bool HasEntity(long entityId)
        {
            return absEntityWrapsDic.ContainsKey(entityId);
        }

        private List<long> m_RemoveEntityIds = new List<long>();

        /// <summary>
        /// 删除所有该实体类型
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public void RemoveEntities<T>() where T : AbsEntity
        {
            m_RemoveEntityIds.Clear();
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                var entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity is T)
                {
                    m_RemoveEntityIds.Add(entityWrap.Entity.id);
                }
            }
            foreach (var item in m_RemoveEntityIds)
            {
                RemoveEntity(item);
            }
        }

        /// <summary>
        /// 移除单个实体
        /// </summary>
        /// <param name="id"></param>
        public void RemoveEntity(long id)
        {
            if (absEntityWrapsDic.TryGetValue(id, out var absEntityWrap))
            {
                absEntityWrap.Entity.OnRemove();
                absEntityWrapsDic.Remove(id);
                absEntityWrapsList.Remove(absEntityWrap);
            }
        }

        /// <summary>
        /// 移除所有实体
        /// </summary>
        public void RemoveAllEntity()
        {
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                var entity = absEntityWrapsList[i].Entity;
                entity.OnRemove();
            }
            absEntityWrapsList.Clear();
            absEntityWrapsDic.Clear();
        }

        #endregion 操作实体对象公共方法

        #region 初始化实体对象

        public abstract void InitEntities();

        #endregion 初始化实体对象

        #region 驱动实体对象更新

        public override void OnFixedUpdate(float delta)
        {
            base.OnFixedUpdate(delta);
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive)
                {
                    entityWrap.Entity.OnFixedUpdate(delta);
                }
            }
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive)
                {
                    entityWrap.Entity.OnUpdate(delta);
                }
            }
        }

        public override void OnLateUpdate(float delta)
        {
            base.OnLateUpdate(delta);
            for (int i = 0; i < absEntityWrapsList.Count; i++)
            {
                AbsEntityWrap entityWrap = absEntityWrapsList[i];
                if (entityWrap.Entity != null && entityWrap.Entity.IsActive)
                {
                    entityWrap.Entity.OnLateUpdate(delta);
                }
            }
        }

        #endregion 驱动实体对象更新

        private sealed class AbsEntityWrap
        {
            public long Id { get; private set; }
            public AbsEntity Entity { get; private set; }

            public AbsEntityWrap(long id, AbsEntity entity)
            {
                Id = id;
                Entity = entity;
            }
        }
    }
}