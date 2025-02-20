using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class MyScriptableTile : TileBase
{
    public Sprite m_Sprite;
    public GameObject m_Prefab;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_Sprite;
        tileData.gameObject = m_Prefab;
    }
    
    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        Debug.Log("MyScriptableTile: Hello World!");
        return base.StartUp(location, tilemap, go);
    }
}