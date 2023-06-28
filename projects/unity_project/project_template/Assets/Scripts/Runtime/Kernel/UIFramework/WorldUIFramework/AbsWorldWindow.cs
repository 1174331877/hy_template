using System.Runtime.CompilerServices;

namespace LT_UI
{
    public abstract class AbsWorldWindow : AbsWindow
    {
        public AbsWorldWindow(bool isResident) : base(isResident)
        {
        }

        private WorldPanelMgr m_PanelMgr;

        public override void OnInit(ITuple tuple = null)
        {
            m_PanelMgr = new WorldPanelMgr();
            m_PanelMgr.OnInit();
        }

        public override void ShowPanel<T>(ITuple tuple = null)
        {
            m_PanelMgr.ShowPanel<T>(tuple);
        }

        public override void HidePanel<T>(ITuple tuple = null)
        {
            m_PanelMgr.HidePanel<T>(tuple);
        }
    }
}