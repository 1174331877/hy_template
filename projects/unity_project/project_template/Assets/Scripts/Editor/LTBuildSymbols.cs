using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class LTBuildSymbols
{
    static List<string> m_Symbols = new List<string>() { LTSymbols.LTDEBUG, LTSymbols.LTRELEASE };

    [MenuItem("Tools/发布类型/DEBUG")]
    static void SetDebugDefineSymbol()
    {
        var buildTargetGroup = GetBuildTargetGroup();
        var symbolsStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        string[] symbols = GetActiveSymbols();
        List<string> s = symbols.Where((d) => !m_Symbols.Contains(d)).ToList();
        s.Add(LTSymbols.LTDEBUG);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, s.ToArray());
    }

    [MenuItem("Tools/发布类型/DEBUG", true)]
    static bool SetDebugDefineSymbolValidate()
    {
        return !GetActiveSymbols().Contains(LTSymbols.LTDEBUG);
    }

    [MenuItem("Tools/发布类型/RELEASE")]
    static void SetReleaseDefineSymbol()
    {
        var buildTargetGroup = GetBuildTargetGroup();
        var symbolsStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        string[] symbols = GetActiveSymbols();
        List<string> s = symbols.Where((d) => !m_Symbols.Contains(d)).ToList();
        s.Add(LTSymbols.LTRELEASE);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, s.ToArray());
    }

    [MenuItem("Tools/发布类型/RELEASE", true)]
    static bool SetReleaseDefineSymbolValidate()
    {
        return !GetActiveSymbols().Contains(LTSymbols.LTRELEASE);
    }

    static string[] GetActiveSymbols()
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
        var symbolsStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        return symbolsStr.Split(';');
    }

    static BuildTargetGroup GetBuildTargetGroup()
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
        return BuildPipeline.GetBuildTargetGroup(buildTarget);
    }
}
