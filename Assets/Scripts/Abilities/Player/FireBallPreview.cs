using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FireBallPreview : AbilityPreview
{
    public string AirNavMashName;
    private TileMapNavMesh m_airNavMesh;
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);

        GameObject obj = GameObject.Find(AirNavMashName);
        if(obj != null)
            m_airNavMesh = obj.GetComponent<TileMapNavMesh>();
    }

    public override bool Validate(AbilityUsageContext abilityUsageContext)
    {
        if(m_airNavMesh == null) 
        {
            Debug.Log("Fire Ball Failed: no NavMesh found");
            return false;
        }

        Vector3Int selectedLocation = Vector3Int.zero;
        if(!m_airNavMesh.WorldPositionToTileLocation(abilityUsageContext.m_mousePos, ref selectedLocation))
        {
            Debug.Log("Fire Ball Failed: selected location is not on the map");
            
            return false;
        }

        if(IsInLocationInRange(selectedLocation))
        {
            return true;
        }

        return false;
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.FILL;
        request.location = m_location;
        request.range = 8;
        request.context = selectableContext;

        requestUITileMapChannel.Raise(request);
    }

    public override void RenderSelectionUI(AbilityUsageContext context)
    {
        Vector3Int targetLocation = Vector3Int.zero;
        if(m_airNavMesh.WorldPositionToTileLocation(context.m_mousePos, ref targetLocation) && IsInLocationInRange(targetLocation))
        {
            UITileMapRequest request = new UITileMapRequest();
            request.header = UITileMapRequestHeader.CLEAR;
            request.context = selectedContext;

            requestUITileMapChannel.Raise(request);

            request.header = UITileMapRequestHeader.FILL;
            request.location = targetLocation;
            request.range = 2;
            request.context = selectedContext;
            requestUITileMapChannel.Raise(request);
        }
        else
        {
            UITileMapRequest request = new UITileMapRequest();
            request.header = UITileMapRequestHeader.CLEAR;
            request.context = selectedContext;

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
        request.context = selectedContext;

        requestUITileMapChannel.Raise(request);
    }

    private bool IsInLocationInRange(Vector3Int targetLocation)
    {
        List<Vector3Int> validLocations = m_airNavMesh.GetRangeCordintates(m_location, 8);

        foreach (Vector3Int location in validLocations)
        {
            if(location == targetLocation)
                return true;
        }

        return false;
    }
}

