using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileNavMeshPoint
{
    public Vector3Int position;

    public List<TileNavMeshPoint> links;

    public TileNavMeshPoint(Vector3Int newPosition)
    {
        position = newPosition;
        links = new List<TileNavMeshPoint>();
    }

    public void GiveLink(TileNavMeshPoint pointToLink)
    {
        links.Add(pointToLink);
    }
}