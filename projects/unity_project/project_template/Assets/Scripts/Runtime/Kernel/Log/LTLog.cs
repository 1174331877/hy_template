using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class LTSymbols
{
    public const string LTDEBUG = "LTDEBUG";
    public const string LTRELEASE = "LTRELEASE";
}

public static class LTLog
{
    [Conditional(LTSymbols.LTDEBUG)]
    public static void L(object msg)
    {
        Debug.Log(msg);
    }

    [Conditional(LTSymbols.LTDEBUG)]
    public static void W(object msg)
    {
        Debug.LogWarning(msg);
    }

    [Conditional(LTSymbols.LTDEBUG)]
    public static void E(object msg)
    {
        Debug.LogError(msg);
    }
}
