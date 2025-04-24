using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SlashPreview : AbilityPreview
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
            Debug.Log("Slash Failed: no NavMesh found");
            return false;
        }

        Vector3Int selectedLocation = Vector3Int.zero;
        if(!tileMapNavMesh.WorldPositionToTileLocation(abilityUsageContext.m_mousePos, ref selectedLocation))
        {
            Debug.Log("Slash Failed: selected location is not on the map");
            
            return false;
        }

        if(selectedLocation == (m_location + Vector3Int.left) || selectedLocation == (m_location + Vector3Int.right))
        {
            return true;
        }
        else return false;

        return true;
    }

    protected override void RenderUI()
    {
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.PLACE_MANY;
        request.locations = new List<Vector3Int>();
        request.locations.Add(m_location + Vector3Int.left);
        request.locations.Add(m_location + Vector3Int.right);
        request.context = selectableContext;

        requestUITileMapChannel.Raise(request);
    }

    public override void RenderSelectionUI(AbilityUsageContext context)
    {
        Vector3Int location = Vector3Int.zero;
        if(m_navMeshAgent.tileMapNavMesh.WorldPositionToTileLocation(context.m_mousePos, ref location))
        {
            UITileMapRequest request = new UITileMapRequest();
            request.header = UITileMapRequestHeader.CLEAR;
            request.context = selectedContext;

            requestUITileMapChannel.Raise(request);

            // if(location == (m_location + Vector3Int.left) || location == (m_location + Vector3Int.right))
            // {
            //     request.header = UITileMapRequestHeader.PLACE;
            //     request.location = location;
            //     request.context = selectedContext;

            //     requestUITileMapChannel.Raise(request);
            // }

            if(location == (m_location + Vector3Int.left))
            {
                request.header = UITileMapRequestHeader.PLACE_MANY;
                request.locations = new List<Vector3Int>();

                for (int i = 0; i < 5; i++)
                {
                    request.locations.Add(m_location + (Vector3Int.left * (i + 1)));
                }

                request.context = selectedContext;

                requestUITileMapChannel.Raise(request);
            }
            else if(location == (m_location + Vector3Int.right))
            {
                request.header = UITileMapRequestHeader.PLACE_MANY;
                request.locations = new List<Vector3Int>();

                for (int i = 0; i < 5; i++)
                {
                    request.locations.Add(m_location + (Vector3Int.right * (i + 1)));
                }

                request.context = selectedContext;

                requestUITileMapChannel.Raise(request);
            }
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
}

