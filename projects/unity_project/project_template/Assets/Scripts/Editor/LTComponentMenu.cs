using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;

public static class LTComponentMenu
{
    [MenuItem("GameObject/UI/LT/LTText")]
    private static void CreateLTText()
    {
        Transform parent = Selection.activeTransform;
        GameObject go = new GameObject("LTText", typeof(LTText));
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        Selection.activeObject = go;
        Preset preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/LTText.preset");
        preset.ApplyTo(go.GetCom<LTText>());
    }

    [MenuItem("GameObject/UI/LT/LTTextWorld")]
    private static void CreateLTTextWorld()
    {
        Transform parent = Selection.activeTransform;
        GameObject go = new GameObject("LTTextWorld", typeof(LTTextWorld));
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        var rectTrans = go.transform as RectTransform;
        rectTrans.sizeDelta = Vector2.one;
        Selection.activeObject = go;
        Preset preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/LTTextWorld.preset");
        preset.ApplyTo(go.GetCom<LTTextWorld>());
    }

    [MenuItem("GameObject/UI/LT/LTButton")]
    private static void CreateLTButton()
    {
        Transform parent = Selection.activeTransform;
        GameObject go = new GameObject("LTButton", typeof(Image), typeof(Button));
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        Button btn = go.GetCom<Button>();
        btn.transition = Selectable.Transition.SpriteSwap;
        Selection.activeTransform = btn.transform;
        CreateLTText();
        Selection.activeObject = go;
    }
}