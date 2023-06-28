using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LT_Event
{
    public sealed class EventSys : IEvent
    {
        /// <summary>
        /// 回调缓存容器
        /// </summary>
        private Dictionary<int, Action<ITuple>> handlers = new Dictionary<int, Action<ITuple>>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        public void RegisterHandler(Enum eventName, Action<ITuple> action, CancellationToken cancellationToken)
        {
            int code = (int)(eventName as ValueType);
            if (handlers.TryGetValue(code, out var handler))
            {
                if (!handler.HasContainDelegate(action))
                {
                    handlers[code] = handler.CombineDelegate(action);
                }
                cancellationToken.Register(() =>
                {
                    UnRegisterHandler(eventName, action);
                });
            }
            else
            {
                handlers.Add(code, action);
                cancellationToken.Register(() =>
                {
                    UnRegisterHandler(eventName, action);
                });
            }
        }

        /// <summary>
        /// 注销单个事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        public void UnRegisterHandler(Enum eventName, Action<ITuple> action)
        {
            if (action != null)
            {
                int code = (int)(eventName as ValueType);
                if (handlers.TryGetValue(code, out var handler))
                {
                    if (handler != null)
                    {
                        if (handler.HasContainDelegate(action))
                        {
                            var dirtyAction = handler.RemoveDelegate(action);
                            if (dirtyAction == null)
                            {
                                handlers.Remove(code);
                            }
                            else
                            {
                                handlers[code] = dirtyAction;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="eventName"></param>
        public void UnRegisterHandler(Enum eventName)
        {
            int code = (int)(eventName as ValueType);
            if (handlers.ContainsKey(code))
            {
                handlers.Remove(code);
            }
        }

        public void UnRegisterAllHandler()
        {
            handlers.Clear();
        }

        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="tuple"></param>
        public void Notify(Enum eventName, ITuple tuple = null)
        {
            int code = (int)(eventName as ValueType);
            if (handlers.TryGetValue(code, out var handler))
            {
                if (handler != null)
                {
                    handler.Invoke(tuple);
                }
            }
        }
    }
}
