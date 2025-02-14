using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSFXChannel", menuName = "ScriptableObjects/Channels/SFX", order = 1)]
public class SFXChannel : ScriptableObject
{

    public SFXEvent channelEvent = new SFXEvent();

    public void Raise(string name, Vector3 location)
    {
        channelEvent.Invoke(name, location);
    }

    
}
