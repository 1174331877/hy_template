using System.IO;
using UnityEngine;

public class CreateScript : UnityEditor.AssetModificationProcessor
{
    static string modelTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/ModelTemplete.txt";
    static string modulelTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/ModuleTemplete.txt";
    static string uiPanelTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/UIPanelTemplete.txt";
    static string uiWindowTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/UIWindowTemplete.txt";
    static string worldUIPanelTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/WorldUIPanelTemplete.txt";
    static string worldWindowTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/WorldUIWindowTemplete.txt";
    static string nDataTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/NDataTemplete.txt";
    static string mgrTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/MgrTemplete.txt";
    static string subViewTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/UISubViewTemplete.txt";
    static string entityTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/EntityTemplete.txt";
    static string componentTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/ComponentTemplete.txt";
    static string ltEditorOnlyMonoBehaviourTempletePath = Application.dataPath + "/Scripts/Editor/FastScript/LTEditorOnlayMonoBehaviourTemplete.txt";


    private static string WillCreateScript(string scriptName, string templateTxtPath)
    {
        string str = File.ReadAllText(templateTxtPath);
        str = str.Replace("#SCRIPTNAME#", scriptName);
        return str;
    }

    private static string GetScriptPathAndName(string path, out string filePath)
    {
        string[] pathNames = path.Split('/');
        string scriptAllPath = pathNames[pathNames.Length - 1];
        string[] names = scriptAllPath.Split('.');
        string fileName = names[0];
        filePath = path.Replace("/" + fileName, "");
        return fileName;
    }

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs") && path.Contains("Assets/Scripts"))
        {
            string filePath;
            string scriptName = GetScriptPathAndName(path, out filePath);
            string allText;
            //Debug.Log("path:" + path);
            //Debug.Log("scriptName:" + scriptName);
            //Debug.Log(LTUtility.DirectoryIsMatchInPath(path, "Mgr"));
            //Debug.Log(LTUtility.StringIsMatchIn(path, "Mgr"));
            //return;
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "UI") && scriptName.IndexOf("Panel") != -1 && scriptName.IndexOf("World") == -1)
            {
                allText = WillCreateScript(scriptName, uiPanelTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "UI") && scriptName.IndexOf("Panel") != -1 && scriptName.IndexOf("World") != -1)
            {
                allText = WillCreateScript(scriptName, worldUIPanelTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "Model") && scriptName.IndexOf("Model") != -1)
            {
                allText = WillCreateScript(scriptName, modelTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "Module") && scriptName.IndexOf("Module") != -1)
            {
                allText = WillCreateScript(scriptName, modulelTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && path.IndexOf("UI") != -1 && scriptName.IndexOf("Window") != -1 && scriptName.IndexOf("World") == -1)
            {
                allText = WillCreateScript(scriptName, uiWindowTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            bool isEditor = !LTUtility.IsEditorScript(path);
            if (isEditor && LTUtility.DirectoryIsMatchInPath(path, "UI") && scriptName.IndexOf("Window") != -1 && scriptName.IndexOf("World") != -1)
            {
                allText = WillCreateScript(scriptName, worldWindowTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "NetData") && LTUtility.StringIsMatchIn(scriptName, "Data"))
            {
                allText = WillCreateScript(scriptName, nDataTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "Mgr") && LTUtility.StringIsMatchIn(scriptName, "Mgr"))
            {
                allText = WillCreateScript(scriptName, mgrTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "SubView") && LTUtility.StringIsMatchIn(scriptName, "SubView"))
            {
                allText = WillCreateScript(scriptName, subViewTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "Entity") && LTUtility.StringIsMatchIn(scriptName, "Entity"))
            {
                allText = WillCreateScript(scriptName, entityTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (!LTUtility.IsEditorScript(path) && LTUtility.DirectoryIsMatchInPath(path, "Component") && LTUtility.StringIsMatchIn(scriptName, "Component"))
            {
                allText = WillCreateScript(scriptName, componentTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
            if (LTUtility.DirectoryIsMatchInPath(path, "EditorHelperExtention") && !LTUtility.IsEditorScript(path) && LTUtility.StringIsMatchIn(scriptName, "Component"))
            {
                allText = WillCreateScript(scriptName, ltEditorOnlyMonoBehaviourTempletePath);
                File.WriteAllText(path, allText);
                return;
            }
        }
    }


}