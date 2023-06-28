using LT_Kernel;
using System;
using System.Collections;
using System.Threading;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LT_GL
{
    public sealed class ResMgr : AbsMgr
    {
        /// <summary>
        /// 加载Resources下资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <returns></returns>
        public T LoadFromResources<T>(string resName) where T : UnityEngine.Object
        {
            return Resources.Load<T>(resName);
        }

        /// <summary>
        /// 异步加载Resources下资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <param name="cb"></param>
        public void LoadAsyncFromResources<T>(string resName, Action<T> cb = null) where T : UnityEngine.Object
        {
            LTGL.Ins.CoroutineMgr.StartCoroutine(LoadAsync_(resName, cb), TokenSource.Token);
        }

        private IEnumerator LoadAsync_<T>(string resName, Action<T> cb = null) where T : UnityEngine.Object
        {
            var resHandler = Resources.LoadAsync<T>(resName);
            yield return resHandler;
            cb?.Invoke(resHandler.asset as T);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetKey"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string assetKey, CancellationToken cancellationToken) where T : UnityEngine.Object
        {
            //Basic use case of forcing a synchronous load of a GameObject
#if UNITY_EDITOR
            assetKey = $"Assets/Res/BundleRes/{assetKey}";
            return AssetDatabase.LoadAssetAtPath<T>(assetKey);
#else
            var op = Addressables.LoadAssetAsync<T>(assetKey);
            op.WaitForCompletion();
            cancellationToken.Register(() =>
            {
                Relase(op);
            });
            return op.Result;
#endif
        }

        public void LoadAssetAsync<T>(string assetKey, Action<T> cb, CancellationToken cancellationToken) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            assetKey = $"Assets/Res/BundleRes/{assetKey}";
            var r = AssetDatabase.LoadAssetAtPath<T>(assetKey);
            cb.Invoke(r);
#else
            var handle = Addressables.LoadAssetAsync<T>(assetKey);
            handle.Completed += (h) =>
            {
                cb.Invoke(h.Result);
            };
            cancellationToken.Register(() =>
            {
                Relase(handle);
            });
#endif
        }

        public void Relase<T>(AsyncOperationHandle<T> handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="key"></param>
        /// <param name="processCb"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        public void LoadSceneAsync(string key, Action<float> processCb = null, Action<SceneInstance> cb = null,
            LoadSceneMode sceneMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            string name = SceneManager.GetActiveScene().name;
            string[] strs = key.Split('/');
            if (strs[strs.Length - 1].Split('.')[0] != name)
            {
                LTGL.Ins.CoroutineMgr.StartCoroutine(LoadSceneAsync_(key, processCb, cb, sceneMode, activateOnLoad),
                    TokenSource.Token);
            }
        }

        private IEnumerator LoadSceneAsync_(string key, Action<float> processCb = null, Action<SceneInstance> cb = null,
    LoadSceneMode sceneMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            var handle = Addressables.LoadSceneAsync(key, sceneMode, activateOnLoad);

            //场景加载进度
            float loadFactor = activateOnLoad ? 1f : 0.8f;

            while (!handle.IsDone)
            {
                processCb?.Invoke(handle.PercentComplete * loadFactor);
                yield return null;
            }
            processCb?.Invoke(handle.PercentComplete * loadFactor);
            if (!activateOnLoad)
            {
                float reduceTime = 2f;
                float timeTick = 0f;
                while (timeTick < reduceTime)
                {
                    timeTick += Time.deltaTime;
                    processCb?.Invoke(loadFactor + (timeTick / reduceTime) * (1 - loadFactor));
                    yield return null;
                }
                processCb?.Invoke(1f);
                cb?.Invoke(handle.Result);
            }
            else
            {
                cb?.Invoke(handle.Result);
            }
        }
    }
}