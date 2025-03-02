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
    private UITileMapDrawer m_UITileMapDrawer;
    
    public override void Use()
    {
        // Health health = m_caster.GetComponent<Health>();
        // health.Heal(HealAmount);

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIAirTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);
        allAgentsHealed = new List<TileNavMeshAgent>();
        startingLocation = m_caster.gameObject.GetComponent<TileNavMeshAgent>().location;
        List<Vector3Int> cordinates = m_UITileMapDrawer.GetGridCordinates(startingLocation, range);

        for (int i = 0; i < cordinates.Count; i++)
        {
            foreach (TileNavMeshAgent agent in allNavMeshAgents)
            {
                if(agent.location == cordinates[i] && !allAgentsHealed.Contains(agent))
                {
                    agent.gameObject.GetComponent<Health>().Heal(HealAmount);
                    allAgentsHealed.Add(agent);
                }
            }
        }

        base.Use();
    }
}