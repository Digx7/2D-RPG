using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovePreview : AbilityPreview
{

   
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);
    }

    public override bool Validate(AbilityUsageContext abilityUsageContext)
    {
        TileMapNavMesh tileMapNavMesh = m_navMeshAgent.tileMapNavMesh;
        if(tileMapNavMesh == null) 
        {
            Debug.Log("Move Validation Failed: TileMapNavMesh == NULL");
            return false;
        }

        Vector3Int endLocation = Vector3Int.zero;
        if(!tileMapNavMesh.WorldPositionToTileLocation(abilityUsageContext.m_mousePos, ref endLocation))
        {
            Debug.Log("Move Validation Failed: Chosen location is not on the NavMesh (Screen Location: " + abilityUsageContext.m_mousePos + " NavMesh Location: " + endLocation);
            return false;
        }

        if(tileMapNavMesh.IsLocationOccupied(endLocation))
        {
            Debug.Log("Move Validation Failed: Location (" + endLocation + ") is occupied");
            return false;
        }

        List<TileNavMeshNode> path = new List<TileNavMeshNode>();

        if(!tileMapNavMesh.GetPath(m_location, endLocation, ref path))
        {
            Debug.Log("Move Validation Failed: Get Path Failed");
            return false;
        }

        if(path.Count > m_caster.Stats.Speed.TrueValue())
        {
            Debug.Log("Move Validation Failed: Path.Count (" + path.Count + " > " + m_caster.Stats.Speed.TrueValue() + " Speed");
            
            foreach (TileNavMeshNode node in path)
            {
                Debug.Log("" + node.position);
            }

            return false;
        }

        return true;
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.FILL;
        request.location = m_location;
        request.range = m_caster.Stats.Speed.TrueValue();
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
            request.range = m_caster.Stats.Speed.TrueValue();
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

