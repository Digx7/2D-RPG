using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlyingEye_BitePreview : AbilityPreview
{

    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.PLACE_MANY;
        request.locations = new List<Vector3Int>();
        request.locations.AddRange(m_navMeshAgent.tileMapNavMesh.GetNeighborCordinates(m_location));
        request.locations.AddRange(m_navMeshAgent.tileMapNavMesh.GetCornerCordinates(m_location));
        request.locations.Add(m_location + Vector3Int.right);
        request.context = selectedContext;

        requestUITileMapChannel.Raise(request);
    }

    protected override void ClearUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.CLEAR;
        request.context = selectedContext;

        requestUITileMapChannel.Raise(request);
    }
}

