using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatInstance : MonoBehaviour
{
    public List<PartyMemberSpawnPoint> partyMemberSpawnPoints;
    public List<CombatUnit> allEnemies;

    public CombatInstanceChannel notifyActiveCombatInstance;
    public Channel requestCombatStartChannel;

    public bool isActive {get; private set;} = false;
    public bool hasBeenFinished {get; private set;} = false;

    public void OnEnable()
    {
        requestCombatStartChannel.channelEvent.AddListener(StartCombat);
    }

    public void OnDisable()
    {
        requestCombatStartChannel.channelEvent.RemoveListener(StartCombat);
    }

    public void StartCombat()
    {
        if(isActive) notifyActiveCombatInstance.Raise(this);
    }

    public void SetInstanceActive(bool value)
    {
        isActive = value;
    }

    public void SethasBeenFinished(bool value)
    {
        hasBeenFinished = value;
    }
}

[System.Serializable]
public class CombatInstanceEvent : UnityEvent<CombatInstance> {}