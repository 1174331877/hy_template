using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Unity引擎皮肤工具接口
/// 说明:所有编辑器二次开发相关得皮肤都从GUI.skin默认皮肤拷贝一份
/// </summary>
public class LTUnityEngineSkinUtil
{
    /// <summary>
    /// 获取一种类型组件样式副本
    /// </summary>
    /// <param name="templateStyle"></param>
    /// <param name="fontSize"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static GUIStyle GetGUIStyle(GUIStyle templateStyle, int fontSize, Color c, FontStyle fontStyle = FontStyle.Normal)
    {
        GUIStyle st = new GUIStyle(templateStyle);
        st.fontSize = fontSize;
        st.normal.textColor = c;
        st.fontStyle = fontStyle;
        return st;
    }
}
