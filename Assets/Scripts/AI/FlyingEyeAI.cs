using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FlyingEyeAI : AI
{
    private const int BITE_INDEX = 1;
    private const int MOVE_INDEX = 2;

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
        Debug.Log("GoblinAI: Start Thinking");
        
        AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
        abilityUsageContext.Setup();

        yield return new WaitForSeconds(thinkTime);

        if(APlayerIsInRange(1))
        {
            if(m_combatUnit.CurrentEnergy >= m_combatUnit.abilities[1].EnergyCost)
                UseBiteAbility();
            else UseEndTurn();
        }
        else
        {
            // Move to closest player
            MoveToClosestPlayer();
        }
    }

    private void MoveToClosestPlayer()
    {
        AbilityUsageContext context = new AbilityUsageContext();
        context.Setup();
        
        Vector3Int closestPlayer = GetClosestPlayer();
        List<TileNavMeshNode> path = new List<TileNavMeshNode>();

        if(m_NavMesh.GetPath(m_tileNavMeshAgent.location, closestPlayer, ref path))
        {
            Debug.Log("Path Length: " + path.Count + " trying to access index " + (m_combatUnit.Stats.Speed.TrueValue() - 1));
            
            int index = m_combatUnit.Stats.Speed.TrueValue() - 1;

            if(index >= path.Count) index = path.Count - 1;

            context.m_mousePos = path[index].position;
            UseMoveAbility(context);
        }
        else
        {
            UseEndTurn();
        }
    }

    private void UseBiteAbility()
    {
        AbilityUsageContext context = new AbilityUsageContext();
        context.Setup();

        m_combatUnit.UseAbility(BITE_INDEX, context);
    }

    private void UseMoveAbility(AbilityUsageContext context)
    {
        m_combatUnit.UseAbility(MOVE_INDEX, context);
    }  

    
}
