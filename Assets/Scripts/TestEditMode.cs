// The PrintAwake script is placed on a GameObject. Usually, the Awake function is
// called when the GameObject with this script is initialized at runtime. Due to the ExecuteInEditMode
// attribute, the Editor also calls Awake when the script component is created via an Editor menu or when a scene that contains it is loaded.
// The Update function is called when the Scene view needs to render, which happens when something in the scene changes or you navigate the scene with mouse or keyboard inputs.

using UnityEngine;

[ExecuteInEditMode]
public class TestEditMode : MonoBehaviour
{
    void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("Editor only Awake");   
        }
    }

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("Editor only Update");
        }
    }
}