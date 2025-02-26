using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class TileMapNavMesh : MonoBehaviour
{
    [SerializeField] private List<TileNavMeshNode> points;
    [SerializeField] private BoundsInt area;
    public Tilemap tilemap;

    

    public void Start()
    {
        Color color = tilemap.color;
        color.a = 0f;
        tilemap.color = color;
    }

    [ContextMenu("Bake")]
    public void Bake()
    {
        ClearAllPoints();

        GatherAllPoints();
        LinkAllPoints();

        int numberOfLinks = 0;
        if(points.Count > 0)
        {
            for (int i = 0; i < points.Count; i++)
            {
                numberOfLinks += points[i].links.Count;
            }
        }
        Debug.Log("TileMapNavMesh: baked new NavMesh with " + points.Count + " points and " + numberOfLinks + " links");
    }

    [ContextMenu("Clear")]
    public void ClearAllPoints()
    {
        points.Clear();
        Debug.Log("TileMapNavMesh: cleared points");
    }

    public TileNavMeshNode GetPointAt(Vector3Int location)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(location == points[i].position) return points[i];
        }

        return points[0];
    }

    private void GatherAllPoints()
    {
        for (int x = area.x; x < area.xMax; x++)
        {
            for (int y = area.y; y < area.yMax; y++)
            {
                Vector3Int location = new Vector3Int(x,y,0);
                if(tilemap.HasTile(location))
                {
                    TileNavMeshNode TileNavMeshNode = new TileNavMeshNode(location);
                    points.Add(TileNavMeshNode);
                }
            }
        }
    }

    private void LinkAllPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3Int location = points[i].position;

            Vector3Int left = location + Vector3Int.left;
            Vector3Int upLeft = location + Vector3Int.up + Vector3Int.left;
            Vector3Int up = location + Vector3Int.up;
            Vector3Int upRight = location + Vector3Int.up + Vector3Int.right;
            Vector3Int right = location + Vector3Int.right;
            Vector3Int downRight = location + Vector3Int.down + Vector3Int.right;
            Vector3Int down = location + Vector3Int.down;
            Vector3Int downLeft = location + Vector3Int.down + Vector3Int.left;

            for (int j = 0; j < points.Count; j++)
            {
                if(points[j].position == left) points[i].GiveLink(points[j]);
                else if(points[j].position == upLeft) points[i].GiveLink(points[j]);
                else if(points[j].position == up) points[i].GiveLink(points[j]);
                else if(points[j].position == upRight) points[i].GiveLink(points[j]);
                else if(points[j].position == right) points[i].GiveLink(points[j]);
                else if(points[j].position == downRight) points[i].GiveLink(points[j]);
                else if(points[j].position == down) points[i].GiveLink(points[j]);
                else if(points[j].position == downLeft) points[i].GiveLink(points[j]);
            }
        }
    }

    public bool IsValidLocation(Vector3Int location)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(location == points[i].position) return true;
        }

        return false;
    }

    public List<TileNavMeshNode> GetNeighbors(Vector3Int location)
    {
        List<TileNavMeshNode> neighbors = new List<TileNavMeshNode>();
        
        Vector3Int left = location + Vector3Int.left;
        // Vector3Int upLeft = location + Vector3Int.up + Vector3Int.left;
        Vector3Int up = location + Vector3Int.up;
        // Vector3Int upRight = location + Vector3Int.up + Vector3Int.right;
        Vector3Int right = location + Vector3Int.right;
        // Vector3Int downRight = location + Vector3Int.down + Vector3Int.right;
        Vector3Int down = location + Vector3Int.down;
        // Vector3Int downLeft = location + Vector3Int.down + Vector3Int.left;

        if(IsValidLocation(left)) neighbors.Add(GetPointAt(left));
        // if(IsValidLocation(upLeft)) neighbors.Add(GetPointAt(upLeft));
        if(IsValidLocation(up)) neighbors.Add(GetPointAt(up));
        // if(IsValidLocation(upRight)) neighbors.Add(GetPointAt(upRight));
        if(IsValidLocation(right)) neighbors.Add(GetPointAt(right));
        // if(IsValidLocation(downRight)) neighbors.Add(GetPointAt(downRight));
        if(IsValidLocation(down)) neighbors.Add(GetPointAt(down));
        // if(IsValidLocation(downLeft)) neighbors.Add(GetPointAt(downLeft));

        return neighbors;
    }


    public bool GetPath(Vector3Int startLocation, Vector3Int endLocation, ref List<TileNavMeshNode> path)
    {
        if(!IsValidLocation(startLocation) || !IsValidLocation(endLocation)) 
        {
            Debug.Log("TileMapNavMesh: Failed to find path because the start or end location are out of bounds");
            return false;
        }

        path.Clear();

        TileNavMeshNode startPoint = GetPointAt(startLocation);
        TileNavMeshNode endPoint = GetPointAt(endLocation);

        // A star
        PriorityQueue<TileNavMeshNode, int> frontier = new PriorityQueue<TileNavMeshNode, int>();
        frontier.Enqueue(startPoint, 0);
        Dictionary<TileNavMeshNode, TileNavMeshNode> cameFrom = new Dictionary<TileNavMeshNode, TileNavMeshNode>();
        Dictionary<TileNavMeshNode, int> costSoFar = new Dictionary<TileNavMeshNode, int>();
        cameFrom[startPoint] = startPoint;
        costSoFar[startPoint] = 0;

        while(frontier.Count  != 0)
        {
            TileNavMeshNode current = frontier.Dequeue();

            if (current == endPoint)
                break;
            
            List<TileNavMeshNode> neighbors = GetNeighbors(current.position);
            foreach (TileNavMeshNode next in neighbors)
            {
                int newCost = costSoFar[current] + (int)Vector3Int.Distance(current.position, next.position);
                if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + (int)Vector3Int.Distance(next.position, endPoint.position);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        
        // runs backwards to get path
        if(cameFrom.ContainsKey(endPoint))
        {
            TileNavMeshNode x = endPoint;

            path.Add(x);

            while (x != startPoint)
            {
                x = cameFrom[x];
                path.Add(x);
            }

            path.Reverse();
            return true;
        }
        else 
        {
            Debug.Log("TileMapNavMesh: Failed to find path because destination is unreachable");
            return false;
        }

        
    }

    public Vector3 TileLocationToWorldPosition(Vector3Int location)
    {
        

        Vector3 result = location;

        result.x += 0.5f;
        result.y += 0.5f;

        return result;
    }

    public bool WorldPositionToTileLocation(Vector3 worldPosition, ref Vector3Int result)
    {

        result.x = (int)worldPosition.x;
        result.y = (int)worldPosition.y;
        result.z = 0;

        Debug.Log("TileMapNavMesh: World -> Tile: " + worldPosition + " -> " + result);

        return IsValidLocation(result);
    }
}