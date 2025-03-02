using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityData))]
public class AbilityEditor : Editor
{
    AbilityData _abilityData;

    private void OnEnable()
    {
        _abilityData = target as AbilityData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(_abilityData.AbilityIcon == null)
            return;

        // Get the Sprite
        Texture2D sprite = AssetPreview.GetAssetPreview(_abilityData.AbilityIcon);

        // Define image size
        GUILayout.Label("", GUILayout.Height(120), GUILayout.Width(120));

        // Draw the image
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), sprite);

    }
}
