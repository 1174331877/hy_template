using System;
using System.Collections;
using System.Threading;
using LT_Kernel;
using UnityEngine;

namespace LT_GL
{
    public sealed class CoroutineMgr : AbsMgr
    {
        public Coroutine StartCoroutine(IEnumerator enumerator, CancellationToken cancellationToken)
        {
            Coroutine ret = null;
            if (enumerator != null && cancellationToken != null)
            {
                ret = LTGL.Ins.MainBehaviour.StartCoroutine(enumerator);
                cancellationToken.Register(() =>
                {
                    StopCoroutine(ret);
                });
            }
            return ret;
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null && LTGL.Ins.MainBehaviour != null)
            {
                LTGL.Ins.MainBehaviour.StopCoroutine(coroutine);
            }
        }

        /// <summary>
        /// 等待一帧执行回调
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        public void WaitNextFrameCallBack(Action action, CancellationToken cancellationToken)
        {
            StartCoroutine(_WaitNextFrameCallBack(action), cancellationToken);
        }

        private IEnumerator _WaitNextFrameCallBack(Action action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }
    }
}