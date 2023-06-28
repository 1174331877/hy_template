namespace LT_EntityComponent
{
    /// <summary>
    /// 对象激活状态接口
    /// </summary>
    public interface IActive
    {
        /// <summary>
        /// 对象激活状态
        /// </summary>
        bool IsActive { get; set; }
    }

}