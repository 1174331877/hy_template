using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

/// <summary>
/// LT多语言组件自定义Inspector脚本
/// </summary>
[CustomEditor(typeof(LTText))]
public class LTTextMeshProUGUIEditor : TMP_EditorPanelUI
{
    private LTText m_Text;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Text = (LTText)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUIStyle style = LTUnityEngineSkinUtil.GetGUIStyle(GUI.skin.label, 16, Color.green, FontStyle.Bold);
        style.normal.textColor = Color.green;
        style.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("乐推多语言组件", style);
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.LabelField("多语言配置Id", m_Text.textId.ToString(), style);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}