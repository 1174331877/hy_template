using LT_Kernel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LT_UI
{
    public abstract class AbsWindow : IIsActive, ILifecycle, IUpdate, IIsUpdate
    {
        /// <summary>
        /// 提供自动注销回调功能
        /// </summary>
        protected CancellationTokenSource TokenSource = new CancellationTokenSource();

        public bool IsUpdate { get; set; } = true;

        public bool IsResident { get; private set; }

        private PanelMgr m_PanelMgr { get; set; }

        public bool IsActive { get; set; } = false;

        public AbsWindow(bool isResident)
        {
            IsResident = isResident;
        }

        public virtual void OnInit(ITuple tuple = null)
        {
            m_PanelMgr = new PanelMgr();
            m_PanelMgr.OnInit();
        }

        public void OnUpdate(float delta)
        {
            if (!IsUpdate || !IsActive)
            {
                return;
            }
            m_PanelMgr.OnUpdate(delta);
        }

        public void OnRemove()
        {
            TokenSource.Cancel();
            TokenSource.Dispose();
            IsResident = false;
            m_PanelMgr.OnRemove();
        }

        public abstract void OnShow(ITuple tuple = null);

        public virtual void OnHide(ITuple tuple = null)
        {
            m_PanelMgr.HideAllPanel(tuple, IsResident);
        }

        public virtual void ShowPanel<T>(ITuple tuple = null) where T : AbsPanel
        {
            m_PanelMgr.ShowPanel<T>(tuple);
        }

        public virtual void HidePanel<T>(ITuple tuple = null) where T : AbsPanel
        {
            m_PanelMgr.HidePanel<T>(tuple);
        }
    }
}