using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EventSysExample
{
    public enum TestMsg
    {
        Test
    }
    public class EventSysExample : MonoBehaviour
    {
        /// <summary>
        /// 消息自动取消令牌
        /// </summary>
        CancellationTokenSource cancellationToken = new CancellationTokenSource();


        // Start is called before the first frame update
        void Start()
        {
            ExampleMain.eventSys.RegisterHandler(TestMsg.Test, OnTest, cancellationToken.Token);
        }

        private void OnTest(ITuple obj)
        {
            Tuple<string> tuple = obj as Tuple<string>;
            //自定义逻辑代码
            Debug.Log(this.GetType() + "收到的参数:" + tuple.Item1);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            //自动注销消息
            cancellationToken.Cancel();
        }
    }
}