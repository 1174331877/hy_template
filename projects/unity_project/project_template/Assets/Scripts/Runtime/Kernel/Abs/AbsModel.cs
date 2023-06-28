using System.Runtime.CompilerServices;

namespace LT_Kernel
{
    /// <summary>
    /// 数据操作模型
    /// </summary>
    public abstract class AbsModel : AutoCancleToken, ILifecycle, IClear
    {
        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
        }

        public virtual void OnInitFinish()
        {
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        /// <summary>
        /// 退出到登录界面时要清理数据缓存
        /// </summary>
        public abstract void OnClear();
    }
}