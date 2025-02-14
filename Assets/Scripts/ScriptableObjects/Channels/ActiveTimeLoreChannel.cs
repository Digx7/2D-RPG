using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewActiveTimeLore", menuName = "ScriptableObjects/Channels/Lore/ActiveTime", order = 1)]
public class ActiveTimeLoreChannel : ScriptableObject
{

    public ActiveTimeLoreEvent channelEvent = new ActiveTimeLoreEvent();

    public void Raise(ActiveTimeLore value)
    {
        channelEvent.Invoke(value);
    }

    
}
