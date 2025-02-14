using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStringChannel", menuName = "ScriptableObjects/Channels/String", order = 1)]
public class StringChannel : ScriptableObject
{
    public StringEvent channelEvent = new StringEvent();

    public void Raise(string value)
    {
        channelEvent.Invoke(value);
    }

    
}
