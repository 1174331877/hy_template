using LT_Kernel;
using LT_UI;
using System.Runtime.CompilerServices;

namespace LT_GL
{
    public sealed class GLModule : AbsModule
    {
        protected override void InitMgrs()
        {
            LTGL.Ins.TokenSource = TokenSource;
            RegisterMgr<EngineTimeMgr>();
            LTGL.Ins.GameSettingMgr = RegisterMgr<GameSettingMgr>();
            LTGL.Ins.CoroutineMgr = RegisterMgr<CoroutineMgr>();
            LTGL.Ins.TimerMgr = RegisterMgr<TimerMgr>();
            LTGL.Ins.ResMgr = RegisterMgr<ResMgr>();
            LTGL.Ins.AudioMgr = RegisterMgr<AudioMgr>();
            LTGL.Ins.SceneMgr = RegisterMgr<SceneMgr>();
            LTGL.Ins.ModelMgr = RegisterMgr<ModelMgr>();
            LTGL.Ins.Tables = RegisterMgr<CfgMgr>().Tables;
            LTGL.Ins.UIRootMgr = RegisterMgr<UIRootMgr>();
            LTGL.Ins.WindowMgr = RegisterMgr<WindowMgr>();
        }

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit();
            //这里判断进入哪一个场景
            SelectEnterGameScene();
        }

        /// <summary>
        /// 选择进入游戏场景
        /// </summary>
        private void SelectEnterGameScene()
        {
            //LTGL.Ins.WindowMgr.ShowWindow<LaunchWindow>();
        }
    }
}