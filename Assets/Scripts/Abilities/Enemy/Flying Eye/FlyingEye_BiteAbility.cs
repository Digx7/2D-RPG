using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlyingEye_BiteAbility : Ability
{
    public Damage damage;
    public string UIAirTileMapDrawerName;

    private UITileMapDrawer m_UITileMapDrawer;

    private TileNavMeshAgent[] allNavMeshAgents;
    private Vector3Int m_location;
    private List<Vector3Int> m_neighbors;
    
    public override void Use()
    {
        // TODO
        
        // Get targets based on navmesh

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIAirTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);
        m_location = m_caster.gameObject.GetComponent<TileNavMeshAgent>().location;

        m_neighbors = m_UITileMapDrawer.GetNeighborCordinates(m_location);

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