using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCombatUnit", menuName = "ScriptableObjects/Channels/CombatUnit/List", order = 1)]
public class CombatUnitListChannel : ScriptableObject
{

    public CombatUnitListEvent channelEvent = new CombatUnitListEvent();

    public void Raise(List<CombatUnit> value)
    {
        channelEvent.Invoke(value);
    }

    
}
