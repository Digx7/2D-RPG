using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewIntChannel", menuName = "ScriptableObjects/Channels/Int", order = 1)]
public class IntChannel : ScriptableObject
{

    public IntEvent channelEvent = new IntEvent();

    public void Raise(int value)
    {
        channelEvent.Invoke(value);
    }
}
