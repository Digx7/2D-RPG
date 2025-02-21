using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class NavMeshTile : TileBase
{
    public Sprite m_Sprite;
    public GameObject m_Prefab;

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        // Add code here

        return base.GetTileAnimationData(location, tileMap, ref tileAnimationData);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_Sprite;
        tileData.gameObject = m_Prefab;
        
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // Add code here

        base.RefreshTile(position, tilemap);
    }

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        //  Add code here

        return base.StartUp(location, tilemap, go);
    }
}
