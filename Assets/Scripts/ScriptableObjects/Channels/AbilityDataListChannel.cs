using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbilityDataList", menuName = "ScriptableObjects/Channels/Abilities/DataList", order = 1)]
public class AbilityDataListChannel : ScriptableObject
{

    public AbilityDataListEvent channelEvent = new AbilityDataListEvent();

    public void Raise(List<AbilityData> value)
    {
        channelEvent.Invoke(value);
    }

    
}
