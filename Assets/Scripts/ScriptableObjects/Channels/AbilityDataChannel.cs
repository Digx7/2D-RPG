using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ScriptableObjects/Channels/Abilities/Data", order = 1)]
public class AbilityDataChannel : ScriptableObject
{

    public AbilityDataEvent channelEvent = new AbilityDataEvent();

    public void Raise(AbilityData value)
    {
        channelEvent.Invoke(value);
    }

    
}
