using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
    public float thinkTime = 0.5f;
    protected CombatUnit m_combatUnit;
    protected TileNavMeshAgent m_tileNavMeshAgent;
    protected TileMapNavMesh m_NavMesh;

    protected virtual void Awake()
    {
        m_combatUnit = GetComponent<CombatUnit>();
        m_tileNavMeshAgent = GetComponent<TileNavMeshAgent>();
    }

    protected virtual void Start()
    {

    }

    public virtual void OnTurn()
    {
        m_NavMesh = m_tileNavMeshAgent.tileMapNavMesh;
    }

    protected virtual bool APlayerIsInRange(float range)
    {
        GameObject[] allPlayerUnits = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerUnit in allPlayerUnits)
        {
            Vector3Int playerLoc = playerUnit.GetComponent<TileNavMeshAgent>().location;

            if(Vector3Int.Distance(playerLoc, m_tileNavMeshAgent.location) <= range) 
                return true;
        }

        return false;
    }

    protected virtual bool AnotherEnemyIsInRange(float range)
    {
        GameObject[] allPlayerUnits = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject playerUnit in allPlayerUnits)
        {
            Vector3Int enemyLoc = playerUnit.GetComponent<TileNavMeshAgent>().location;

            if(enemyLoc != m_tileNavMeshAgent.location && Vector3Int.Distance(enemyLoc, m_tileNavMeshAgent.location) <= range) 
                return true;
        }

        return false;
    }

    protected Vector3Int GetClosestPlayer()
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

    protected Vector3Int GetClosestEnemy()
    {
        GameObject[] allEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        float smallestDistance = 999f;
        Vector3Int output = new Vector3Int(999,999,999);

        foreach (GameObject enemyUnit in allEnemyUnits)
        {
            Vector3Int enemyLoc = enemyUnit.GetComponent<TileNavMeshAgent>().location;

            float distance = Vector3Int.Distance(enemyLoc, m_tileNavMeshAgent.location);

            if(enemyLoc != m_tileNavMeshAgent.location && distance < smallestDistance) 
            {
                smallestDistance = distance;
                output = enemyLoc;
            }
        }

        output += Vector3Int.right;

        return output;
    }

    protected void UseEndTurn()
    {
        AbilityUsageContext context = new AbilityUsageContext();
        context.Setup();
        
        m_combatUnit.UseAbility(0, context);
    }
}
