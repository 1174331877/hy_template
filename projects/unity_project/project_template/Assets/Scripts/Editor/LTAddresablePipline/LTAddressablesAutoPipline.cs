using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

#if UNITY_EDITOR

/// <summary>
/// 可寻址系统自定义扩展构建管线
/// </summary>
public class LTAddressablesAutoPipline
{
    [MenuItem("Tools/Addressables工作流工具/自动创建分组")]
    public static void AutoGroup()
    {
        getSettingsObject(settings_asset);
        AutoCreateAAGroups();
    }

    private static void AutoCreateAAGroups()
    {
        //删除旧的分组
        DeleteOldGroup();
        //创建打包资源根目录分组
        CreateChildFolderGroup(BUNDLE_RELATIVE_PATH);
        //自动打开分组面板
        EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Groups");
    }

    private const string BUNDLE_RELATIVE_PATH = "Res/BundleRes";

    /// <summary>
    /// 删除旧的分组
    /// </summary>
    private static void DeleteOldGroup()
    {
        string buildRootPath = Path.Combine(Application.dataPath, BUNDLE_RELATIVE_PATH);
        var dirs = Directory.GetDirectories(buildRootPath);
        //当前打包所有分组的名字
        List<string> newGourpNames = new List<string>();
        //打包根目录组名字
        foreach (var item in dirs)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(item);
            string name = directoryInfo.Name;
            newGourpNames.Add(name);
        }
        //当前aa所有的组
        var groups = settings.groups;
        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            if (group.name != "Built In Data" && group.name != "Default Local Group")
            {
                if (!newGourpNames.Contains(group.name))
                {
                    settings.RemoveGroup(group);
                }
            }
        }
    }

    /// <summary>
    /// 目录下的所有一级子目录创建分组
    /// </summary>
    /// <param name="relativePath"></param>
    private static void CreateChildFolderGroup(string relativePath)
    {
        string folderPath = Path.Combine(Application.dataPath, relativePath);
        var dirs = Directory.GetDirectories(folderPath);
        foreach (var item in dirs)
        {
            string path = item.Replace(Application.dataPath + "\\", "");
            CreateFolderGroup(path);
        }
    }

    /// <summary>
    /// 以该文件夹为目录创建一个分组
    /// </summary>
    /// <param name="folderPath"></param>
    private static void CreateFolderGroup(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, folderPath));
        string name = directoryInfo.Name;
        AddressableAssetGroup group = CreateGroup(name);
        //该文件夹下的所有文件创建条目
        var files = directoryInfo.GetFiles();
        foreach (var item in files)
        {
            if (item.FullName.Contains(".meta"))
            {
                continue;
            }
            string fullPath = "Assets" + Path.GetFullPath(item.FullName).Replace("\\", "/").Replace(Application.dataPath, "");
            CreateGroupEntry(fullPath, group);
        }
        //向组中添加每个资源类型条目
        DirectoryInfo[] childDirs = directoryInfo.GetDirectories();
        foreach (var child in childDirs)
        {
            string fullPath = "Assets" + Path.GetFullPath(child.FullName).Replace("\\", "/").Replace(Application.dataPath, "");
            CreateGroupEntry(fullPath, group);
        }
    }

    /// <summary>
    /// 创建一个分组
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    private static AddressableAssetGroup CreateGroup(string groupName)
    {
        AddressableAssetGroup oldGroup = settings.FindGroup(groupName);
        if (oldGroup != null)
        {
            settings.RemoveGroup(oldGroup);
        }
        AddressableAssetGroup group = settings.CreateGroup(groupName, false, false, false, null, typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
        group.GetSchema<BundledAssetGroupSchema>().BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
        var bundleSchema = group.GetSchema<BundledAssetGroupSchema>();
        bundleSchema.LoadPath.SetVariableByName(settings, "Local.LoadPath");
        bundleSchema.BuildPath.SetVariableByName(settings, "Local.BuildPath");
        return group;
    }

    private static AddressableAssetGroup CreateNewGroup(string groupName)
    {
        AddressableAssetGroup oldGroup = settings.FindGroup(groupName);
        if (oldGroup != null)
        {
            settings.RemoveGroup(oldGroup);
        }
        AddressableAssetGroup group = settings.CreateGroup(groupName, false, false, false, null, typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
        group.GetSchema<BundledAssetGroupSchema>().BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
        return group;
    }

    private static AddressableAssetEntry CreateGroupEntry(string fullPath, AddressableAssetGroup group)
    {
        Debug.Log(fullPath);
        string guid = AssetDatabase.AssetPathToGUID(fullPath);
        var oldEntry = settings.FindAssetEntry(guid);
        if (oldEntry != null)
        {
            group.RemoveAssetEntry(oldEntry);
        }
        AddressableAssetEntry ret = settings.CreateOrMoveEntry(guid, group, false);
        ret.address = fullPath.Substring(fullPath.IndexOf(group.name));
        return ret;
    }

    private static AddressableAssetSettings settings;

    public static string build_script
            = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";

    public static string settings_asset
        = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";

    public static string profile_name = "Default";

    private static void getSettingsObject(string settingsAsset)
    {
        // This step is optional, you can also use the default settings:
        //settings = AddressableAssetSettingsDefaultObject.Settings;

        settings
            = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset)
                as AddressableAssetSettings;

        if (settings == null)
            Debug.LogError($"{settingsAsset} couldn't be found or isn't " +
                           $"a settings object.");
    }

    private static void setProfile(string profile)
    {
        string profileId = settings.profileSettings.GetProfileId(profile);
        if (string.IsNullOrEmpty(profileId))
            Debug.LogWarning($"Couldn't find a profile named, {profile}, " +
                             $"using current profile instead.");
        else
            settings.activeProfileId = profileId;
    }

    private static void setBuilder(IDataBuilder builder)
    {
        int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

        if (index > 0)
            settings.ActivePlayerDataBuilderIndex = index;
        else
            Debug.LogWarning($"{builder} must be added to the " +
                             $"DataBuilders list before it can be made " +
                             $"active. Using last run builder instead.");
    }

    private static bool buildAddressableContent()
    {
        AddressableAssetSettings
            .BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (!success)
        {
            Debug.LogError("Addressables build error encountered: " + result.Error);
        }
        return success;
    }

    [MenuItem("Tools/Addressables工作流工具/Build Addressables only")]
    public static bool BuildAddressables()
    {
        getSettingsObject(settings_asset);
        setProfile(profile_name);
        IDataBuilder builderScript
          = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;

        if (builderScript == null)
        {
            Debug.LogError(build_script + " couldn't be found or isn't a build script.");
            return false;
        }

        setBuilder(builderScript);

        return buildAddressableContent();
    }

    [MenuItem("Tools/Addressables工作流工具/Build Addressables and Player")]
    public static void BuildAddressablesAndPlayer()
    {
        bool contentBuildSucceeded = BuildAddressables();

        if (contentBuildSucceeded)
        {
            var options = new BuildPlayerOptions();
            BuildPlayerOptions playerSettings
                = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options);

            BuildPipeline.BuildPlayer(playerSettings);
        }
    }
}

#endif