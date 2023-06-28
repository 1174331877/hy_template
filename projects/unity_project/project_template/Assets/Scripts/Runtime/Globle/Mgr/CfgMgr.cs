using Bright.Serialization;
using cfg;
using LT_Kernel;
using SimpleJSON;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_GL
{
    public class CfgMgr : AbsMgr
    {
        public Tables Tables { get; private set; }

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
            GetCfgCount();
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
            System.Delegate loader = loaderReturnType == typeof(ByteBuf) ?
                new System.Func<string, ByteBuf>(LoadByteBuf)
                : (System.Delegate)new System.Func<string, JSONNode>(LoadJson);
            Tables = (cfg.Tables)tablesCtor.Invoke(new object[] { loader });
        }

        private JSONNode LoadJson(string file)
        {
            var cfg = LTGL.Ins.ResMgr.LoadAsset<TextAsset>($"cfgs/jsons/{file}.json", TokenSource.Token);
            CfgLoadEnvent();
            return JSON.Parse(cfg.text);
        }

        private ByteBuf LoadByteBuf(string file)
        {
            var cfg = LTGL.Ins.ResMgr.LoadAsset<TextAsset>($"cfgs/bytes/{file}.bytes", TokenSource.Token);
            CfgLoadEnvent();
            return new ByteBuf(cfg.bytes);
        }

        /// <summary>
        /// 配置表总个数
        /// </summary>
        private int m_CfgSumCount;

        /// <summary>
        /// 已经加载的配置表总数
        /// </summary>
        private int m_HasLoadedCfgCount = 0;

        private void GetCfgCount()
        {
            var tables = typeof(Tables).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            m_CfgSumCount = tables.Length;
            //foreach (var item in tables)
            //{
            //    Debug.Log(item.Name);
            //}
        }

        private void CfgLoadEnvent()
        {
            m_HasLoadedCfgCount++;
            //Debug.Log($"总数:{m_CfgSumCount}\t当前个数:{m_HasLoadedCfgCount}");
        }
    }
}