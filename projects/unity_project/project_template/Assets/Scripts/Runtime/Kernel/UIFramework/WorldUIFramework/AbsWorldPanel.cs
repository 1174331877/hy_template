using UnityEngine;

namespace LT_UI
{
    public abstract class AbsWorldPanel : AbsPanel
    {
        //这里的传入的ECanvasType参数是为了随便给一个参数给父类,世界UI其实并不会用到这个参数
        public AbsWorldPanel(Transform transform) : base(transform, ECanvasType.BaseCanvas)
        {
        }
    }
}
