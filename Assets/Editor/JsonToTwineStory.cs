using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class JsonToTwineStory : Editor
{
    [MenuItem("Assets/Create/TwineStory")]
    public static void CreateDialogue()
    {
        TextAsset textAsset = (TextAsset) Selection.activeObject;

        TwineStory twineStory = ScriptableObject.CreateInstance<TwineStory>();

        JsonUtility.FromJsonOverwrite(textAsset.text, twineStory);

        AssetDatabase.CreateAsset(twineStory, "Assets/New Twine Story.asset");
        AssetDatabase.SaveAssets();
    }
}
