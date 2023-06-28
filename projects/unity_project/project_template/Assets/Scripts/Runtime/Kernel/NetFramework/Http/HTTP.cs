using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace NET
{
    public static class HTTP
    {
        #region HLAPI

        /// <summary>
        /// GET操作
        /// </summary>
        /// <param name="url">通用资源定位路径</param>
        /// <param name="cb">操作完成回调</param>
        /// <param name="processCb">进度回调</param>
        /// <returns></returns>
        public static IEnumerator GET(string url, Action<UnityWebRequest> cb, Action<float> processCb = null)
        {
            var uwr = UnityWebRequest.Get(url);
            var asyncOpt = uwr.SendWebRequest();
            while (!asyncOpt.isDone)
            {
                processCb?.Invoke(asyncOpt.progress);
                yield return null;
            }
            processCb?.Invoke(asyncOpt.progress);
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.result);
            }
            else
            {
                Debug.Log("HTTP get complete!");
                cb.Invoke(uwr);
            }
        }

        public static IEnumerator PUT(string url, string strData, Action<UnityWebRequest> cb, Action<float> processCb = null)
        {
            var uwr = UnityWebRequest.Put(url, strData);
            var asyncOpt = uwr.SendWebRequest();
            while (!asyncOpt.isDone)
            {
                processCb?.Invoke(asyncOpt.progress);
                yield return null;
            }
            processCb?.Invoke(asyncOpt.progress);
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.result);
            }
            else
            {
                Debug.Log("HTTP put string complete!");
                cb.Invoke(uwr);
            }
        }

        public static IEnumerator PUT(string url, byte[] bytesData, Action<UnityWebRequest> cb, Action<float> processCb = null)
        {
            var uwr = UnityWebRequest.Put(url, bytesData);
            var asyncOpt = uwr.SendWebRequest();
            while (!asyncOpt.isDone)
            {
                processCb?.Invoke(asyncOpt.progress);
                yield return null;
            }
            processCb?.Invoke(asyncOpt.progress);
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.result);
            }
            else
            {
                Debug.Log("HTTP put bytes complete!");
                cb.Invoke(uwr);
            }
        }

        #endregion HLAPI

        #region LLAPI

        public static UnityWebRequest GetUWR(string url, string verb)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            uwr.method = verb;
            uwr.useHttpContinue = false;
            uwr.redirectLimit = 0;
            uwr.timeout = 60;
            return uwr;
        }

        public static UnityWebRequest GetUWRUploader(string url, string verb, UploadHandler uploadHandler)
        {
            var uwr = GetUWR(url, verb);
            uwr.uploadHandler = uploadHandler;
            return uwr;
        }

        public static UnityWebRequest GetUWRDownloader(string url, string verb, DownloadHandler downloadHandler)
        {
            var uwr = GetUWR(url, verb);
            uwr.downloadHandler = downloadHandler;
            return uwr;
        }

        #endregion LLAPI
    }
}