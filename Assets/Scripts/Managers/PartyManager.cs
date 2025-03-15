using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class PartyManager : Singleton<PartyManager>
{
    public List<GameObject> PartyMembers;
    private List<CombatUnit> spawnedPartyMembers;

    public override void Awake()
    {
        base.Awake();

        spawnedPartyMembers = new List<CombatUnit>();
    }

    public List<CombatUnit> PlacePartyMembers(List<PartyMemberSpawnPoint> allValidSpawnPoints)
    {
        spawnedPartyMembers.Clear();

        for (int i = 0; i < allValidSpawnPoints.Count; i++)
        {
            if(i >= PartyMembers.Count) break;
            
            GameObject obj = Instantiate(PartyMembers[i], allValidSpawnPoints[i].transform.position, Quaternion.identity);
            CombatUnit partyMember = obj.GetComponent<CombatUnit>();

            spawnedPartyMembers.Add(partyMember);
        }

        return spawnedPartyMembers;
    }

    public void RemoveAllPartyMembers()
    {
        for (int i = 0; i < spawnedPartyMembers.Count; i++)
        {
            if(spawnedPartyMembers[i] != null) spawnedPartyMembers[i].DestroyUnit();
        }

        spawnedPartyMembers.Clear();
    }
}