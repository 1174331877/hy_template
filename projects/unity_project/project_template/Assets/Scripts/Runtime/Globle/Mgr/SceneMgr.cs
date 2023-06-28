using LT_Kernel;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LT_GL
{
    public class SceneMgr : AbsMgr
    {
        //游戏是单场景模式所以游戏要在场景切换的时候卸载上一个场景的AbsModule
        //当前场景模块
        private AbsModule curSceneModule;

        public AbsModule CurrentSceneModule
        { get { return curSceneModule; } }

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit();
            LTGL.Ins.SceneEvent.RegisterHandler(ESceneNotification.ResetLevel, OnResetLevel, TokenSource.Token);
            LTGL.Ins.SceneEvent.RegisterHandler(ESceneNotification.Level1, OnLevel1, TokenSource.Token);
            LTGL.Ins.SceneEvent.RegisterHandler(ESceneNotification.Level2, OnLevel2, TokenSource.Token);
            LTGL.Ins.SceneEvent.RegisterHandler(ESceneNotification.Level3, OnLevel3, TokenSource.Token);
        }

        private void OnLevel3(ITuple obj)
        {
            LTLog.L("OnLevel3");
            if (SceneManager.GetActiveScene().name == nameof(ESceneNotification.Level3))
            {
                return;
            }
            //LoadScene<Level3.Level3Module>("Level3/Scenes/Level3.unity", obj);
        }

        private void OnLevel2(ITuple obj)
        {
            LTLog.L("OnLevel2");
            if (SceneManager.GetActiveScene().name == nameof(ESceneNotification.Level2))
            {
                return;
            }
            //LoadScene<Level2.Level2Module>("Level2/Scenes/Level2.unity", obj);
        }

        private void OnLevel1(ITuple obj)
        {
            LTLog.L("OnLevel1");
            if (SceneManager.GetActiveScene().name == nameof(ESceneNotification.Level1))
            {
                return;
            }
            //LoadScene<Level1.Level1Module>("Level1/Scenes/Level1.unity", obj);
        }

        private void OnResetLevel(ITuple obj)
        {
            LTLog.L("加载重置关卡场景");
            string curScene = SceneManager.GetActiveScene().name;
            Enum.TryParse<ESceneNotification>(curScene, out var sceneNotify);
            LoadTransitionScene(Tuple.Create(sceneNotify));
        }

        private void LoadTransitionScene(ITuple obj)
        {
            //LoadScene<ResetLevel.ResetLevelModule>("ResetLevel/Scenes/ResetLevel.unity", obj);
        }

        /// <summary>
        /// 记录逻辑场景名字
        /// </summary>
        private void RecordLogicSceneName(string scenePath)
        {
            var strs = scenePath.Split('/');
            ActiveSceneName = strs[strs.Length - 1].Replace(".unity", string.Empty);
        }

        #region 场景加载方法

        private void LoadScene<T>(string sceneAbPath, ITuple param = null,
            LoadSceneMode sceneMode = LoadSceneMode.Single, bool activateOnLoad = false) where T : AbsModule
        {
            RecordLogicSceneName(sceneAbPath);
            if (curSceneModule != null)
            {
                curSceneModule.IsUpdate = false;
            }

            //开启场景过度渐变
            LTGL.Ins.GameEvent.Notify(ESceneMsg.StartLoadScene);

            LTGL.Ins.ResMgr.LoadSceneAsync(sceneAbPath, UpdateSceneLoadingPanelProcess, (scene) =>
            {
                if (!activateOnLoad)
                {
                    AsyncOperation asyncOperation = scene.ActivateAsync();
                    asyncOperation.completed += (opt) => { OnLoadFinish(); };
                }
                else
                {
                    OnLoadFinish();
                }
                void OnLoadFinish()
                {
                    // 场景加载完成
                    if (curSceneModule != null)
                    {
                        LTGL.Ins.ModuleMgr.UnRegisterModule(curSceneModule);
                    }
                    curSceneModule = LTGL.Ins.ModuleMgr.RegisterModule<T>(param);
                }
            }, sceneMode, activateOnLoad);
        }

        private void UpdateSceneLoadingPanelProcess(float process)
        {
            LTGL.Ins.LogicEvent.Notify(ECommon.ShowSceneRatio, Tuple.Create(process));
        }

        /// <summary>
        /// 场景是否处于激活状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsActive(string name)
        {
            return SceneManager.GetActiveScene().name == name;
        }

        public string ActiveSceneName { get; private set; }

        #endregion 场景加载方法
    }
}