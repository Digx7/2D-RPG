using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileNavMeshPoint
{
}

[System.Serializable]
public struct TileNavMeshNode
{
    public Vector3Int position;
    public List<TileNavMeshEdge> links;
    public List<NavMeshTileFlags> flags;

    public TileNavMeshNode(Vector3Int newPosition)
    {
        position = newPosition;
        links = new List<TileNavMeshEdge>();
        flags = new List<NavMeshTileFlags>();

        
    }

    public void SetFlags(List<NavMeshTileFlags> newFlags)
    {
        flags = newFlags;
    }

    public void GiveLink(TileNavMeshNode otherNode)
    {
        TileNavMeshEdge edge;
        edge.otherEnd = otherNode.position;
        edge.linkType = TileNavMeshLinkType.NORMAL;
        links.Add(edge);
    }

    public void GiveLink(TileNavMeshEdge link)
    {
        links.Add(link);
    }

    public static bool operator ==(TileNavMeshNode n1, TileNavMeshNode n2)
    {
        if(n1.position == n2.position) return true;
        else return false;
    }

    public static bool operator !=(TileNavMeshNode n1, TileNavMeshNode n2)
    {
        if(n1.position == n2.position) return false;
        else return true;
    }
}

[System.Serializable]
public struct TileNavMeshEdge
{
    public Vector3Int otherEnd;
    public TileNavMeshLinkType linkType;
}

[System.Serializable]
public enum TileNavMeshLinkType {NORMAL, JUMP, FALL, CLIMB};