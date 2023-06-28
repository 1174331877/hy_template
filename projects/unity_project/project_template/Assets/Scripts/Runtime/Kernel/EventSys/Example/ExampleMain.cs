using LT_Event;
using UnityEngine;
namespace EventSysExample
{
    public class ExampleMain : MonoBehaviour
    {
        public static EventSys eventSys = new EventSys();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnGUI()
        {
            if (GUILayout.Button("发送消息", GUILayout.Width(100), GUILayout.Height(80)))
            {
                eventSys.Notify(TestMsg.Test, System.Tuple.Create("自定义数据"));
            }
            if (GUILayout.Button("销毁订阅事件的对象,发送消息", GUILayout.Width(200), GUILayout.Height(80)))
            {
                var trans = GameObject.Find("EventSysExample");
                if (trans != null)
                {
                    GameObject.DestroyImmediate(trans.gameObject);
                }
                eventSys.Notify(TestMsg.Test, System.Tuple.Create("自定义数据"));
            }
        }
    }
}