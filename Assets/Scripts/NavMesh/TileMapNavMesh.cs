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
                    TileNavMeshNode tileNavMeshNode = new TileNavMeshNode(location);
                    tileNavMeshNode.SetFlags(tilemap.GetTile<NavMeshTile>(location).flags);
                    points.Add(tileNavMeshNode);
                }
            }
        }
    }

    private void LinkAllPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            LinkWalk(i);
            LinkFall(i);
        }
    }

    private void LinkWalk(int pointIndex)
    {
        Vector3Int location = points[pointIndex].position;

        // Walk Links

        List<Vector3Int> neighbors = GetNeighborCordinates(location);

        foreach (Vector3Int neighbor in neighbors)
        {
            int j;
            if(IsValidLocation(neighbor, out j))
            {
                TileNavMeshEdge edge;
                edge.otherEnd = points[j].position;
                edge.linkType = TileNavMeshLinkType.NORMAL;
                
                points[pointIndex].GiveLink(edge);
            }
        }
    }

    private void LinkFall(int pointIndex)
    {
        Vector3Int location = points[pointIndex].position;
        
        if(points[pointIndex].flags.Contains(NavMeshTileFlags.FLOOR_EDGE))
            {
                Vector3Int left = location + Vector3Int.left;
                Vector3Int right = location + Vector3Int.right;

                if(!IsValidLocation(left))
                {
                    for (int j = left.y; j >= area.y; j--)
                    {
                        Vector3Int landingLocation = left;
                        landingLocation.y = j;
                        int landingIndex;

                        if(IsValidLocation(landingLocation, out landingIndex))
                        {
                            TileNavMeshEdge edge;
                            edge.otherEnd = points[landingIndex].position;
                            edge.linkType = TileNavMeshLinkType.FALL;
                    
                            points[pointIndex].GiveLink(edge);
                            break;
                        }
                    }
                }
                else if(!IsValidLocation(right))
                {
                    for (int j = right.y; j >= area.y; j--)
                    {
                        Vector3Int landingLocation = right;
                        landingLocation.y = j;
                        int landingIndex;

                        if(IsValidLocation(landingLocation, out landingIndex))
                        {
                            TileNavMeshEdge edge;
                            edge.otherEnd = points[landingIndex].position;
                            edge.linkType = TileNavMeshLinkType.FALL;
                    
                            points[pointIndex].GiveLink(edge);
                            break;
                        }
                    }
                }
            }
    }

    public List<Vector3Int> GetNeighborCordinates(Vector3Int location)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        neighbors.Add(location + Vector3Int.left);
        neighbors.Add(location + Vector3Int.up);
        neighbors.Add(location + Vector3Int.right);
        neighbors.Add(location + Vector3Int.down);
        // neighbors.Add(location + Vector3Int.up + Vector3Int.left);
        // neighbors.Add(location + Vector3Int.up + Vector3Int.right);
        // neighbors.Add(location + Vector3Int.down + Vector3Int.left);
        // neighbors.Add(location + Vector3Int.down + Vector3Int.right);

        return neighbors;
    }

    public bool IsValidLocation(Vector3Int location)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(location == points[i].position) return true;
        }

        return false;
    }

    public bool IsValidLocation(Vector3Int location, out int index)
    {
        index = -1;

        for (int i = 0; i < points.Count; i++)
        {
            if(location == points[i].position) 
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    public List<TileNavMeshNode> GetNeighbors(Vector3Int location)
    {
        List<TileNavMeshNode> neighbors = new List<TileNavMeshNode>();
        
        Vector3Int left = location + Vector3Int.left;
        Vector3Int up = location + Vector3Int.up;
        Vector3Int right = location + Vector3Int.right;
        Vector3Int down = location + Vector3Int.down;

        if(IsValidLocation(left)) neighbors.Add(GetPointAt(left));
        if(IsValidLocation(up)) neighbors.Add(GetPointAt(up));
        if(IsValidLocation(right)) neighbors.Add(GetPointAt(right));
        if(IsValidLocation(down)) neighbors.Add(GetPointAt(down));

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
            
            // List<TileNavMeshNode> neighbors = GetNeighbors(current.position);

            List<TileNavMeshEdge> links = current.links;
            foreach (TileNavMeshEdge link in links)
            {
                TileNavMeshNode next = GetPointAt(link.otherEnd);
                
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

    // Gets a Path ignoring all links if filteredLinks
    public bool GetFilteredPath(Vector3Int startLocation, Vector3Int endLocation, List<TileNavMeshLinkType> filteredLinks, ref List<TileNavMeshNode> path)
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
            
            // List<TileNavMeshNode> neighbors = GetNeighbors(current.position);

            List<TileNavMeshEdge> links = current.links;
            foreach (TileNavMeshEdge link in links)
            {
                if(filteredLinks.Contains(link.linkType)) continue;
                
                TileNavMeshNode next = GetPointAt(link.otherEnd);
                
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

    // Gets a Path of ONLY links found in maskedLinks
    public bool GetMaskedPath(Vector3Int startLocation, Vector3Int endLocation, List<TileNavMeshLinkType> maskedLinks, ref List<TileNavMeshNode> path)
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
            
            // List<TileNavMeshNode> neighbors = GetNeighbors(current.position);

            List<TileNavMeshEdge> links = current.links;
            foreach (TileNavMeshEdge link in links)
            {
                if(!maskedLinks.Contains(link.linkType)) continue;
                
                TileNavMeshNode next = GetPointAt(link.otherEnd);
                
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

    public List<NavMeshTileFlags> GetPointFlags(Vector3Int location)
    {
        if(!IsValidLocation(location)) return null;
        
        return GetPointAt(location).flags;
    }
}