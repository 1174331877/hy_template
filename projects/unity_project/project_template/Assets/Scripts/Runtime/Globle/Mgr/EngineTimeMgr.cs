using LT_Kernel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_GL
{
    /// <summary>
    /// 引擎时间控制器
    /// </summary>
    public class EngineTimeMgr : AbsMgr
    {
        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
            //设置物理引擎的更新步长为80帧/秒:原因是在目标设备上的运行逻辑帧(Update)为60-72帧之间,为了避免在逻辑帧中出现空的物理更新帧,需要设置物理更新帧率比逻辑帧高
            Time.fixedDeltaTime = 1f / 80f;
            /*设置Time.deltaTime的最大更新步长,较小的的数值可以防止多个过长的逻辑更新帧出现,较高的该值出现过多逻辑更新帧出现的原因是:
            如果该值比较大,那么一个逻辑帧执行的最大固定帧数为 maxFixedUpdateCount = Time.maximumDeltatime/Time.fixedDeltaTime,意味着
            会有更多的物理更新次数发生,也就更加导致CPU的负载,进而导致下次的Time.deltaTime仍然大于Time.maximumDeltatime的值,导致恶性循环.
            所以较小的Time.maximumDeltatime可以减缓情况的恶化程度
            */
            //设置一个最大的Time.deltaTime步长内,最多执行两个物理更新帧
            Time.maximumDeltaTime = Time.fixedDeltaTime * 2f;
        }
    }
}