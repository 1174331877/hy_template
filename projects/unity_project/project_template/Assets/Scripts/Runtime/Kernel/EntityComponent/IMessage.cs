using System;
using System.Runtime.CompilerServices;

namespace LT_EntityComponent
{
    /// <summary>
    /// 广播消息接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 给所有实体对象的所有组件广播消息
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="tuple">参数</param>
        void SendMsg(Enum eventName, ITuple tuple = null);

        /// <summary>
        /// 给所有实体对象的指定组件发消息
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="eventName">事件名</param>
        /// <param name="tuple">参数</param>
        void SendMsg<T>(Enum eventName, ITuple tuple = null) where T : AbsComponent;

        /// <summary>
        /// 给指定标签实体对象的所有组件发消息
        /// </summary>
        /// <param name="entityTag">实体对象标签</param>
        /// <param name="enventName">事件名</param>
        /// <param name="tuple">参数</param>
        void SendMsg(string entityTag, Enum eventName, ITuple tuple = null);

        /// <summary>
        /// 给指定标签实体对象的指定组件发消息
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="entityTag"></param>
        /// <param name="enventName"></param>
        /// <param name="tuple"></param>
        void SendMsg<T>(string entityTag, Enum eventName, ITuple tuple = null) where T : AbsComponent;
    }

    /// <summary>
    /// 组件响应消息接口
    /// </summary>
    public interface IComponentMessage
    {
        /// <summary>
        /// 组件相应消息
        /// </summary>
        /// <param name="eventMsg"></param>
        /// <param name="tuple"></param>
        void OnMsg(Enum eventMsg, ITuple tuple = null);
    }

    /// <summary>
    /// 实体消息响应接口
    /// </summary>
    public interface IEntityMessage : IComponentMessage
    {
        /// <summary>
        /// 实体的T类型组件响应消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventMsg"></param>
        /// <param name="tuple"></param>
        void OnMsg<T>(Enum eventMsg, ITuple tuple = null) where T : AbsComponent;
    }
}
