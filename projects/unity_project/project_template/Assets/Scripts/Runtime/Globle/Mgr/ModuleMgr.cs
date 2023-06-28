using LT_Kernel;

namespace LT_GL
{
    public sealed class ModuleMgr : AbsModuleMgr
    {
        protected override void InitModules()
        {
            RegisterModule<GLModule>();
        }
    }
}