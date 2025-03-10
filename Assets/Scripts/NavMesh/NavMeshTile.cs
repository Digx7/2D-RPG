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
    public List<NavMeshTileFlags> flags;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_Sprite;
        tileData.gameObject = m_Prefab;
        
        base.GetTileData(position, tilemap, ref tileData);
    }
}

[System.Serializable]
public enum NavMeshTileFlags {FLOOR_EDGE, DIFFICULT_TERRAIN, HAZARD};
