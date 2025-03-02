using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EndPreview : AbilityPreview
{
    public string UIGroundTileMapDrawerName;

    private UITileMapDrawer m_UITileMapDrawer;
    private Vector3Int m_location;
    private TileNavMeshAgent tileNavMeshAgent;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIGroundTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        tileNavMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = tileNavMeshAgent.location;
    }

    protected override void RenderUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.TryPlaceTile(m_location);
    }

    protected override void ClearUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.Clear();
    }
}

