using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LT_UI
{
    public class PanelMgr : AbsMgr
    {
        /// <summary>
        /// 页面缓存容器
        /// </summary>
        protected Dictionary<Type, AbsPanel> panels = new Dictionary<Type, AbsPanel>();

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit();
            IsUpdate = true;
        }

        public override void OnUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }
            base.OnUpdate(delta);
            var valueEnumerator = panels.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var temp = valueEnumerator.Current;
                if (temp != null && temp.IsActive && temp.IsUpdate)
                {
                    temp.OnUpdate(delta);
                }
            }
        }

        public override void OnRemove()
        {
            var valueEnumerator = panels.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var temp = valueEnumerator.Current;
                if (temp != null)
                {
                    temp.OnRemove();
                }
            }
            panels.Clear();
            base.OnRemove();
        }

        public void ShowPanel<T>(ITuple tuple = null, Action cb = null) where T : AbsPanel
        {
            Type panelType = typeof(T);
            if (panels.TryGetValue(panelType, out AbsPanel panel))
            {
                if (panel != null && !panel.IsActive)
                {
                    panel.OnShowBefore();
                    panel.OnShow(tuple);
                    panel.IsActive = true;
                }
            }
            else
            {
                object[] ats = panelType.GetCustomAttributes(typeof(PanelAttribute), false);
                PanelAttribute attribute = ats[0] as PanelAttribute;
                if (attribute.IsResources)
                {
                    LTGL.Ins.ResMgr.LoadAsyncFromResources<GameObject>(attribute.PrefabPath, (go) =>
                     {
                         LoadPanel(go, attribute, panelType, tuple, cb);
                     });
                }
                else
                {
                    LTGL.Ins.ResMgr.LoadAssetAsync<GameObject>(attribute.PrefabPath, (r) => { LoadPanel(r, attribute, panelType, tuple, cb); }, TokenSource.Token);
                }
            }
        }

        protected void LoadPanel(GameObject prefab, PanelAttribute attribute, Type panelType, ITuple tuple = null, Action cb = null)
        {
            Transform prefabTrans = GameObject.Instantiate(prefab).transform;
            prefabTrans.SetParent(LTGL.Ins.UIRootMgr.GetCanvasByType(attribute.CanvasType));
            RectTransform rectTransform = prefabTrans as RectTransform;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            AbsPanel absPanel = Activator.CreateInstance(panelType, prefabTrans, attribute.CanvasType) as AbsPanel;
            panels.Add(panelType, absPanel);
            absPanel.OnInit(tuple);
            absPanel.OnShowBefore(tuple);
            absPanel.OnShow(tuple);
            absPanel.IsActive = true;
            cb?.Invoke();
        }

        /// <summary>
        /// 加载世界空间UI
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pt"></param>
        /// <param name="panelType"></param>
        /// <param name="tuple"></param>
        /// <param name="cb"></param>
        protected void LoadWorldPanel(GameObject prefab, Transform pt, Type panelType, ITuple tuple = null, Action cb = null)
        {
            Transform prefabTrans = GameObject.Instantiate(prefab).transform;
            prefabTrans.SetParent(pt);
            if (prefabTrans is RectTransform)
            {
                RectTransform rectTransform = prefabTrans as RectTransform;
                rectTransform.pivot = Vector2.one * 0.5f;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localScale = Vector3.one;
            }
            else
            {
                prefabTrans.localPosition = Vector3.zero;
                prefabTrans.localRotation = Quaternion.identity;
                prefabTrans.localScale = Vector3.one;
            }
            AbsPanel absPanel = Activator.CreateInstance(panelType, new object[] { prefabTrans }) as AbsPanel;
            panels.Add(panelType, absPanel);
            absPanel.OnInit(tuple);
            absPanel.OnShowBefore(tuple);
            absPanel.OnShow(tuple);
            absPanel.IsActive = true;
            cb?.Invoke();
        }

        public void HidePanel<T>(ITuple tuple = null, bool isCache = false) where T : AbsPanel
        {
            Type panelType = typeof(T);
            if (panels.TryGetValue(panelType, out AbsPanel panel))
            {
                if (panel != null)
                {
                    panel.OnHideBefore(tuple);
                    panel.OnHide(tuple, isCache);
                    if (!isCache)
                    {
                        panels.Remove(panelType);
                        panel.OnRemove();
                    }
                    panel.IsActive = false;
                }
            }
        }

        public void HideAllPanel(ITuple tuple = null, bool isCache = false)
        {
            var valueEnumerator = panels.Values.GetEnumerator();
            while (valueEnumerator.MoveNext())
            {
                var temp = valueEnumerator.Current;
                if (temp != null)
                {
                    temp.OnHideBefore(tuple);
                    temp.OnHide(tuple, isCache);
                    temp.IsActive = false;
                }
            }
        }
    }
}