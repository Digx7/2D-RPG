using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGameObjectChannel", menuName = "ScriptableObjects/Channels/GameObject", order = 1)]
public class GameObjectChannel : ScriptableObject
{

    public GameObjectEvent channelEvent = new GameObjectEvent();

    public void Raise(GameObject value)
    {
        channelEvent.Invoke(value);
    }

    
}
