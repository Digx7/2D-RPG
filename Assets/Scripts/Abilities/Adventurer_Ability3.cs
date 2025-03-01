using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Adventurer_Ability3 : Ability
{
    public float moveTime;
    public Vector2 moveDirection;

    private Movement2D movement2D;
    private TileNavMeshAgent tileNavMeshAgent;
    
    public override void Use()
    {
        Debug.Log("Adventurer Uses " + AbilityName);

        movement2D = m_caster.GetComponent<Movement2D>();
        tileNavMeshAgent = m_caster.GetComponent<TileNavMeshAgent>();
        tileNavMeshAgent.TryToMoveToWorldPosition(m_context.m_mousePos);
        tileNavMeshAgent.OnReachEndOfPath.AddListener(EndMove);

        m_caster.animator.SetBool("IsRunning", true);


        // StartCoroutine(Move());

        // base.Use();
    }

    public void EndMove()
    {
        m_caster.animator.SetBool("IsRunning", false);
        tileNavMeshAgent.OnReachEndOfPath.RemoveListener(EndMove);
        Teardown();
    }

    IEnumerator Move()
    {
        // movement2D.setDesiredMoveDirection(moveDirection);
        m_caster.animator.SetBool("IsRunning", true);
        yield return new WaitForSeconds(moveTime);
        // movement2D.setDesiredMoveDirection(new Vector2(0,0));
        m_caster.animator.SetBool("IsRunning", false);
    }
}