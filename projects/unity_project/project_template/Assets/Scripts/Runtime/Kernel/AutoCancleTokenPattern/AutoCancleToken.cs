using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LT_Kernel
{
    /// <summary>
    /// 提供自动取消订阅机制
    /// </summary>
    public class AutoCancleToken : ILifecycle, IDisposable
    {
        protected CancellationTokenSource TokenSource = new CancellationTokenSource();

        public void Dispose()
        {
            Dispose(true);
        }

        private bool m_Disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed) return;
            if (disposing)
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
                TokenSource = null;
            }
            m_Disposed = true;
        }

        public virtual void OnInit(ITuple tuple = null)
        {
        }

        public virtual void OnRemove()
        {
            Dispose();
        }
    }
}