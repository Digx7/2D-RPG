using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class JumpAbility : Ability
{

    private Movement2D movement2D;
    private TileNavMeshAgent tileNavMeshAgent;
    
    public override void Use()
    {
        movement2D = m_caster.GetComponent<Movement2D>();
        tileNavMeshAgent = m_caster.GetComponent<TileNavMeshAgent>();
        
        tileNavMeshAgent.TryTeleportToWorldPosition(m_context.m_mousePos);
        base.Use();
    }
}