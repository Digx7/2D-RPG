using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovePreview : AbilityPreview
{

    private int m_speed;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);
        m_speed = m_caster.Stats.data.Speed;
    }

    public override bool Validate(AbilityUsageContext abilityUsageContext)
    {
        TileMapNavMesh tileMapNavMesh = m_navMeshAgent.tileMapNavMesh;
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

        if(!tileMapNavMesh.GetPath(m_location, endLocation, ref path))
        {
            return false;
        }

        if(path.Count > m_caster.Stats.data.Speed)
        {
            Debug.Log("Move Preview: Validation failed because the Path was to long");
            return false;
        }

        return true;
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.FILL;
        request.location = m_location;
        request.range = m_caster.Stats.data.Speed;
        request.context = selectableContext;

        requestUITileMapChannel.Raise(request);
    }

    public override void RenderSelectionUI(AbilityUsageContext context)
    {
        Vector3Int endLocation = Vector3Int.zero;
        if(m_navMeshAgent.tileMapNavMesh.WorldPositionToTileLocation(context.m_mousePos, ref endLocation))
        {
            UITileMapRequest request = new UITileMapRequest();
            request.header = UITileMapRequestHeader.CLEAR;
            request.context = lineContext;

            requestUITileMapChannel.Raise(request);


            request.header = UITileMapRequestHeader.PATH;
            request.locations = new List<Vector3Int>();
            request.locations.Add(m_location);
            request.locations.Add(endLocation);
            request.range = m_caster.Stats.data.Speed;
            request.context = lineContext;

            requestUITileMapChannel.Raise(request);
        }
        else
        {
            UITileMapRequest request = new UITileMapRequest();
            request.header = UITileMapRequestHeader.CLEAR;
            request.context = lineContext;

            requestUITileMapChannel.Raise(request);
        }
    }

    protected override void ClearUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.CLEAR;
        request.context = selectableContext;

        requestUITileMapChannel.Raise(request);


        request.header = UITileMapRequestHeader.CLEAR;
        request.context = lineContext;

        requestUITileMapChannel.Raise(request);
    }
}

