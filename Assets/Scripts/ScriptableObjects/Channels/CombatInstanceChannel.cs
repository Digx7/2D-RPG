using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ScriptableObjects/Channels/Combat/Instance", order = 1)]
public class CombatInstanceChannel : ScriptableObject
{

    public CombatInstanceEvent channelEvent = new CombatInstanceEvent();

    public void Raise(CombatInstance value)
    {
        channelEvent.Invoke(value);
    }

    
}
