using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HealAbility : Ability
{
    public Damage HealAmount;
    public int range = 3;
    public string UIAirTileMapDrawerName;

    private TileNavMeshAgent[] allNavMeshAgents;
    private List<TileNavMeshAgent> allAgentsHealed;
    private Vector3Int startingLocation;
    
    public override void Use()
    {

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);
        allAgentsHealed = new List<TileNavMeshAgent>();
        startingLocation = m_caster.gameObject.GetComponent<TileNavMeshAgent>().location;
        List<Vector3Int> cordinates = m_navMeshAgent.tileMapNavMesh.GetRangeCordintates(startingLocation, range);

        for (int i = 0; i < cordinates.Count; i++)
        {
            foreach (TileNavMeshAgent agent in allNavMeshAgents)
            {
                if(agent.location == cordinates[i] && !allAgentsHealed.Contains(agent))
                {
                    Damage trueDamage = HealAmount;
                    trueDamage.amount += m_caster.Stats.Wisdom.TrueValue();
                    agent.gameObject.GetComponent<Health>().Heal(trueDamage);
                    allAgentsHealed.Add(agent);
                }
            }
        }

        base.Use();
    }
}