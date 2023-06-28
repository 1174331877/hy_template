public enum EPlayerMsg
{
    /// <summary>
    /// 玩家的行走方式发生改变
    /// </summary>
    OnWalkPatternChange,

    End
}

public enum EShopMsg
{
    OnGenerateWeaponModel = EPlayerMsg.End + 1,
    End
}

public enum EGameMsg
{
    AddAlarmValue = EShopMsg.End + 1,

    /// <summary>
    /// 天气变化
    /// </summary>
    WeatherChange,

    End
}

public enum ESceneMsg
{
    StartLoadScene = EGameMsg.End + 1,
    End
}