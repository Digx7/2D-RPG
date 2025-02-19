using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSpriteList", menuName = "ScriptableObjects/Channels/List/Sprite", order = 1)]
public class SpriteListChannel : ScriptableObject
{

    public SpriteListEvent channelEvent = new SpriteListEvent();

    public void Raise(List<Sprite> value)
    {
        channelEvent.Invoke(value);
    }

    
}
