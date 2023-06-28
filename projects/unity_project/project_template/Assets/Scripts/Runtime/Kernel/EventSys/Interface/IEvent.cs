using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LT_Event
{
    public interface IEvent
    {
        void RegisterHandler(Enum eventName, Action<ITuple> action, CancellationToken cancellationToken);
        void UnRegisterHandler(Enum eventName, Action<ITuple> action);
        void UnRegisterHandler(Enum eventName);
        void UnRegisterAllHandler();
        void Notify(Enum eventName, ITuple tuple = null);
    }
}
