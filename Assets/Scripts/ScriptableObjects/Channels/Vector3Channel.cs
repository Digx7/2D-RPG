using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVector3Channel", menuName = "ScriptableObjects/Channels/Vector3", order = 1)]
public class Vector3Channel : ScriptableObject
{
    public Vector3Event channelEvent = new Vector3Event();

    public void Raise(Vector3 value)
    {
        channelEvent.Invoke(value);
    }
}
