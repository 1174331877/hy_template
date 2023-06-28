/// <summary>
/// 多语言切换消息
/// </summary>
public enum ELT_MultiLanguage
{
    OnChangeLanguage = 1,
    End
}

public enum ECommon
{
    ShowSceneRatio = ELT_MultiLanguage.End + 1,
    ShowCoinCount,
    End
}

public enum EInfoPanel
{
    SetLookRotation = ECommon.End + 1,
    End
}

public enum ETutorial
{
    FinishTutorial = EInfoPanel.End + 1,
    End
}