using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityPreview : MonoBehaviour
{
    public UITileMapRequestChannel requestUITileMapChannel;
    public UITileMapContext selectableContext;
    public UITileMapContext selectedContext;
    public UITileMapContext lineContext;
    protected CombatUnit m_caster;
    protected TileNavMeshAgent m_navMeshAgent;
    protected Vector3Int m_location;
    
    public virtual void Setup(CombatUnit newCaster)
    {
        m_caster = newCaster;
        m_navMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = m_navMeshAgent.location;
    }

    public virtual void Use()
    {
        RenderUI();
    }

    public virtual bool Validate(AbilityUsageContext abilityUsageContext)
    {
        return true;
    }

    protected virtual void RenderUI() {}

    public virtual void RenderSelectionUI(AbilityUsageContext abilityUsageContext) {}

    protected virtual void ClearUI() {}

    public virtual void Teardown()
    {
        ClearUI();
        Destroy(gameObject);
    }
}

