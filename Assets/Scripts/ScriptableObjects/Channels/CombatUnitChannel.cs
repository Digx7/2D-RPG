using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCombatUnit", menuName = "ScriptableObjects/Channels/CombatUnit", order = 1)]
public class CombatUnitChannel : ScriptableObject
{

    public CombatUnitEvent channelEvent = new CombatUnitEvent();

    public void Raise(CombatUnit value)
    {
        channelEvent.Invoke(value);
    }

    
}
