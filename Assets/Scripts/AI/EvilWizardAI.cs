using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EvilWizardAI : AI
{
    private const int BLAST_INDEX = 1;
    private const int HEAL_INDEX = 2;
    private const int MOVE_INDEX = 3;

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
        Debug.Log("Evil Wizard: Start Thinking");
        
        AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
        abilityUsageContext.Setup();

        yield return new WaitForSeconds(thinkTime);

        bool playerCloseEnough = APlayerIsInRange(8);
        bool enemyCloseEnough = AnotherEnemyIsInRange(8);

        if(m_combatUnit.CurrentEnergy >= 2)
        {
            if(playerCloseEnough && enemyCloseEnough)
            {
                if(UnityEngine.Random.Range(0,1) >= 0.9)
                {
                    UseBlastAbility(GetClosestPlayer());
                }
                else
                {
                    UseHealAbility(GetClosestEnemy());
                }
            }
            else if(playerCloseEnough)
            {
                UseBlastAbility(GetClosestPlayer());
            }
            else if(enemyCloseEnough)
            {
                UseHealAbility(GetClosestEnemy());
            }
            else
            {
                UseEndTurn();
            }
        }
        else
        {
            UseEndTurn();
        }
    }

    private void UseBlastAbility(Vector3Int targetLocation)
    {
        AbilityUsageContext context = new AbilityUsageContext();
        context.Setup();
        context.m_mousePos = targetLocation;
        
        m_combatUnit.UseAbility(BLAST_INDEX, context);
    }

    private void UseHealAbility(Vector3Int targetLocation)
    {
        AbilityUsageContext context = new AbilityUsageContext();
        context.Setup();
        context.m_mousePos = targetLocation;
        
        m_combatUnit.UseAbility(HEAL_INDEX, context);
    }

    private void UseMoveAbility(AbilityUsageContext context)
    {
        m_combatUnit.UseAbility(MOVE_INDEX, context);
    }
}
