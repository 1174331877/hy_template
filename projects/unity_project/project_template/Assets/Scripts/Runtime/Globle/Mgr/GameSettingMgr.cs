using LT_Kernel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_GL
{
    public class GameSettingMgr : AbsMgr
    {
        /// <summary>
        /// 游戏帧率
        /// </summary>
        public int GameFrameRate { get; set; } = 72;

        /// <summary>
        /// 画布标准分辨率
        /// </summary>
        public Vector2 ScreenRatio { get; } = new Vector2(1080f, 1920f);

        public override void OnInit(ITuple tuple = null)
        {
            IsUpdate = true;
            base.OnInit();
            //Application.targetFrameRate = GameFrameRate;
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Application.Quit();
            //}
        }
    }
}