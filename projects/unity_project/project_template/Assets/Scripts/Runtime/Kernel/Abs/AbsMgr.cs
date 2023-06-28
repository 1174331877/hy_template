using System.Runtime.CompilerServices;

namespace LT_Kernel
{
    /// <summary>
    /// 管理器的抽象实现
    /// </summary>
    public abstract class AbsMgr : AutoCancleToken, ILifecycle, IFixedUpdate, IUpdate, ILateUpdate, IIsUpdate
    {
        #region 基础生命周期

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        #endregion 基础生命周期

        #region 生命周期更新行为

        public bool IsUpdate { get; set; } = false;

        public virtual void OnFixedUpdate(float delta)
        {
        }

        public virtual void OnUpdate(float delta)
        {
        }

        public virtual void OnLateUpdate(float delta)
        {
        }

        #endregion 生命周期更新行为
    }
}