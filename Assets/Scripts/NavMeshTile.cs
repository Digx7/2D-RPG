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
    public NavMeshTileType m_type;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_Sprite;
        tileData.gameObject = m_Prefab;
        
        base.GetTileData(position, tilemap, ref tileData);
    }
}

public enum NavMeshTileType {FLOOR, DIFFICULT_TERRAIN, HAZARD};
