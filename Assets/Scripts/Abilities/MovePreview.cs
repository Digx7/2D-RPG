using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovePreview : AbilityPreview
{
    public string UIGroundTileMapDrawerName;

    private UITileMapDrawer m_UITileMapDrawer;
    private Vector3Int m_location;
    private int m_speed;
    private TileNavMeshAgent tileNavMeshAgent;
    
    public override void Setup(CombatUnit newCaster)
    {
        m_caster = newCaster;

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIGroundTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        tileNavMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = tileNavMeshAgent.location;
        m_speed = m_caster.Stats.data.Speed;

        RenderUI();
    }

    public override bool Validate(AbilityUsageContext abilityUsageContext)
    {
        TileMapNavMesh tileMapNavMesh = tileNavMeshAgent.tileMapNavMesh;
        if(tileMapNavMesh == null) 
        {
            return false;
        }

        Vector3Int endLocation = Vector3Int.zero;
        if(!tileMapNavMesh.WorldPositionToTileLocation(abilityUsageContext.m_mousePos, ref endLocation))
        {
            return false;
        }

        List<TileNavMeshNode> path = new List<TileNavMeshNode>();

        if(!tileMapNavMesh.GetPath(tileNavMeshAgent.location, endLocation, ref path))
        {
            return false;
        }

        if(path.Count > m_caster.Stats.data.Speed)
        {
            return false;
        }

        return true;
    }

    protected override void RenderUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.GridFill(m_location, (m_speed - 1));
    }

    protected override void ClearUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.Clear();
    }
}

