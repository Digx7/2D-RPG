using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SlashPreview : AbilityPreview
{
    public string UIAirTileMapDrawerName;

    private UITileMapDrawer m_UITileMapDrawer;
    private Vector3Int m_location;
    private TileNavMeshAgent tileNavMeshAgent;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);

        UITileMapDrawer[] uITileMapDrawers = GameObject.FindObjectsByType<UITileMapDrawer>(FindObjectsSortMode.None);

        foreach (UITileMapDrawer drawer in uITileMapDrawers)
        {
            if(drawer.gameObject.name == UIAirTileMapDrawerName) m_UITileMapDrawer = drawer;
        }

        tileNavMeshAgent = m_caster.gameObject.GetComponent<TileNavMeshAgent>();
        m_location = tileNavMeshAgent.location;

        // RenderUI();
    }

    public override bool Validate(AbilityUsageContext abilityUsageContext)
    {
        // TODO

        return true;
    }

    protected override void RenderUI()
    {
        // if(m_UITileMapDrawer != null) m_UITileMapDrawer.LineFill(m_location + new Vector3Int(1,0,0), new Vector3Int(1,0,0), 5);
        if(m_UITileMapDrawer != null)
        {
            m_UITileMapDrawer.TryPlaceTile(m_location + Vector3Int.right);
            m_UITileMapDrawer.TryPlaceTile(m_location + Vector3Int.left);
        }
    }

    protected override void ClearUI()
    {
        if(m_UITileMapDrawer != null) m_UITileMapDrawer.Clear();
    }
}

