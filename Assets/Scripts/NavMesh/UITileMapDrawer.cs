using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class UITileMapDrawer : MonoBehaviour
{
    public List<UITileMapLookUp> allUITileMapLookUpData;

    public UITileMapRequestChannel requestUITileMapChannel;

    public void OnEnable()
    {
        requestUITileMapChannel.channelEvent.AddListener(RecieveRequest);
    }

    public void OnDisable()
    {
        requestUITileMapChannel.channelEvent.RemoveListener(RecieveRequest);
    }

    public void RecieveRequest(UITileMapRequest request)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(request.context))
            {
                switch (request.header)
                {
                    case UITileMapRequestHeader.FILL:
                        mapLookUp.Fill(request.location, request.range);
                        break;
                    case UITileMapRequestHeader.FILL_FILTERED:
                        mapLookUp.FillFiltered(request.location, request.range, request.linkList);
                        break;
                    case UITileMapRequestHeader.FILL_MASKED:
                        mapLookUp.FillMasked(request.location, request.range, request.linkList);
                        break;
                    case UITileMapRequestHeader.DRAWLINE:
                        mapLookUp.DrawLine(request.location, request.direction, request.range);
                        break;
                    case UITileMapRequestHeader.PATH:
                        mapLookUp.DrawPath(request.locations[0], request.locations[1], request.range);
                        break;
                    case UITileMapRequestHeader.PLACE:
                        mapLookUp.TryPlaceTile(request.location);
                        break;
                    case UITileMapRequestHeader.PLACE_MANY:
                        mapLookUp.PlaceTiles(request.locations);
                        break;
                    case UITileMapRequestHeader.CLEAR:
                        mapLookUp.Clear();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ClearAll()
    {     
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            mapLookUp.Clear();
        }
    }

    public void Clear(UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.Clear();
        }
    }

    public void Fill(Vector3Int location, int range, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.Fill(location, range);
        }
    }

    public void FillFiltered(Vector3Int location, int range, List<TileNavMeshLinkType> filteredLinks, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.FillFiltered(location, range, filteredLinks);
        }
    }

    public void FillMasked(Vector3Int location, int range, List<TileNavMeshLinkType> maskedLinks, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.FillMasked(location, range, maskedLinks);
        }
    }

    public void DrawLine(Vector3Int location, Vector3Int direction, int range, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.DrawLine(location, direction, range);
        }
    }
    
    public void DrawPath(Vector3Int startingLocation, Vector3Int endingLocation, int range, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) mapLookUp.DrawPath(startingLocation, endingLocation, range);
        }
    }

    public bool TryPlaceTile(Vector3Int location, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) 
            {
                return mapLookUp.TryPlaceTile(location);
            }
        }

        return false;
    }

    private void ForcePlaceTile(Vector3Int location, UITileMapContext context)
    {
        foreach (UITileMapLookUp mapLookUp in allUITileMapLookUpData)
        {
            if(mapLookUp.IsValidContext(context)) 
            {
                mapLookUp.PlaceTile(location);
            }
        }
    }
}

[System.Serializable]
public enum UITileMapCanvasType {SELECTABLE, SELECTED, LINE}

[System.Serializable]
public enum UITileMapType {GROUND, AIR}

[System.Serializable]
public struct UITileMapLookUp
{
    public Tilemap canvasMap;
    public TileMapNavMesh refrenceNavMesh;
    public TileBase uiTile;
    public UITileMapCanvasType canvasType;
    public UITileMapType tileMapType; 

    public bool IsValidContext(UITileMapContext context)
    {
        if(context.canvasType == canvasType && 
            context.tileMapType == tileMapType) 
                return true;
        else
            return false;
    }

    public void Clear()
    {
        canvasMap.ClearAllTiles();
    }

    public bool Fill(Vector3Int location, int range)
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        locations = refrenceNavMesh.GetRangeCordintates(location, range);

        if(locations.Count <= 0) return false;

        foreach (Vector3Int loc in locations)
        {
            PlaceTile(loc);
        }

        return true;
    }

    public bool FillFiltered(Vector3Int location, int range, List<TileNavMeshLinkType> filteredLinks)
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        locations = refrenceNavMesh.GetFilteredRangeCordinates(location, range, filteredLinks);

        if(locations.Count <= 0) return false;

        foreach (Vector3Int loc in locations)
        {
            PlaceTile(loc);
        }

        return true;
    }

    public bool FillMasked(Vector3Int location, int range, List<TileNavMeshLinkType> maskedLinks)
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        locations = refrenceNavMesh.GetMaskedRangeCordinates(location, range, maskedLinks);

        if(locations.Count <= 0) return false;

        foreach (Vector3Int loc in locations)
        {
            PlaceTile(loc);
        }

        return true;
    }

    public bool DrawLine(Vector3Int location, Vector3Int direction, int range)
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        locations = refrenceNavMesh.GetLineCordinates(location, direction, range);

        if(locations.Count <= 0) return false;

        foreach (Vector3Int loc in locations)
        {
            PlaceTile(loc);
        }

        return true;
    }

    public bool DrawPath(Vector3Int startingLocation, Vector3Int endingLocation, int range)
    {
        List<Vector3Int> locations = new List<Vector3Int>();
        locations = refrenceNavMesh.GetPathCordinates(startingLocation, endingLocation, range);

        if(locations.Count <= 0) return false;

        foreach (Vector3Int loc in locations)
        {
            PlaceTile(loc);
        }

        return true;
    }

    public void PlaceTiles(List<Vector3Int> locations)
    {
        foreach (Vector3Int location in locations)
        {
            if(refrenceNavMesh.IsValidLocation(location)) PlaceTile(location);
        }
    }

    public void PlaceTile(Vector3Int location)
    {
        canvasMap.SetTile(location, uiTile);
    }

    public bool TryPlaceTile(Vector3Int location)
    {
        if(refrenceNavMesh.IsValidLocation(location))
        {
            PlaceTile(location);
            return true;
        }
        
        return false;
    }
}

[System.Serializable]
public struct UITileMapContext
{
    public UITileMapCanvasType canvasType;
    public UITileMapType tileMapType;
}

[System.Serializable]
public struct UITileMapRequest
{
    public UITileMapRequestHeader header;
    public Vector3Int location;
    public List<Vector3Int> locations;
    public Vector3Int direction;
    public int range;
    public List<TileNavMeshLinkType> linkList;
    public UITileMapContext context;
}

[System.Serializable]
public enum UITileMapRequestHeader {FILL, FILL_FILTERED, FILL_MASKED, DRAWLINE, PLACE, PLACE_MANY, PATH, CLEAR}

public class UITileMapRequestEvent : UnityEvent<UITileMapRequest> {}