using System;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class LTText : TextMeshProUGUI
{
    /// <summary>
    /// 文本Id
    /// </summary>
    public int textId = 10000;

    private CancellationTokenSource TokenSource = new CancellationTokenSource();

    //切换多语言文本后对文本内容修改的回调
    public Action<string> SetTextCb;

    protected override void Awake()
    {
        base.Awake();
        LTGL.Ins.LogicEvent.RegisterHandler(ELT_MultiLanguage.OnChangeLanguage, OnChangeLanguage, TokenSource.Token);
        SetTextId(textId);
    }

    private void OnChangeLanguage(ITuple obj)
    {
        if (Application.isPlaying && LTGL.Ins.FrameIsInit)
        {
            //text = LTGL.Ins.GameConfigMgr.GetLanguageContent(textId);
            SetTextCb?.Invoke(text);
        }
    }

    /// <summary>
    /// 通过配置Id设置文本内容
    /// </summary>
    /// <param name="id"></param>
    public void SetTextId(int id)
    {
        textId = id;
        OnChangeLanguage(null);
    }

    /// <summary>
    /// 通过对文本处理的后期绑定重新设置文本内容
    /// </summary>
    public void RefrshTextBySetTextCb()
    {
        OnChangeLanguage(null);
    }

    protected override void OnDestroy()
    {
        TokenSource.Cancel();
        TokenSource.Dispose();
    }
}