using LT_Event;
using LT_Kernel;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_EntityComponent
{
    /// <summary>
    /// 具体子类可以通过该特性指定禁止允许添加多个组件(默认一个实体对象可以添加多个该类型组件实例)
    /// </summary>
    //[DisallowMultipleComponent]
    public abstract class AbsComponent : AutoCancleToken, ILifecycle, IComponentMessage, IActive
    {
        //组件实体消息系统机制
        protected EventSys EventSys = new EventSys();

        /// <summary>
        /// 组件参数
        /// </summary>
        protected ITuple ComponentParam;

        /// <summary>
        /// 挂载该组件的实体对象
        /// </summary>
        public readonly AbsEntity entity;

        /// <summary>
        /// 该组件映射Entity对象所映射的GameObject上的UnityEngine.Component组件(该组件不一定存在映射)
        /// </summary>
        public readonly Component component;

        /// <summary>
        /// 该组件是否处于激活状态
        /// </summary>
        public bool IsActive { get; set; } = true;

        public AbsComponent(AbsEntity entity, Component component = null, ITuple componentParam = null)
        {
            this.entity = entity ?? throw new Exception($"{this.GetType().FullName} constructor param transform is null");
            this.component = component;
            ComponentParam = componentParam;
        }

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
            RegisterMsgs();
        }

        public override void OnRemove()
        {
            EventSys.UnRegisterAllHandler();
            base.OnRemove();
        }

        public void OnMsg(Enum eventMsg, ITuple tuple = null)
        {
            EventSys.Notify(eventMsg, tuple);
        }

        /// <summary>
        /// 注册该组件关心的消息,通过EventSys对象管理消息
        /// </summary>
        protected abstract void RegisterMsgs();
    }
}