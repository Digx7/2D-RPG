using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HealPreview : AbilityPreview
{
    public string UIAirTileMapDrawerName;

    private UITileMapDrawer m_UITileMapDrawer;
    private Vector3Int m_location;
    private int m_range = 3;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIAirTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        TileNavMeshAgent tileNavMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = tileNavMeshAgent.location;

        // RenderUI();
    }

    protected override void RenderUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.GridFill(m_location, m_range);
    }

    protected override void ClearUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.Clear();
    }
}

