using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class UITileMapDrawer : MonoBehaviour
{
    public Tilemap canvasTileMap;
    public TileBase tileBase;
    public TileMapNavMesh referenceNavMesh;


    public void Clear()
    {     
        canvasTileMap.ClearAllTiles();
    }

    public void RadiusFill(Vector3Int origin, float radius)
    {
        List<Vector3Int> area = GetRadiusCordinates(origin, radius);
        
        foreach (Vector3Int point in area)
        {
            TryPlaceTile(point);
        }
    }

    public void GridFill(Vector3Int origin, int maxDistance)
    {
        List<Vector3Int> cords = GetGridCordinates(origin, maxDistance);

        foreach (Vector3Int cord in cords)
        {
            ForcePlaceTile(cord);
        }
    }

    public void LineFill(Vector3Int origin, Vector3 direction, int maxDistance = 3)
    {
        if(maxDistance <= 0) return;
        
        Vector3Int gridDirection = Vector3Int.RoundToInt(direction.normalized);
        Vector3Int endLocation = origin + (gridDirection * maxDistance);

        for (int i = 0; i < maxDistance; i++)
        {
            Vector3Int location = origin + (gridDirection * i);
            
            if(!TryPlaceTile(location)) return;
        }
    }

    public List<Vector3Int> GetRadiusCordinates(Vector3Int origin, float radius)
    {
        List<Vector3Int> output = new List<Vector3Int>();
        
        Vector3Int startLocation = new Vector3Int(origin.x - (int)radius, origin.y - (int)radius, 0);
        Vector3Int endLocation = new Vector3Int(startLocation.x + (int)(radius * 2), startLocation.y + (int)(radius * 2), 0);

        

        for (int i = startLocation.x; i < endLocation.x; i++)
        { 
            for (int j = startLocation.y; j < endLocation.y; j++)
            {
                Vector3Int location = new Vector3Int(i, j, 0);
                float distance = Vector3Int.Distance(location, origin);
                if(distance <= radius) 
                {
                    output.Add(location);
                }
            }
        }

        return output;
    }

    public List<Vector3Int> GetGridCordinates(Vector3Int origin, int maxDistance)
    {
        List<Vector3Int> output = new List<Vector3Int>();

        Dictionary<Vector3Int, int> visited = new Dictionary<Vector3Int, int>();
        PriorityQueue<Vector3Int, int> frontier = new PriorityQueue<Vector3Int, int>();

        if(referenceNavMesh.IsValidLocation(origin))
        {
            frontier.Enqueue(origin, 0);
            visited[origin] = 0;
        }

        while(frontier.Count != 0)
        {
            Vector3Int current = frontier.Dequeue();

            List<Vector3Int> neighbors = GetNeighborCordinates(current);
            foreach (Vector3Int neighbor in neighbors)
            {
                int newDistance = visited[current] + 1;
                if(newDistance > maxDistance) continue;

                if(!visited.ContainsKey(neighbor) || newDistance < visited[neighbor])
                {
                    visited[neighbor] = newDistance;
                    frontier.Enqueue(neighbor, newDistance);
                }
            }
        }

        foreach (KeyValuePair<Vector3Int, int> entry in visited)
        {
            output.Add((Vector3Int)entry.Key);
        }

        return output;
    }

    public List<Vector3Int> GetNeighborCordinates(Vector3Int origin)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        
        Vector3Int up = origin + Vector3Int.up;
        Vector3Int down = origin + Vector3Int.down;
        Vector3Int left = origin + Vector3Int.left;
        Vector3Int right = origin + Vector3Int.right;

        if(referenceNavMesh.IsValidLocation(up)) neighbors.Add(up);
        if(referenceNavMesh.IsValidLocation(down)) neighbors.Add(down);
        if(referenceNavMesh.IsValidLocation(left)) neighbors.Add(left);
        if(referenceNavMesh.IsValidLocation(right)) neighbors.Add(right);

        return neighbors;
    }

    public bool TryPlaceTile(Vector3Int location)
    {
        if(referenceNavMesh.IsValidLocation(location)) 
        {
            canvasTileMap.SetTile(location, tileBase);
            return true;
        }
        else return false;
    }

    private void ForcePlaceTile(Vector3Int location)
    {
        canvasTileMap.SetTile(location, tileBase);
    }
}