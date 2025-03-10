using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FireBallAbility : Ability
{
    public Damage damage;
    public string AirNavMashName;

    private TileNavMeshAgent[] allNavMeshAgents;
    private Vector3Int startingLocation;
    private Vector3Int direction;
    private TileMapNavMesh m_airNavMesh;
    
    public override void Use()
    {

        allNavMeshAgents = GameObject.FindObjectsByType<TileNavMeshAgent>(FindObjectsSortMode.None);
        
        GameObject obj = GameObject.Find(AirNavMashName);
        if(obj != null)
            m_airNavMesh = obj.GetComponent<TileMapNavMesh>();

        Vector3Int targetLocation = Vector3Int.zero;
        m_caster.GetComponent<TileNavMeshAgent>().tileMapNavMesh.WorldPositionToTileLocation(m_context.m_mousePos, ref targetLocation);

        DoDamage(m_airNavMesh.GetRangeCordintates(targetLocation, 2));

        

        base.Use();
    }

    private void DoDamage(List<Vector3Int> range)
    {
        Debug.Log("Fire Ball: DoDamage()");
        for (int i = 0; i < range.Count; i++)
        {

            foreach (TileNavMeshAgent agent in allNavMeshAgents)
            {
                if(agent.location == range[i])
                {
                    agent.gameObject.GetComponent<Health>().Damage(damage);
                }
            }
        }

    }
}