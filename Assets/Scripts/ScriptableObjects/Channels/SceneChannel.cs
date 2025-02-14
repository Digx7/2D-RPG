using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSceneChannel", menuName = "ScriptableObjects/Channels/Scene/Default", order = 1)]
public class SceneChannel : ScriptableObject
{

    public SceneEvent channelEvent = new SceneEvent();

    public SceneData lastValue {get; private set;}

    private void OnEnable()
    {
        ResetLastValue();
    }

    public void ResetLastValue()
    {
        lastValue.Clear();
    }
    
    public void Raise(SceneData value)
    {
        lastValue = value;
        channelEvent.Invoke(value);
    }

    
}
