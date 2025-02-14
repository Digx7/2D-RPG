using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSceneContextChannel", menuName = "ScriptableObjects/Channels/Scene/Context", order = 1)]
public class SceneContextChannel : ScriptableObject
{

    public SceneContextEvent channelEvent = new SceneContextEvent();

    public SceneContext lastValue {get; private set;}

    private void OnEnable()
    {
        ResetLastValue();
    }

    private void ResetLastValue()
    {
        lastValue = new SceneContext();
    }

    public void Raise(SceneContext value)
    {
        lastValue = value;
        channelEvent.Invoke(value);
    }

    
}
