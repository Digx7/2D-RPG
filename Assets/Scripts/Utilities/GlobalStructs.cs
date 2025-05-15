using UnityEngine;
using System;

public class GlobalStructs : MonoBehaviour
{
    
}

[System.Serializable]
public struct StringAndGameObject
{
    public string name;
    public GameObject obj;

    public void Clear()
    {
        name = "";
        obj = null;
    }
}

public struct PlayerSpawnInfo
{
    public int ID;
    public Vector3 location;
    public Quaternion rotation;

    public void Clear()
    {
        ID = 0;
        location = Vector3.zero;
        rotation = Quaternion.identity;
    }
}

[System.Serializable]
public struct SceneContext
{
    public int SpawnPointID;
    public SceneCameraMode sceneCameraMode;

    public void Clear()
    {
        SpawnPointID = 0;
        sceneCameraMode = SceneCameraMode.FollowPlayer;
    }
}

[System.Serializable]
public struct SceneData
{
    public string sceneName;
    public SceneContext context;

    public void Clear()
    {
        sceneName = "";
        context.Clear();
    }
}