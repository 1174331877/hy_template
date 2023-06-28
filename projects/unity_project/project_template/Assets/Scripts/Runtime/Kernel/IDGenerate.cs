namespace LT_Kernel
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public sealed class IDGenerator
    {
        private long current = -1;
        public long Next
        {
            get
            {
                long temp = current + 1;
                current = temp > long.MaxValue ? -1 : temp;
                return current;
            }
        }
    }
}
