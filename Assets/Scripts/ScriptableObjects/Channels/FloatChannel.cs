using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFloatChannel", menuName = "ScriptableObjects/Channels/Float", order = 1)]
public class FloatChannel : ScriptableObject
{

    public FloatEvent channelEvent = new FloatEvent();

    public void Raise(float value)
    {
        channelEvent.Invoke(value);
    }

    
}
