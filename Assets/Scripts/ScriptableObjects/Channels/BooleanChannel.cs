using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBooleanChannel", menuName = "ScriptableObjects/Channels/Boolean", order = 1)]
public class BooleanChannel : ScriptableObject
{

    public BooleanEvent channelEvent = new BooleanEvent();

    public void Raise(bool value)
    {
        channelEvent.Invoke(value);
    }

    
}
