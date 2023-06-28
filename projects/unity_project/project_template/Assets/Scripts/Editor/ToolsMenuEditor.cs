using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class ToolsMenuEditor
{
    [MenuItem("Tools/删除玩家数据")]
    static void DelatePlayerData()
    {
        string apth = Application.dataPath;
        DirectoryInfo dirInfo = new DirectoryInfo(apth);
        string fileFullPath = Path.Combine(dirInfo.Parent.FullName, "player.data");
        if (File.Exists(fileFullPath))
        {
            File.Delete(fileFullPath);
            Debug.Log("成功删除玩家数据!" + DateTime.UtcNow.ToUniversalTime());
        }
    }
}
