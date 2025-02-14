using UnityEngine;
using System.Runtime.Serialization;

[System.Serializable]
[KnownType(typeof(float[]))]
[KnownType(typeof(Vector3))]
[KnownType(typeof(int))]
[DataContract]
public class Save : Blackboard
{
    public void CreateDefaultSaveFile()
    {
        // Add generic values here
        // UpdateData<T>(key, value)

        Vector3 playerPos = new Vector3(1, 2, 4);

        float health = 12.2f;

        int level = 6;

        UpdateData<Vector3>("PlayerPos", playerPos);
        UpdateData<float>("Health", health);
        UpdateData<int>("Level", level);
    }
}
