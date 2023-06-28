using LT_UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldPanelMgr : PanelMgr
{
    /// <summary>
    /// 显示世界UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pt"></param>
    /// <param name="tuple"></param>
    /// <param name="cb"></param>
    public void ShowWorldUI<T>(Transform pt, ITuple tuple = null, Action cb = null) where T : AbsPanel
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
            object[] ats = panelType.GetCustomAttributes(typeof(WorldPanelAttribute), false);
            WorldPanelAttribute attribute = ats[0] as WorldPanelAttribute;
            if (attribute.IsResources)
            {
                var go = LTGL.Ins.ResMgr.LoadFromResources<GameObject>(attribute.PrefabPath);
                LoadWorldPanel(go, pt, panelType, tuple, cb);
            }
            else
            {
                var r = LTGL.Ins.ResMgr.LoadAsset<GameObject>(attribute.PrefabPath, TokenSource.Token);
                LoadWorldPanel(r, pt, panelType, tuple, cb);
            }
        }
    }
}