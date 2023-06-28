
namespace LT_Kernel
{
    /// <summary>
    /// 单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class , new() 
    {
        public static object lockObj = new object();

        private static T ins = null;

        /// <summary>
        /// 支持线程安全的类型单实例
        /// </summary>
        public static T Ins
        {
            get
            {
                if (ins == null)
                {
                    lock (lockObj)
                    {
                        if (ins == null)
                        {
                            ins = new T();
                        }
                    }
                }
                return ins;                
            }
        }
            
    }
}
