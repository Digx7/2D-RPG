using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVector2Channel", menuName = "ScriptableObjects/Channels/Vector2", order = 1)]
public class Vector2Channel : ScriptableObject
{

    public Vector2Event channelEvent = new Vector2Event();

    public void Raise(Vector2 value)
    {
        channelEvent.Invoke(value);
    }
}
