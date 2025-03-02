using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoblinAI : MonoBehaviour
{
    public float thinkTime = 0.5f;
    private CombatUnit m_combatUnit;
    private TileNavMeshAgent m_tileNavMeshAgent;
    private TileMapNavMesh m_NavMesh;

    private void Awake()
    {
        m_combatUnit = GetComponent<CombatUnit>();
        m_tileNavMeshAgent = GetComponent<TileNavMeshAgent>();
    }

    public void OnTurn()
    {
        m_NavMesh = m_tileNavMeshAgent.tileMapNavMesh;
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
        abilityUsageContext.Setup();

        yield return new WaitForSeconds(thinkTime);
        // if(m_combatUnit.CurrentEnergy >= 2)
        //     m_combatUnit.UseAbility(0, abilityUsageContext);
        // else m_combatUnit.UseAbility(1, abilityUsageContext);

        if(APlayerIsInRange())
        {
            if(m_combatUnit.CurrentEnergy >= m_combatUnit.abilities[1].EnergyCost)
                m_combatUnit.UseAbility(1, abilityUsageContext);
            else m_combatUnit.UseAbility(0, abilityUsageContext);
        }
        else
        {
            // Move to closest player
            Vector3Int closestPlayer = GetClosestPlayer();
            List<TileNavMeshNode> path = new List<TileNavMeshNode>();

            if(m_NavMesh.GetPath(m_tileNavMeshAgent.location, closestPlayer, ref path))
            {
                Debug.Log("Path Length: " + path.Count + " trying to access index " + (m_combatUnit.Stats.data.Speed - 1));
                
                int index = m_combatUnit.Stats.data.Speed - 1;

                if(index >= path.Count) index = path.Count - 1;

                abilityUsageContext.m_mousePos = path[index].position;
                m_combatUnit.UseAbility(2, abilityUsageContext);
            }
            else
            {
                m_combatUnit.UseAbility(0, abilityUsageContext);
            }
        }
    }

    private bool APlayerIsInRange()
    {
        GameObject[] allPlayerUnits = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerUnit in allPlayerUnits)
        {
            Vector3Int playerLoc = playerUnit.GetComponent<TileNavMeshAgent>().location;

            if(Vector3Int.Distance(playerLoc, m_tileNavMeshAgent.location) <= 1) 
                return true;
        }

        return false;
    }

    private Vector3Int GetClosestPlayer()
    {
        GameObject[] allPlayerUnits = GameObject.FindGameObjectsWithTag("Player");
        float smallestDistance = 999f;
        Vector3Int output = new Vector3Int(999,999,999);

        foreach (GameObject playerUnit in allPlayerUnits)
        {
            Vector3Int playerLoc = playerUnit.GetComponent<TileNavMeshAgent>().location;

            float distance = Vector3Int.Distance(playerLoc, m_tileNavMeshAgent.location);

            if(distance < smallestDistance) 
            {
                smallestDistance = distance;
                output = playerLoc;
            }
        }

        output += Vector3Int.right;

        return output;
    }
}
