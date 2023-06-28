using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class LTTextWorld : TextMeshPro
{
    /// <summary>
    /// 文本Id
    /// </summary>
    public int textId = 10000;

    private CancellationTokenSource TokenSource = new CancellationTokenSource();

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

    protected override void OnDestroy()
    {
        TokenSource.Cancel();
        TokenSource.Dispose();
    }
}