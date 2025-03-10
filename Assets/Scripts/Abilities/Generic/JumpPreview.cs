using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class JumpPreview : AbilityPreview
{
    public string AirNavMashName;
    private int m_speed;
    private TileMapNavMesh m_groundedNavMash;
    private TileMapNavMesh m_airNavMesh;
    
    public override void Setup(CombatUnit newCaster)
    {
        base.Setup(newCaster);
        m_speed = m_caster.Stats.data.Speed;
        m_groundedNavMash = m_navMeshAgent.tileMapNavMesh;

        GameObject obj = GameObject.Find(AirNavMashName);
        if(obj != null)
            m_airNavMesh = obj.GetComponent<TileMapNavMesh>();
    }

    public override bool Validate(AbilityUsageContext context)
    {
        if(m_groundedNavMash == null) 
        {
            Debug.Log("Jump Validation Failed: TileMapNavMesh == NULL");
            return false;
        }

        Vector3Int targetLocation = Vector3Int.zero;
        if(!m_groundedNavMash.WorldPositionToTileLocation(context.m_mousePos, ref targetLocation))
        {
            Debug.Log("Jump Validation Failed: Chosen location is not on the NavMesh (Screen Location: " + context.m_mousePos + " NavMesh Location: " + targetLocation);
            return false;
        }

        if(m_groundedNavMash.IsLocationOccupied(targetLocation))
        {
            Debug.Log("Jump Validation Failed: Location (" + targetLocation + ") is occupied");
            return false;
        }

        List<Vector3Int> validLocations = GetValidLocations();
        foreach (Vector3Int loc in validLocations)
        {
            if(targetLocation == loc)
                return true;
        }



        return false;
    }

    protected override void RenderUI()
    {
        List<Vector3Int> locations = GetValidLocations();

        
        
        UITileMapRequest request = new UITileMapRequest();
        request.header = UITileMapRequestHeader.PLACE_MANY;
        request.context = selectableContext;
        request.locations = locations;

        requestUITileMapChannel.Raise(request);
    }

    public override void RenderSelectionUI(AbilityUsageContext context)
    {
        Vector3Int targetLocation = Vector3Int.zero;
        if(m_airNavMesh.WorldPositionToTileLocation(context.m_mousePos, ref targetLocation))
        {

            List<Vector3Int> locations = GetValidLocations();

            foreach (Vector3Int loc in locations)
            {
                if(targetLocation == loc)
                {
                    UITileMapRequest request = new UITileMapRequest();
                    request.header = UITileMapRequestHeader.CLEAR;
                    request.context = selectedContext;

                    requestUITileMapChannel.Raise(request);

                    request.header = UITileMapRequestHeader.PLACE;
                    request.location = targetLocation;
                    
                    requestUITileMapChannel.Raise(request);
                }
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

    private List<Vector3Int> GetValidLocations()
    {
        List<Vector3Int> validLocations = new List<Vector3Int>();

        List<Vector3Int> airLocations = m_airNavMesh.GetRangeCordintates(m_location, 10);

        for (int i = 0; i < airLocations.Count; i++)
        {
            if(m_groundedNavMash.IsValidLocation(airLocations[i]))
                validLocations.Add(airLocations[i]);
        }

        return validLocations;
    }
}

