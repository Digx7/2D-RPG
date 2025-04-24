using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class TileNavMeshAgent : MonoBehaviour
{
    public float speed = 1f;
    public Vector3Int location;
    public string NavMeshName;
    public UnityEvent OnReachEndOfPath;
    public TileMapNavMesh tileMapNavMesh{get; private set;}

    public void Start()
    {
        LookForNavMesh();
        RefreshLocation();
    }

    public void LookForNavMesh()
    {
        foreach (TileMapNavMesh navMesh in GameObject.FindObjectsByType<TileMapNavMesh>(FindObjectsSortMode.None))
        {
            if(navMesh.gameObject.name == NavMeshName) tileMapNavMesh = navMesh;
        }

        if(tileMapNavMesh == null)
        {
            Debug.Log("TileNavMeshAgent: No Nav Mesh titled " + NavMeshName + " found \nCheck the spelling of the nav mesh names");
        }
    }
    
    public void TryTeleportToWorldPosition(Vector3 worldPosition)
    {
        if(tileMapNavMesh == null) 
        {
            Debug.Log("TileNavMeshAgent: TryTeleportToWorldPosition() FAILED because tileMapNavMesh == null");
            return;
        }

        Debug.Log("TileNavMeshAgent: TryTeleportToWorldPosition()");

        Vector3Int endlocation = Vector3Int.zero;

        if(!tileMapNavMesh.WorldPositionToTileLocation(worldPosition, ref endlocation))
        {
            Debug.Log("TileNavMeshAgent: Given end position is not on nav mesh");
            return;
        }
        else
        {
            transform.position = endlocation;
            location = endlocation;
        }
    }

    public void TryToMoveToWorldPosition(Vector3 worldEndPosition)
    {
        if(tileMapNavMesh == null) 
        {
            Debug.Log("TileNavMeshAgent: TryToMoveToWorldPosition() FAILED because tileMapNavMesh == null");
            return;
        }

        Debug.Log("TileNavMeshAgent: TryToMoveToWorldPosition()");

        Vector3Int endlocation = Vector3Int.zero;

        if(!tileMapNavMesh.WorldPositionToTileLocation(worldEndPosition, ref endlocation))
        {
            Debug.Log("TileNavMeshAgent: Given end position is not on nav mesh");
            return;
        }
        else
        {
            TryToMove(endlocation);
        }
    }

    public void TryToMove(Vector3Int endLocation)
    {
        if(tileMapNavMesh == null) {
            Debug.Log("TileNavMeshAgent: TryToMove() FAILED because tileMapNavMesh == null");
            return;
        }
        
        Debug.Log("TileNavMeshAgent: TryToMove()");

        List<TileNavMeshNode> path = new List<TileNavMeshNode>();

        Vector3Int startLocation = Vector3Int.zero;

        if(!tileMapNavMesh.WorldPositionToTileLocation(transform.position, ref startLocation))
        {
            Debug.Log("TileNavMeshAgent: Can't move cause agent is not on nav mesh");
            return;
        }

        if(tileMapNavMesh.GetPath(startLocation, endLocation, ref path))
        {
            Debug.Log("TileNavMeshAgent: Found a path");
            StartCoroutine(Move(path));
        }
        else
        {
            Debug.Log("TileNavMeshAgent: Failed to find a path");
        }
    }

    public void RefreshLocation()
    {
        if(tileMapNavMesh == null) LookForNavMesh();

        if(tileMapNavMesh != null)
        {
            tileMapNavMesh.WorldPositionToTileLocation(transform.position, ref location);
            transform.position = tileMapNavMesh.TileLocationToWorldPosition(location);
        }
    }

    IEnumerator Move(List<TileNavMeshNode> path)
    {
        Debug.Log("TileNavMeshAgent: Move()");
        
        int index = 0;
        float timer = 0f;

        while(index < (path.Count - 1))
        {

            Vector3 a = tileMapNavMesh.TileLocationToWorldPosition(path[index].position);
            Vector3 b = tileMapNavMesh.TileLocationToWorldPosition(path[index + 1].position);

            timer += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(a,b, timer);

            if(timer > 1f)
            {
                index++;
                timer = 0;
                location = path[index].position;
            }
            yield return null;
        }

        OnReachEndOfPath.Invoke();
    }
}