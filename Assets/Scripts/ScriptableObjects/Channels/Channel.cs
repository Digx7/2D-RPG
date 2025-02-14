using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewChannel", menuName = "ScriptableObjects/Channels/Default", order = 1)]
public class Channel : ScriptableObject
{
    public UnityEvent channelEvent = new UnityEvent();

    public void Raise()
    {
        channelEvent.Invoke();
    }

}
