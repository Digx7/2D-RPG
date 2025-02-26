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
    
    public override void Setup(CombatUnit newCaster)
    {
        m_caster = newCaster;

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIGroundTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        TileNavMeshAgent tileNavMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = tileNavMeshAgent.location;
        m_speed = m_caster.Stats.data.Speed;

        RenderUI();
    }

    protected override void RenderUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.GridFill(m_location, m_speed);
    }

    protected override void ClearUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.Clear();
    }
}

