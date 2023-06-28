using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class FolderMenu
{
    private static string projectRootFolder;

    static FolderMenu()
    {
        projectRootFolder = new DirectoryInfo(Application.dataPath).Parent.Parent.FullName;
    }

    [MenuItem("常用文件夹/打开配置目录")]
    private static void OpenDataTableForlder()
    {
        string path = Path.Combine(projectRootFolder, "cfgs", "Datas");
        Application.OpenURL(path);
    }
}