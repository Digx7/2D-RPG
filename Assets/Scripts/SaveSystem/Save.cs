using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
[KnownType(typeof(float[]))]
[KnownType(typeof(Vector3))]
[KnownType(typeof(int))]
[KnownType(typeof(Dictionary<string, string>))]
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

        Dictionary<string,string> savedQuests = new Dictionary<string, string>();
        UpdateData<Dictionary<string,string>>("Quests", savedQuests);

        UpdateData<Vector3>("PlayerPos", playerPos);
        UpdateData<float>("Health", health);
        UpdateData<int>("Level", level);
    }

    public void SaveQuest(string questName, string questNode)
    {
        Dictionary<string,string> savedQuests = GetData<Dictionary<string,string>>("Quests");

        savedQuests[questName] = questNode;

        UpdateData<Dictionary<string,string>>("Quests", savedQuests);
    }

    public void FinishQuest(string questName)
    {
        Dictionary<string,string> savedQuests = GetData<Dictionary<string,string>>("Quests");

        savedQuests.Remove(questName);

        UpdateData<Dictionary<string,string>>("Quests", savedQuests);
    }

    public Dictionary<string,string> GetSavedQuests()
    {
        return GetData<Dictionary<string,string>>("Quests");
    }
}
