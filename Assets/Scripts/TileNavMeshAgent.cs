using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;


public class TileNavMeshAgent : MonoBehaviour
{
    public TileMapNavMesh tileMapNavMesh;
    public float speed = 1f;
    // [SerializeField] private Vector3Int m_startLocation;
    [SerializeField] private Vector3Int m_endLocation;

    private Camera cam;

    public void Start()
    {
        // TryToMove(m_endLocation);
        cam = Camera.main;
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            if(tileMapNavMesh.WorldPositionToTileLocation(mousePosition, ref m_endLocation))
            {
                TryToMove(m_endLocation);
            }
        }
    }
    
    public void TryToMove(Vector3Int endLocation)
    {
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

    IEnumerator Move(List<TileNavMeshNode> path)
    {
        Debug.Log("TileNavMeshAgent: Move()");
        // yield return new WaitForSeconds(1f);
        
        int index = 0;
        float timer = 0f;

        while(index < (path.Count - 1))
        {
            // Debug.Log("TileNavMeshAgent: Moved to a new spot");
            // transform.position = tileMapNavMesh.TileLocationToWorldPosition(path[index].position);
            // index++;
            // yield return new WaitForSeconds(1f/speed);

            Vector3 a = tileMapNavMesh.TileLocationToWorldPosition(path[index].position);
            Vector3 b = tileMapNavMesh.TileLocationToWorldPosition(path[index + 1].position);

            timer += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(a,b, timer);

            if(timer > 1f)
            {
                index++;
                timer = 0;
            }
            yield return null;
        }
    }
}