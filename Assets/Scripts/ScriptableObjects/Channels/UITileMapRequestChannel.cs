using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUITileMapRequest", menuName = "ScriptableObjects/Channels/UI/TileMap", order = 1)]
public class UITileMapRequestChannel : ScriptableObject
{

    public UITileMapRequestEvent channelEvent = new UITileMapRequestEvent();

    public void Raise(UITileMapRequest value)
    {
        channelEvent.Invoke(value);
    }

    
}
