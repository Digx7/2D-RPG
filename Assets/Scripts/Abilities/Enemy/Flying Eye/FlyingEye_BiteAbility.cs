using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlyingEye_BiteAbility : Ability
{
    public Damage damage;

    private TileNavMeshAgent[] allNavMeshAgents;
    private List<Vector3Int> m_neighbors;
    
    public override void Use()
    {

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);

        m_neighbors = m_navMeshAgent.tileMapNavMesh.GetNeighborCordinates(m_location);
        m_neighbors.AddRange(m_navMeshAgent.tileMapNavMesh.GetCornerCordinates(m_location));

        foreach (Vector3Int neighbor in m_neighbors)
        {
            foreach (TileNavMeshAgent agent in allNavMeshAgents)
            {
                if(agent.location == neighbor) agent.gameObject.GetComponent<Health>().Damage(damage);
            }
        }

        

        base.Use();
    }
}