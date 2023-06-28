/// <summary>
/// 工程核心接口
/// </summary>
namespace LT_Kernel
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 脚本生命周期接口
    /// </summary>
    public interface ILifecycle
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit(ITuple tuple = null);

        /// <summary>
        /// 移除清理
        /// </summary>
        void OnRemove();
    }

    /// <summary>
    /// 生命周期更新控制开关
    /// </summary>
    public interface IIsUpdate
    {
        bool IsUpdate { get; set; }
    }

    /// <summary>
    /// 对应Monobehaviour:FixedUpadte方法
    /// </summary>
    public interface IFixedUpdate
    {
        void OnFixedUpdate(float delta);
    }

    /// <summary>
    /// 对应Monobehaviour:Update方法
    /// </summary>
    public interface IUpdate
    {
        void OnUpdate(float delta);
    }

    /// <summary>
    /// 对应Monobehaviour:LateUpdate方法
    /// </summary>
    public interface ILateUpdate
    {
        void OnLateUpdate(float delta);
    }
}