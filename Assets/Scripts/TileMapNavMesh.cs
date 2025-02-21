using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;

[RequireComponent(typeof(Tilemap))]
public class TileMapNavMesh : MonoBehaviour
{
    [SerializeField] private List<TileNavMeshPoint> points;
    [SerializeField] private BoundsInt area;
    private Tilemap tilemap;

    public Vector3Int TestStartSpot;
    public Vector3Int TestEndSpot;

    public void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Start()
    {
        Color color = tilemap.color;
        color.a = 0f;
        tilemap.color = color;


        List<TileNavMeshPoint> path = new List<TileNavMeshPoint>();
        if(GetPath(TestStartSpot, TestEndSpot, ref path))
        {
            Debug.Log("Found Path and traversing");
            foreach (TileNavMeshPoint point in path)
            {
                Debug.Log("" + point.position);
            }
        }
        else
        {
            Debug.Log("Failed to find path");
        }
    }

    [ContextMenu("Bake")]
    public void Bake()
    {
        points.Clear();
        tilemap = GetComponent<Tilemap>();

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

    public TileNavMeshPoint GetPointAt(Vector3Int location)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(location == points[i].position) return points[i];
        }

        return null;
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
                    TileNavMeshPoint tileNavMeshPoint = new TileNavMeshPoint(location);
                    points.Add(tileNavMeshPoint);
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

    public List<TileNavMeshPoint> GetNeighbors(Vector3Int location)
    {
        List<TileNavMeshPoint> neighbors = new List<TileNavMeshPoint>();
        
        Vector3Int left = location + Vector3Int.left;
        Vector3Int upLeft = location + Vector3Int.up + Vector3Int.left;
        Vector3Int up = location + Vector3Int.up;
        Vector3Int upRight = location + Vector3Int.up + Vector3Int.right;
        Vector3Int right = location + Vector3Int.right;
        Vector3Int downRight = location + Vector3Int.down + Vector3Int.right;
        Vector3Int down = location + Vector3Int.down;
        Vector3Int downLeft = location + Vector3Int.down + Vector3Int.left;

        if(IsValidLocation(left)) neighbors.Add(GetPointAt(left));
        if(IsValidLocation(upLeft)) neighbors.Add(GetPointAt(upLeft));
        if(IsValidLocation(up)) neighbors.Add(GetPointAt(up));
        if(IsValidLocation(upRight)) neighbors.Add(GetPointAt(upRight));
        if(IsValidLocation(right)) neighbors.Add(GetPointAt(right));
        if(IsValidLocation(downRight)) neighbors.Add(GetPointAt(downRight));
        if(IsValidLocation(down)) neighbors.Add(GetPointAt(down));
        if(IsValidLocation(downLeft)) neighbors.Add(GetPointAt(downLeft));

        return neighbors;
    }


    public bool GetPath(Vector3Int startLocation, Vector3Int endLocation, ref List<TileNavMeshPoint> path)
    {
        if(!IsValidLocation(startLocation) || !IsValidLocation(endLocation)) return false;

        path.Clear();

        TileNavMeshPoint startPoint = GetPointAt(startLocation);
        TileNavMeshPoint endPoint = GetPointAt(endLocation);

        // A star
        PriorityQueue<TileNavMeshPoint, int> frontier = new PriorityQueue<TileNavMeshPoint, int>();
        frontier.Enqueue(startPoint, 0);
        Dictionary<TileNavMeshPoint, TileNavMeshPoint> cameFrom = new Dictionary<TileNavMeshPoint, TileNavMeshPoint>();
        Dictionary<TileNavMeshPoint, int> costSoFar = new Dictionary<TileNavMeshPoint, int>();
        cameFrom[startPoint] = null;
        costSoFar[startPoint] = 0;

        while(frontier.Count  != 0)
        {
            TileNavMeshPoint current = frontier.Dequeue();

            if (current == endPoint)
                break;
            
            List<TileNavMeshPoint> neighbors = GetNeighbors(current.position);
            foreach (TileNavMeshPoint next in neighbors)
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
            TileNavMeshPoint x = endPoint;

            path.Add(x);

            while (x != startPoint)
            {
                x = cameFrom[x];
                path.Add(x);
            }

            path.Reverse();
            return true;
        }
        else return false;

        
    }
}