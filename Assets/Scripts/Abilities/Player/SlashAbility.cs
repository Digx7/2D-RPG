using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SlashAbility : Ability
{
    public Damage damage;

    private TileNavMeshAgent[] allNavMeshAgents;
    private Vector3Int startingLocation;
    private Vector3Int direction;
    
    public override void Use()
    {
        // TODO
        
        // Get targets based on navmesh

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);
        startingLocation = m_caster.gameObject.GetComponent<TileNavMeshAgent>().location;
        direction = Vector3Int.zero;

        Vector3Int mouseLoc = Vector3Int.zero;
        m_caster.GetComponent<TileNavMeshAgent>().tileMapNavMesh.WorldPositionToTileLocation(m_context.m_mousePos, ref mouseLoc);

        if(mouseLoc == (startingLocation + Vector3Int.right))
        {
            direction = Vector3Int.right;
            DoDamage();
        }
        else if(mouseLoc == (startingLocation + Vector3Int.left))
        {
            direction = Vector3Int.left;
            DoDamage();
        }

        

        base.Use();
    }

    private void DoDamage()
    {
        Debug.Log("SlashAbility: DoDamage()");
        for (int i = 0; i < 5; i++)
        {
            Vector3Int location = startingLocation + (direction * (i + 1));

            foreach (TileNavMeshAgent agent in allNavMeshAgents)
            {
                if(agent.location == location)
                {
                    agent.gameObject.GetComponent<Health>().Damage(damage);
                }
            }
        }

    }
}