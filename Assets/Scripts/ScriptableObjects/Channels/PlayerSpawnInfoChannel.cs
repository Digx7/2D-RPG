using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPlayerSpawnInfoChannel", menuName = "ScriptableObjects/Channels/PlayerSpawnInfo", order = 1)]
public class PlayerSpawnInfoChannel : ScriptableObject
{

    public PlayerSpawnInfoEvent channelEvent = new PlayerSpawnInfoEvent();

    public void Raise(PlayerSpawnInfo value)
    {
        channelEvent.Invoke(value);
    }

    
}
